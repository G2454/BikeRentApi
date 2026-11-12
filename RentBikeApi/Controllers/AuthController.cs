using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentBikeApi.Auth;
using RentBikeApi.Contracts;
using RentBikeApi.Contracts.Auth;
using RentBikeApi.Domain.Interfaces;

namespace RentBikeApi.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController(IAuthService service, JwtTokenGenerator tokenGenerator) : ControllerBase
    {
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await service.LoginAsync(request.Username, request.Password);
            if (result.IsFailure)
                return Unauthorized(new ErrorResponse(result.Error.Description, result.Error.Code, result.Error.Description));

            var token = tokenGenerator.Generate(result.Value);
            return Ok(new LoginResponse(token.Token, "Bearer", token.ExpiresAt));
        }
    }
}