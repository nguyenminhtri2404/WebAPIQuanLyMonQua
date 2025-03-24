using FluentValidation;
using GiftManagement_Version2.Data;
using GiftManagement_Version2.Models;
using GiftManagement_Version2.Repositories;

namespace GiftManagement_Version2.Validators
{
    public class CartValidator : AbstractValidator<CartRequest>
    {
        private readonly IRepositoryWrapper repository;

        public CartValidator(IRepositoryWrapper repository)
        {
            this.repository = repository;

            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("UserId is required")
                .Must(x => repository.Users.FindByCondition(u => u.Id == x) != null)
                .WithMessage("UserId is not exist");

            RuleFor(x => x.GiftId)
                .NotEmpty()
                .WithMessage("GiftId is required")
                .Must(x => repository.Gifts.FindByCondition(g => g.Id == x) != null)
                .WithMessage("GiftId is not exist");

            //Kiểm tra số lượng của request có lớn hơn số lượng trong kho hay không
            RuleFor(x => x.Quantity)
                .NotEmpty()
                .WithMessage("Quantity is required")
                .Must((x, quantity) =>
                {
                    Gift? gift = repository.Gifts.FindByCondition(g => g.Id == x.GiftId).FirstOrDefault();
                    return gift != null && gift.Quantity >= quantity;
                })
                .WithMessage("Quantity is not enough");

            //Kiểm tra thời gian của gift, nếu gift đã EndDate thì không cho mua
            RuleFor(x => x.GiftId)
                .Must(x =>
                {
                    Gift? gift = repository.Gifts.FindByCondition(g => g.Id == x).FirstOrDefault();
                    return gift != null && gift.EndDate > System.DateTime.Now;
                })
                .WithMessage("Gift is expired");

            //Kiểm tra chỉ cho phép mua quà chính, không cho phép mua quà tặng
            // Quà chính GiftType = 0, quà tặng GiftType = 1
            RuleFor(x => x.GiftId)
                .Must(x =>
                {
                    Gift? gift = repository.Gifts.FindByCondition(g => g.Id == x).FirstOrDefault();
                    return gift != null && gift.GiftType == 0;
                })
                .WithMessage("Gift is not allowed to buy");

        }
    }
}
