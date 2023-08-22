using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProEventos.Application.DTOs;
using ProEventos.Application.Interface;
using ProEventos.Domain.Identity;
using ProEventos.Persistence.Interface;
using System;
using System.Threading.Tasks;

namespace ProEventos.Application.Concreta
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IMapper mapper;
        private readonly IUserPersistence userPersistence;

        public AccountService(UserManager<User> userManager, 
                              SignInManager<User> signInManager, 
                              IMapper mapper,
                              IUserPersistence userPersistence)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.mapper = mapper;
            this.userPersistence = userPersistence;
        }

        public async Task<SignInResult> CheckUserPassWordAsync(UserUpdateDTO userUpdateDTO, string password)
        {
            try
            {
                var user = await userManager.Users.SingleOrDefaultAsync(x => x.UserName.ToLower() == userUpdateDTO.Username.ToLower());

                return await signInManager.CheckPasswordSignInAsync(user, password, false);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao tentar verificar password. Erro: {ex.Message}");
            }
        }

        public async Task<UserUpdateDTO> CreateAccountAsync(UserDTO userDTO)
        {
            try
            {
                var user = mapper.Map<User>(userDTO);
                var result = await userManager.CreateAsync(user, userDTO.Password);

                if (result.Succeeded)
                {
                    return mapper.Map<UserUpdateDTO>(user);
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao tentar criar usuário. Erro: {ex.Message}");
            }
        }

        public async  Task<UserUpdateDTO> GetUserByUsernameAsync(string username)
        {
            try
            {
                var user = await userPersistence.GetUserByUsernameAsync(username);

                if (user == null)
                    return null;

                return mapper.Map<UserUpdateDTO>(user);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao tentar recuperar usuário por username. Erro: {ex.Message}");
            }
        }

        public async Task<UserUpdateDTO> UpdateAccount(UserUpdateDTO userUpdateDTO)
        {
            try
            {
                var user = await userPersistence.GetUserByUsernameAsync(userUpdateDTO.Username);

                if(user == null) 
                    return null;

                userUpdateDTO.Id = user.Id;

                mapper.Map(userUpdateDTO, user);

                user.SecurityStamp = Guid.NewGuid().ToString();

                if (userUpdateDTO.Password != null)
                {
                    var token = await userManager.GeneratePasswordResetTokenAsync(user);
                    await userManager.ResetPasswordAsync(user, token, userUpdateDTO.Password);
                }

                userPersistence.Update(user);

                if(await userPersistence.SaveChangesAsync())
                    return mapper.Map<UserUpdateDTO>(await userPersistence.GetUserByUsernameAsync(user.UserName));
                

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao tentar atualizar usuário. Erro: {ex.Message}");
            }
        }

        public async Task<bool> UserExists(string username)
        {
            try
            {
                return await userManager.Users.AnyAsync(x => x.UserName.ToLower() == username.ToLower());
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao tentar verificar se o usuário existe. Erro: {ex.Message}");
            }
        }
    }
}
