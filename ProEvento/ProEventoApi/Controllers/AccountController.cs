using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProEventos.Api.Extensions;
using ProEventos.Application.DTOs;
using ProEventos.Application.Interface;
using System;
using System.Threading.Tasks;

namespace ProEventos.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService accountService;
        private readonly ITokenService tokenService;

        public AccountController(IAccountService accountService, ITokenService tokenService)
        {
            this.accountService = accountService;
            this.tokenService = tokenService;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginDTO userLoginDTO)
        {
            try
            {
                var user = await accountService.GetUserByUsernameAsync(userLoginDTO.Username);

                if (user == null)
                    return Unauthorized("Usuário ou Senha está inválido");

                var result = await accountService.CheckUserPassWordAsync(user, userLoginDTO.Password);

                if (!result.Succeeded)
                    return Unauthorized("Usuário ou Senha está inválido");

                return Ok(new { 
                    userName = user.Username,
                    nome = user.Nome,
                    token = tokenService.CreateToken(user).Result
                });
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar realizar login. Erro: {ex.Message}");
            }
        }

        [HttpGet("GetUser")]
        public async Task<IActionResult> GetUser()
        {
            try
            {
                var username = User.GetUserName();
                var user = await accountService.GetUserByUsernameAsync(username);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar dados do usuário. Erro: {ex.Message}");
            }
        }

        [HttpPost("RegisterUser")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterUser(UserDTO userDTO)
        {
            try
            {
                if (await accountService.UserExists(userDTO.Username))
                    return BadRequest("Usuário já existe");

                var user = await accountService.CreateAccountAsync(userDTO);
                
                if (user != null)
                    return Ok(user);

                return BadRequest("Erro ao tentar cadastrar usuário");

            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar cadastrar usuário. Erro: {ex.Message}");
            }
        }

        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser(UserUpdateDTO userUpdateDTO)
        {
            try
            {
                var user = await accountService.GetUserByUsernameAsync(User.GetUserName());

                if (user == null)
                    return Unauthorized("Usuário inválido");

                var userResult = await accountService.UpdateAccount(userUpdateDTO);

                if (userResult == null)
                    return NoContent();

                return Ok(userResult);

            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar atualizar usuário. Erro: {ex.Message}");
            }
        }
    }
}