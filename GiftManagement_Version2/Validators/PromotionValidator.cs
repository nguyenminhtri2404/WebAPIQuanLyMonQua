using FluentValidation;
using GiftManagement_Version2.Models;
using GiftManagement_Version2.Repositories;

namespace GiftManagement_Version2.Validators
{
    public class PromotionValidator : AbstractValidator<PromotionRequest>
    {
        private readonly IRepositoryWrapper repository;

        public PromotionValidator(IRepositoryWrapper repository)
        {
            this.repository = repository;

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required");

            //Kiểm tra quà chính và quà tặng kèm đã tồn tại trong bảng Gift chưa
            RuleFor(x => x.MainGiftId)
                .NotEmpty()
                .WithMessage("MainGiftId is required")
                .Must((_, mainGiftId) =>
                {
                    return repository.Gifts.FindByCondition(x => x.Id == mainGiftId).Any();
                }).WithMessage("MainGiftId is not valid");

            RuleFor(x => x.BonusGiftId)
                .NotEmpty()
                .WithMessage("BonusGiftId is required")
                .Must((_, bonusGiftId) =>
                {
                    return repository.Gifts.FindByCondition(x => x.Id == bonusGiftId).Any();
                }).WithMessage("BonusGiftId is not valid");

            //Kiểm tra quà chính và quà tặng kèm đã tồn tại trong bảng Promotion chưa
            RuleFor(x => x)
                .Must(request =>
                {
                    return !repository.Promotions.FindByCondition(p => p.MainGiftId == request.MainGiftId && p.BonusGiftId == request.BonusGiftId).Any();
                }).WithMessage("Promotion is already exist");

            //Kiểm tra quà chính và quà tặng kèm có trùng nhau không
            RuleFor(x => x)
                .Must(request =>
                {
                    return request.MainGiftId != request.BonusGiftId;
                }).WithMessage("MainGiftId and BonusGiftId must be different");
        }
    }
}
