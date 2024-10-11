using System;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.JsonMultipartFormDataSupport.Attributes;

namespace Swashbuckle.AspNetCore.JsonMultipartFormDataSupport.Integrations {
	/// <summary>
	/// Looks for field with <see cref="FromJsonAttribute"/> and use <see cref="JsonModelBinder"/> for binder.
	/// </summary>
	public class FormDataJsonBinderProvider(IOptions<JsonOptions> jsonOptions) : IModelBinderProvider
	{
		/// <inheritdoc />
		public IModelBinder GetBinder(ModelBinderProviderContext context) {
            ArgumentNullException.ThrowIfNull(context);

            // Do not use this provider for binding simple values
            if (!context.Metadata.IsComplexType) return null;

			// Do not use this provider if the binding target is not a property
			var propName = context.Metadata.PropertyName;
			if (propName is null) return null;
			var propInfo = context.Metadata.ContainerType?.GetProperty(propName);
			if (propInfo == null) return null;

			// Do not use this provider if the target property type implements IFormFile
			if (propInfo.PropertyType.IsAssignableFrom(typeof(IFormFile))) return null;

			// Do not use this provider if this property does not have the From attribute
			if (propInfo.GetCustomAttribute<FromJsonAttribute>() == null) return null;

			// All criteria met; use the FormDataJsonBinder
			return jsonOptions != null ? new JsonModelBinder(jsonOptions) : new JsonModelBinder();
		}
	}
}