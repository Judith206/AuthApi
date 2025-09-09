using AuthApi.DTOs.CategoriaJN;
using AuthApi.DTOs.CategoriaJNDTO;
using AuthApi.Entidades;

namespace AuthApi.Interfaces
{
    public interface ICategoriaJNRepository
    {
        Task<List<CategoriaJNListadoDto>> GetAllCategoriasJNAsync();
        Task<CategoriaJN?> GetCategoriaJNByIdAsync(int id);
        Task<CategoriaJN> AddCategoriaJNAsync(CategoriaJN categoria);
        Task<CategoriaJN> UpdateCategoriaJNAsync(CategoriaJN categoria);
        Task<bool> DeleteCategoriaJNAsync(int id);
    }
}
