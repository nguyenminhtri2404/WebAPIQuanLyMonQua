using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using GiftManagement_Version2.Data;
using GiftManagement_Version2.Models;
using GiftManagement_Version2.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GiftManagement_Version2.Services
{
    public interface IPromotionServices
    {
        string? CreatePromotion(PromotionRequest request);
        PromotionRespone GetPromotion(int id);
        List<PromotionRespone> GetAllPromotion();
        string? UpdatePromotion(int id, PromotionRequest request);
        string? DeletePromotion(int id);

        //Tìm kiếm quà chính có những quà khuyến mãi nào
        List<PromotionRespone> GetPromotionByMainGift(int mainGiftId);
    }
    public class PromotionServices : IPromotionServices
    {
        private readonly IRepositoryWrapper repository;
        private readonly IMapper mapper;
        private readonly IValidator<PromotionRequest> validator;

        public PromotionServices(IRepositoryWrapper repository, IMapper mapper, IValidator<PromotionRequest> validator)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.validator = validator;
        }

        public string? CreatePromotion(PromotionRequest request)
        {
            ValidationResult validationResult = validator.Validate(request);
            if (!validationResult.IsValid)
            {
                return validationResult.ToString();
            }

            Promotion promotion = mapper.Map<Promotion>(request);
            repository.Promotions.Create(promotion);
            repository.Save();
            return null;
        }

        public string? DeletePromotion(int id)
        {
            Promotion? promotion = repository.Promotions.FindByCondition(p => p.Id == id).FirstOrDefault();
            if (promotion == null)
            {
                return "Promotion is not exist";
            }
            repository.Promotions.Delete(promotion);
            repository.Save();
            return null;
        }

        public List<PromotionRespone> GetAllPromotion()
        {
            List<Promotion> promotions = repository.Promotions
                .Include(p => p.MainGift)
                .Include(p => p.BonusGift)
                .ToList();
            return mapper.Map<List<PromotionRespone>>(promotions);
        }

        public PromotionRespone GetPromotion(int id)
        {
            PromotionRespone? promotion = repository.Promotions
                .Include(p => p.MainGift)
                .Include(p => p.BonusGift)
                .Where(p => p.Id == id)
                .Select(p => new PromotionRespone
                {
                    Name = p.Name,
                    MainGiftName = p.MainGift.Name,
                    BonusGiftName = p.BonusGift.Name
                })
                .FirstOrDefault();

            if (promotion == null)
            {
                return null;
            }

            return promotion;
        }

        public string? UpdatePromotion(int id, PromotionRequest request)
        {
            Promotion? promotion = repository.Promotions.FindByCondition(p => p.Id == id).FirstOrDefault();
            if (promotion == null)
            {
                return "Promotion is not exist";
            }

            ValidationResult validationResult = validator.Validate(request);
            if (!validationResult.IsValid)
            {
                return validationResult.ToString();
            }

            promotion = mapper.Map(request, promotion);
            repository.Promotions.Update(promotion);
            repository.Save();
            return null;
        }

        public List<PromotionRespone> GetPromotionByMainGift(int mainGiftId)
        {
            List<Promotion> promotions = repository.Promotions
                .Include(p => p.MainGift)
                .Include(p => p.BonusGift)
                .Where(p => p.MainGiftId == mainGiftId)
                .ToList();
            return mapper.Map<List<PromotionRespone>>(promotions);
        }
    }
}
