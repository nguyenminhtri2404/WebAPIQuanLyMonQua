using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using GiftManagement_Version2.Data;
using GiftManagement_Version2.Helpers;
using GiftManagement_Version2.Models;
using GiftManagement_Version2.Repositories;

namespace GiftManagement_Version2.Services
{
    public interface IGiftServices
    {
        Task<string?> CreateGift(GiftRequest request);
        Task<string?> UpdateGift(int id, GiftRequest request);
        string? DeleteGift(int id);
        GiftRespone GetGift(int id);
        List<GiftRespone> GetAllGift();
    }
    public class GiftServices : IGiftServices
    {
        private readonly IMapper mapper;
        private readonly IRepositoryWrapper repository;
        private readonly IValidator<GiftRequest> validator;

        public GiftServices(IMapper mapper, IRepositoryWrapper repository, IValidator<GiftRequest> validator)
        {
            this.mapper = mapper;
            this.repository = repository;
            this.validator = validator;
        }

        public async Task<string?> CreateGift(GiftRequest request)
        {
            Gift gift = mapper.Map<Gift>(request);
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
                gift.Image = imagePath;
            }

            repository.Gifts.Create(gift);
            repository.Save();
            return null;
        }

        public string? DeleteGift(int id)
        {
            Gift gift = repository.Gifts.FindByCondition(g => g.Id == id).FirstOrDefault();

            if (gift == null)
            {
                return "Gift not found";
            }

            repository.Gifts.Delete(gift);
            repository.Save();
            return null;
        }

        public List<GiftRespone> GetAllGift()
        {
            List<Gift> gifts = repository.Gifts.FindAll().ToList();
            return mapper.Map<List<GiftRespone>>(gifts);
        }

        public GiftRespone GetGift(int id)
        {
            return mapper.Map<GiftRespone>(repository.Gifts.FindByCondition(g => g.Id == id).FirstOrDefault());
        }

        public async Task<string?> UpdateGift(int id, GiftRequest request)
        {
            Gift? gift = repository.Gifts.FindByCondition(g => g.Id == id).FirstOrDefault();
            ValidationResult validationResult = validator.Validate(request);
            if (!validationResult.IsValid)
            {
                return validationResult.ToString();
            }

            if (gift != null)
            {
                gift = mapper.Map(request, gift);

                string uploadDirectory = Path.Combine("wwwroot/images");

                string imagePath = await ImageHelper.SaveImageAsync(request.Image, uploadDirectory);

                if (imagePath != null)
                {
                    // Set the image path in the user entity
                    gift.Image = imagePath;
                }

                repository.Gifts.Update(gift);
                repository.Save();
            }
            return null;
        }
    }
}
