
using AuthApi.DTOs.CategoriaJNDTO;
using AuthApi.Entidades;
using AuthApi.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize] 
    public class CategoriaJNController : ControllerBase
    {
        private readonly ICategoriaJNRepository _categoriaJNRepository;

        public CategoriaJNController(ICategoriaJNRepository categoriaJNRepository)
        {
            _categoriaJNRepository = categoriaJNRepository;
        }

        // GET: api/CategoriaJN
        [HttpGet]
        public async Task<IActionResult> GetAllCategoriasJN()
        {
            var categorias = await _categoriaJNRepository.GetAllCategoriasJNAsync();
            return Ok(categorias);
        }

        // GET: api/CategoriaJN/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoriaJNById(int id)
        {
            var categoria = await _categoriaJNRepository.GetCategoriaJNByIdAsync(id);
            if (categoria == null) return NotFound();
            return Ok(categoria);
        }

        // POST: api/CategoriaJN
        [HttpPost]
        public async Task<IActionResult> CreateCategoriaJN([FromBody] CategoriaJNCreacionDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var categoria = new CategoriaJN
            {
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion
            };

            var result = await _categoriaJNRepository.AddCategoriaJNAsync(categoria);
            return CreatedAtAction(nameof(GetCategoriaJNById), new { id = result.Id }, result);
        }

        // PUT: api/CategoriaJN/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategoriaJN(int id, [FromBody] CategoriaJNActualizacionDto dto)
        {
            if (id != dto.Id) return BadRequest("ID no coincide");

            var existingCategoria = await _categoriaJNRepository.GetCategoriaJNByIdAsync(id);
            if (existingCategoria == null) return NotFound();

            existingCategoria.Nombre = dto.Nombre;
            existingCategoria.Descripcion = dto.Descripcion;

            var result = await _categoriaJNRepository.UpdateCategoriaJNAsync(existingCategoria);
            return Ok(result);
        }

        // DELETE: api/CategoriaJN/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoriaJN(int id)
        {
            var success = await _categoriaJNRepository.DeleteCategoriaJNAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
