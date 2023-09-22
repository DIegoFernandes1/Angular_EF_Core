using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProEventos.Api.Extensions;
using ProEventos.Application.DTOs;
using ProEventos.Application.Helpers;
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
        private readonly IUtil util;

        private readonly string destino = "Perfil";

        public AccountController(IAccountService accountService, ITokenService tokenService, IUtil util)
        {
            this.accountService = accountService;
            this.tokenService = tokenService;
            this.util = util;
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

                return Ok(new
                {
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
                    return Ok(new
                    {
                        userName = user.Username,
                        nome = user.Nome,
                        token = tokenService.CreateToken(user).Result
                    });

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
                if (userUpdateDTO.Username != User.GetUserName())
                    return Unauthorized("Usuário Inválido");

                var user = await accountService.GetUserByUsernameAsync(User.GetUserName());

                if (user == null)
                    return Unauthorized("Usuário inválido");

                var userResult = await accountService.UpdateAccount(userUpdateDTO);

                if (userResult == null)
                    return NoContent();

                return Ok(new
                {
                    userName = userResult.Username,
                    nome = userResult.Nome,
                    token = tokenService.CreateToken(userResult).Result
                });

            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar atualizar usuário. Erro: {ex.Message}");
            }
        }

        [HttpPost("UploadImage")]
        public async Task<IActionResult> UploadImage()
        {
            try
            {
                var user = await accountService.GetUserByUsernameAsync(User.GetUserName());
                if (user == null) return NoContent();

                var file = Request.Form.Files[0];

                if (file.Length > 0)
                {
                    util.DeleteImage(user.ImagemURL, destino);
                    user.ImagemURL = await util.SaveImage(file, destino);
                }

                var userRetorno = await accountService.UpdateAccount(user);

                return Ok(userRetorno);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status400BadRequest, $"Erro ao tentar alterar imagem. Erro: {ex.Message}");
            }
        }
    }
}