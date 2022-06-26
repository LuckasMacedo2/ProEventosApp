using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProEventos.Application.Contratos;
using ProEventos.Application.Dtos;
using ProEventos.Domain.Identity;
using ProEventos.Persistence;

namespace ProEventos.Application
{
    public class AccountService : IAccountService
    {
        public readonly UserManager<User> _userManager;
        public readonly SignInManager<User> _signInManager;
        public readonly IMapper _mapper;
        public readonly IUserPersist _userPersist;
        public AccountService(UserManager<User> userManager,
                              SignInManager<User> signInManager,
                              IMapper mapper,
                              IUserPersist userPersist)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _userPersist = userPersist;
        }
        public async Task<SignInResult> CheckUserPasswordAsync(UserUpdateDto userUpdateDto, string password)
        {
            try
            {
                var user = await _userManager
                                 .Users.SingleOrDefaultAsync(user => user.UserName == userUpdateDto.UserName.ToLower());
                return await _signInManager.CheckPasswordSignInAsync(user, password, false);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao tentar verificar a senha. Erro: {ex.Message}");
            }
        }

        public async Task<UserUpdateDto> CreateAccountAsync(UserDto userDto)
        {
            try
            {
                var user = _mapper.Map<User>(userDto);
                var result = await _userManager.CreateAsync(user, userDto.Password);

                if(result.Succeeded) // A criação da conta deu certo
                {
                    var userToReturn = _mapper.Map<UserUpdateDto>(user);
                    return userToReturn;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao criar usuário. Erro: {ex.Message}");
            }
        }

        public async Task<UserUpdateDto> GetUserByUserNameAsync(string userName)
        {
            try
            {
                var user = await _userPersist.GetUserByUserNameAsync(userName);
                if (user == null) return null;

                var userUpdateDto = _mapper.Map<UserUpdateDto>(user);
                return userUpdateDto; 
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao tentar obter usário por userName. Erro: {ex.Message}");
            }
        }

        public async Task<UserUpdateDto> UpdateAccount(UserUpdateDto userUpdateDto)
        {
            try
            {
                var user = await _userPersist.GetUserByUserNameAsync(userUpdateDto.UserName);
                if(user == null) return null;

                userUpdateDto.Id = user.Id;

                _mapper.Map(userUpdateDto, user);

                if(userUpdateDto.Password != null)
                {
                    // Resetando a senha
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user); // Gera o token para que o usuário se mantenha logado
                    await _userManager.ResetPasswordAsync(user, token, userUpdateDto.Password);
                }

                _userPersist.Update<User>(user);

                if(await _userPersist.SaveChangesAsync())
                {
                    var userRetorno = await _userPersist.GetUserByUserNameAsync(user.UserName);
                    return _mapper.Map<UserUpdateDto>(userRetorno);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao tentar obter a conta do usuário. Erro: {ex.Message}");
            }
        }

        public async Task<bool> UserExists(string userName)
        {
            try
            {
                return await _userManager.Users.AnyAsync(user => user.UserName == userName.ToLower());
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao tentar verificar se o usuário existe. Erro: {ex.Message}");
            }
        }
    }
}