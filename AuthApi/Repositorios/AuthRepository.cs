using AuthApi.DTOs.UsuarioDTOs;
using AuthApi.Entidades;
using AuthApi.Interfaces;
using BCrypt.Net;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthApi.Repositorios
{
    public class AuthRepository : IAuthService
    {
        private readonly IUsuarioRepository _usuarioRepo;
        private readonly IConfiguration _config;

        public AuthRepository (IUsuarioRepository usuariosRepo, IConfiguration config)
        {
            _usuarioRepo = usuariosRepo;
            _config = config;
        }

        public async Task<UsuarioRespuestaDto> RegistrarAsync(UsuarioRegistroDTO dto)
        {
            //Crear usuario con rolId 
            var usuario = new Usuario
            {
                Nombre = dto.Nombre,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                RolId = 2 //Usuario por defectos
            };

            //Guardar en la base de datos 

            await _usuarioRepo.AddAsync(usuario);

            //Recargar el usuario para incluir el rol 
            usuario = await _usuarioRepo.GetByEmailAsync(usuario.Email);

            //Generar Token 

            string token = GenerarToken(usuario);

            return new UsuarioRespuestaDto
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Email = usuario.Email,
                Rol = usuario.Rol?.Nombre ?? "Usuario",
                Token = token
            };
        }

        public async Task<UsuarioRespuestaDto?> LoginAsync(UsuarioLoginDto dto)
        {
            var usuario = await _usuarioRepo.GetByEmailAsync(dto.Email);
            if (usuario == null) return null;

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, usuario.PasswordHash)) 
                return null;

            return new UsuarioRespuestaDto
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Email = usuario.Email,
                Rol = usuario.Rol.Nombre,
                Token = GenerarToken(usuario)
            };
        }
        private string GenerarToken(Usuario usuario) 
        {
            if (usuario == null) throw new ArgumentNullException(nameof(usuario));
            if (usuario.Email == null) throw new InvalidOperationException("El usuario no tiene rol asignado.");

            //validar que usuario  y su rol no sean nulos 
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //definir los claims del token (email y rol)

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Email ?? ""),
                new Claim("rol", usuario.Rol.Nombre)
            };

            //Crear token JWT con issuer,audience, claims y expiracion
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(2),
                signingCredentials: creds
                );
            //Retornar token en formato string 

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
