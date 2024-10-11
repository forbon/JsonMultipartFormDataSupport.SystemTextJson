using System;
using System.Linq;
using System.Text.Json;
using Demo.Models.Products;
using Demo.Models.Wrapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.JsonMultipartFormDataSupport.Models;

namespace Demo.Controllers;

[Produces("application/json")]
[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase {
	[HttpPost]
	public IActionResult Post([FromForm] MultipartFormData<Product> data) {
		var json = data.Json ?? throw new NullReferenceException(nameof(data));
		var file = data.File;
		return Ok(new { json, file?.FileName });
	}

	[HttpPost("required")]
	public IActionResult Post([FromForm] MultipartRequiredFormData<Product> data) {
		var json = data.Json ?? throw new NullReferenceException(nameof(data));
		var file = data.File;
		return Ok(new { json, file?.FileName });
	}

	[HttpPost("wrapper/required")]
	public IActionResult Post([FromForm] RequiredProductWrapper wrapper) {
		var wrapperProduct = wrapper.Product ?? throw new NullReferenceException(nameof(wrapper.Product));
		var files = wrapper.Files;
		return Ok(new { wrapperProduct, files = files?.Select(a => a.FileName) });
	}

	[HttpPost("wrapper")]
	public IActionResult PostWrapper([FromForm] ProductWrapper wrapper) {
		var wrapperProduct = wrapper.Product ?? throw new NullReferenceException(nameof(wrapper.Product));
		var files = wrapper.Files;
		return Ok(new { wrapperProduct, files = files?.Select(a => a.FileName) });
	}

	[HttpPost("wrapper/simple")]
	public IActionResult PostWrapper([FromForm] SimpleProductWrapper wrapper) {
		var productName = wrapper.ProductName;
		var productId = wrapper.ProductId ?? throw new NullReferenceException(nameof(wrapper.ProductId));
		var files = wrapper.Files;
		return Ok(new { productName, productId, files = files?.Select(a => a.FileName) });
	}

	[HttpPost("wrapper/complex")]
	public IActionResult PostWrapper([FromForm] ComplexProductWrapper wrapper) {
		var productName = wrapper.ProductName;
		var productId = wrapper.ProductId ?? throw new NullReferenceException(nameof(wrapper.ProductId));
		var product = wrapper.Product;
		var files = wrapper.Files;
		return Ok(new {
			productName, 
			productId, 
			product = JsonSerializer.Serialize(product),
			files = files?.Select(a => a.FileName)
		});
	}
}