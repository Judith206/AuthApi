using AuthApi.Controllers.Repositorios;
using AuthApi.DTOs.CategoriaJN;
using AuthApi.Entidades;
using AuthApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuthApi.Repositorios
{
    public class CategoriaJNRepository : ICategoriaJNRepository
    {
        private readonly AppDbContext _context;

        public CategoriaJNRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<CategoriaJNListadoDto>> GetAllCategoriasJNAsync()
        {
            return await _context.CategoriasJN
                .Select(c => new CategoriaJNListadoDto
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    Descripcion = c.Descripcion
                })
                .ToListAsync();
        }

        public async Task<CategoriaJN?> GetCategoriaJNByIdAsync(int id)
        {
            return await _context.CategoriasJN.FindAsync(id);
        }

        public async Task<CategoriaJN> AddCategoriaJNAsync(CategoriaJN categoria)
        {
            _context.CategoriasJN.Add(categoria);
            await _context.SaveChangesAsync();
            return categoria;
        }

        public async Task<CategoriaJN> UpdateCategoriaJNAsync(CategoriaJN categoria)
        {
            _context.CategoriasJN.Update(categoria);
            await _context.SaveChangesAsync();
            return categoria;
        }

        public async Task<bool> DeleteCategoriaJNAsync(int id)
        {
            var categoria = await _context.CategoriasJN.FindAsync(id);
            if (categoria == null) return false;

            _context.CategoriasJN.Remove(categoria);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
