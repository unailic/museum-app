using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Muzej.API.Service;
using Muzej.Domain.Entities;
using Muzej.Infrastructure.Identity;

namespace Muzej.API.Controllers
{
    public record RegisterRequest(string Email, string Password, string Ime, string Prezime, TipPosetioca TipPosetioca);
    public record LoginRequest(string Email, string Password);

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<Korisnik> _userManager;
        private readonly JwtTokenService _tokenService;

        public AuthController(UserManager<Korisnik> userManager, JwtTokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var korisnik = new Korisnik
            {
                UserName = request.Email,
                Email = request.Email,
                Ime = request.Ime,
                Prezime = request.Prezime,
                TipPosetioca = request.TipPosetioca
            };

            var result = await _userManager.CreateAsync(korisnik, request.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors.Select(e => e.Description));

            await _userManager.AddToRoleAsync(korisnik, "Posetilac");

            var token = await _tokenService.CreateTokenAsync(korisnik);
            return Ok(new { token });
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var korisnik = await _userManager.FindByEmailAsync(request.Email);
            if (korisnik == null)
                return Unauthorized();

            if (!await _userManager.CheckPasswordAsync(korisnik, request.Password))
                return Unauthorized();

            var token = await _tokenService.CreateTokenAsync(korisnik);
            return Ok(new { token });
        }
    }
}