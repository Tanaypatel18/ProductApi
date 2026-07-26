using Microsoft.AspNetCore.Mvc;
using myFirstWebApi.DTOs;
using myFirstWebApi.Services;

namespace myFirstWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _service;

    public ProductsController(IProductService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_service.GetAll());
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var product = _service.GetById(id);

        if (product == null)
            return NotFound($"Product with id {id} not found");

        return Ok(product);
    }

    [HttpPost]
    public IActionResult Create([FromBody] CreateProductDto dto)
    {
        var product = _service.Create(dto);
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] UpdateProductDto dto)
    {
        var product = _service.Update(id, dto);

        if (product == null)
            return NotFound($"Product with id {id} not found");

        return Ok(product);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var result = _service.Delete(id);

        if (!result)
            return NotFound($"Product with id {id} not found");

        return NoContent();
    }
    [HttpGet("error-test")]
    public IActionResult TestError()
    {
        throw new Exception("This is a test error");
    }
}