using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using GiftManagement_Version2.Data;
using GiftManagement_Version2.Helpers;
using GiftManagement_Version2.Models;
using GiftManagement_Version2.Repositories;

namespace GiftManagement_Version2.Services
{
    public interface IUserServices
    {
        Task<string?> Register(UserRegisterRequest request);
        Task<string?> UpdateAccount(int id, UserRegisterRequest request);
        string? DeleteAccount(int id);
        UserRespone GetUserById(int id);
        List<UserRespone> GetAllUser();
        UserRespone GetProfile(int id, HttpContext context);
    }
    public class UserServices : IUserServices
    {
        private readonly IMapper mapper;
        private readonly IValidator<UserRegisterRequest> validator;
        private readonly IRepositoryWrapper repository;

        public UserServices(IMapper mapper, IValidator<UserRegisterRequest> validator, IRepositoryWrapper repository)
        {
            this.mapper = mapper;
            this.validator = validator;
            this.repository = repository;
        }

        public async Task<string?> Register(UserRegisterRequest request)
        {
            User user = mapper.Map<User>(request);
            //Validate request
            ValidationResult validationResult = validator.Validate(request);

            if (!validationResult.IsValid)
            {
                return validationResult.ToString();
            }

            string uploadDirectory = Path.Combine("wwwroot/images");

            string imagePath = await ImageHelper.SaveImageAsync(request.Image, uploadDirectory);

            if (imagePath != null)
            {
                // Set the image path in the user entity
                user.Image = imagePath;
            }
            // Save the user
            user.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
            repository.Users.Create(user);
            repository.Save();
            return null;
        }

        public UserRespone GetUserById(int id)
        {
            UserRespone? user = repository.Users.FindByCondition(u => u.Id == id)
                                         .Select(u => new UserRespone
                                         {
                                             Username = u.Username,
                                             Email = u.Email,
                                             Phone = u.Phone,
                                             Permissions = u.Role.RolePermissions.Select(rp => rp.Permission.Code.Trim()).ToList()
                                         })
                                         .FirstOrDefault();
            return user;
        }

        public List<UserRespone> GetAllUser()
        {
            List<UserRespone> listUser = repository.Users.FindAll()
                                         .Select(u => new UserRespone
                                         {
                                             Username = u.Username,
                                             Email = u.Email,
                                             Phone = u.Phone,
                                             DateOfBirth = u.DateOfBirth,
                                             Permissions = u.Role.RolePermissions.Select(rp => rp.Permission.Code.Trim()).ToList()
                                         })
                                         .ToList();
            return listUser;

        }

        public UserRespone GetProfile(int id, HttpContext context)
        {
            UserRespone? user = repository.Users.FindByCondition(u => u.Id == id)
                                         .Select(u => new UserRespone
                                         {
                                             Username = u.Username,
                                             Email = u.Email,
                                             Phone = u.Phone,
                                             DateOfBirth = u.DateOfBirth,
                                             Image = u.Image,
                                             Permissions = u.Role.RolePermissions.Select(rp => rp.Permission.Code.Trim()).ToList()
                                         })
                                         .FirstOrDefault();
            return user;
        }

        public async Task<string?> UpdateAccount(int id, UserRegisterRequest request)
        {
            User? user = repository.Users.FindByCondition(u => u.Id == id).FirstOrDefault();
            ValidationResult validationResult = validator.Validate(request);
            if (!validationResult.IsValid)
            {
                return validationResult.ToString();
            }

            if (user != null)
            {
                user = mapper.Map(request, user);

                string uploadDirectory = Path.Combine("wwwroot/images");

                string imagePath = await ImageHelper.SaveImageAsync(request.Image, uploadDirectory);

                if (imagePath != null)
                {
                    user.Image = imagePath;
                }

                user.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
                repository.Users.Update(user);
                repository.Save();
            }
            return null;
        }

        public string? DeleteAccount(int id)
        {
            User? user = repository.Users.FindByCondition(u => u.Id == id).FirstOrDefault();
            if (user == null)
            {
                return "User not found";
            }
            repository.Users.Delete(user);
            repository.Save();
            return null;
        }
    }
}
