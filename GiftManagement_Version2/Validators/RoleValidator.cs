using FluentValidation;
using GiftManagement_Version2.Models;
using GiftManagement_Version2.Repositories;

namespace GiftManagement_Version2.Validators
{
    public class RoleValidator : AbstractValidator<RoleRequest>
    {
        private readonly IRepositoryWrapper repository;

        public RoleValidator(IRepositoryWrapper repository)
        {
            this.repository = repository;

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required")
                .Must((_, name) =>
                {
                    return !repository.Roles.FindByCondition(x => x.Name == name).Any();
                }).WithMessage("{PropertyName} already exists");
        }
    }
}
