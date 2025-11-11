using Microsoft.AspNetCore.Mvc;
using SimpleCrud.Api.Requests;
using SimpleCrud.Services;

namespace SimpleCrud.Api.Controllers
{
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService service;

        public ProductsController(ProductService service)
        {
            this.service = service;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await service.GetByIdAsync(id);
            if (!result.Success)
                return NotFound();
            return Ok(result.Data);
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAll()
        {
            var result = await service.GetAll();
            return Ok(result.Data);
        }
        [HttpPost("")]
        public async Task<IActionResult> Create([FromBody] ProductRequest product)
        {
            if (product == null && !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await service.CreateAsync(product.GetProduct());
            if (!result.Success)
                return UnprocessableEntity(result.Errors);

            return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result.Data);
        }

        [HttpPut("")]
        public async Task<IActionResult> Update([FromBody] UpdateProductRequest product)
        {
            if (product == null && !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await service.UpdateAsync(product.GetProduct());

            if (!result.Success)
                return UnprocessableEntity(result.Errors);

            return Ok(result.Data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await service.DeleteAsync(id);
            if (!result.Success)
                return NotFound(result.Message ?? result.Errors?.FirstOrDefault());
            return NoContent();
        }
    }
}
