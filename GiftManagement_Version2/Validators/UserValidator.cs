using FluentValidation;
using GiftManagement_Version2.Models;
using GiftManagement_Version2.Repositories;

namespace GiftManagement_Version2.Validators
{
    public class UserValidator : AbstractValidator<UserRegisterRequest>
    {
        private readonly IRepositoryWrapper repository;
        public UserValidator(IRepositoryWrapper repository)
        {
            this.repository = repository;

            RuleFor(x => x.Username)
                .NotEmpty()
                .WithMessage("Name is required")
                .Must((_, username) =>
                {
                    return !repository.Users.FindByCondition(x => x.Username == username).Any();
                }).WithMessage("{PropertyName} already exists");

            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required");
            RuleFor(x => x.Email)
                .EmailAddress()
                .WithMessage("Email is not valid")
                .Must((_, email) =>
                {
                    return !repository.Users.FindByCondition(x => x.Email == email).Any();
                }).WithMessage("{PropertyName} already exists");

            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required");
            RuleFor(x => x.Password).Matches(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*\W).{8,}$").WithMessage("Password is not valid");

            RuleFor(x => x.Phone)
                .Matches(@"^0(3|5|7|8|9)[0-9]{8}$")
                .WithMessage("Phone is not valid")
                .Must((_, phone) =>
                {
                    return !repository.Users.FindByCondition(x => x.Phone == phone).Any();
                }).WithMessage("{PropertyName} already exists");

        }
    }
}
