using System;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Contracts;
using Infrastructure.Result;
using Infrastructure.Validator;
using Models;
using Models.Dtos.User;
using Models.Error;
using Services.Contracts;

namespace Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher _passwordHasher;

        public UserService
        (
            IUserRepository repository, 
            IMapper mapper,
            IPasswordHasher passwordHasher
        )
        {
            _repository = repository;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
        }

        public async Task<ResultContainer<UserDto>> CreateUserAsync(UserCredentialsDto userDto)
        {
            var existingUser = await _repository.FindByEmailAsync(userDto.Email);
            var result = new ResultContainer<UserDto>();
            var isEmailValid = EmailValidator.EmailIsValid(userDto.Email);
            
            if(existingUser != null || !isEmailValid)
            {
                result.ErrorType = ErrorType.BadRequest;
                return result;
            }
            
            var user = _mapper.Map<UserCredentialsDto, User>(userDto);
            user.Password = _passwordHasher.HashPassword(user.Password);
            user.DateCreated = DateTime.Now;
            
            result = _mapper.Map<ResultContainer<UserDto>>(await _repository.Create(user));

            return result;
        }

        public async Task<ResultContainer<UserCredentialsDto>> FindByEmailAsync(string email)
        {
            var user = await _repository.FindByEmailAsync(email);
            var result = new ResultContainer<UserCredentialsDto>();
            
            if (user == null)
            {
                result.ErrorType = ErrorType.BadRequest;
                return result;
            }  
            
            result = _mapper.Map<ResultContainer<UserCredentialsDto>>(user);
            return result;
        }
        
        public async Task<ResultContainer<UserCredentialsDto>> FindByIdAsync(int id)
        {
            var user = await _repository.GetById(id);
            var result = new ResultContainer<UserCredentialsDto>();
            
            if (user == null)
            {
                result.ErrorType = ErrorType.BadRequest;
                return result;
            }  
            
            result = _mapper.Map<ResultContainer<UserCredentialsDto>>(user);
            return result;
        }
    }
}