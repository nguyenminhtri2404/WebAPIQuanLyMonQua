using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using GiftManagement_Version2.Data;
using GiftManagement_Version2.Models;
using GiftManagement_Version2.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GiftManagement_Version2.Services
{
    public interface ICartServices
    {
        //Thêm sản phẩm vào giỏ hàng
        string? AddToCart(CartRequest request);
        //Xóa sản phẩm khỏi giỏ hàng
        string? RemoveItemFromCart(int userId, int giftId);
        //Cập nhật số lượng sản phẩm trong giỏ hàng
        string? UpdateQuantity(int userId, int giftId, int quantity);
        //Tăng 1 sản phẩm trong giỏ hàng
        string? IncreaseQuantity(int userId, int giftId);
        //Giảm 1 sản phẩm trong giỏ hàng
        string? DecreaseQuantity(int userId, int giftId);
        //Lấy danh sách sản phẩm trong giỏ hàng của user
        List<CartRespone> GetCart(int userId);


    }
    public class CartServices : ICartServices
    {
        private readonly IRepositoryWrapper repository;
        private readonly IMapper mapper;
        private readonly IValidator<CartRequest> validator;

        public CartServices(IRepositoryWrapper repository, IMapper mapper, IValidator<CartRequest> validator)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.validator = validator;
        }

        public string? AddToCart(CartRequest request)
        {
            Cart cart = mapper.Map<CartRequest, Cart>(request);
            ValidationResult result = validator.Validate(request);
            if (!result.IsValid)
            {
                return result.ToString();
            }

            //Kiểm tra xem trong giỏ hàng đã có sản phẩm đó chưa, nếu có thì cộng thêm số lượng
            Cart? cartInDb = repository.Carts.FindByCondition(c => c.UserId == request.UserId && c.GiftId == request.GiftId).FirstOrDefault();
            if (cartInDb != null)
            {
                //Kiểm tra số lượng của request có lớn hơn số lượng trong kho hay không
                ValidationResult validationInDb = validator.Validate(new CartRequest { UserId = request.UserId, GiftId = request.GiftId, Quantity = cartInDb.Quantity + request.Quantity });
                if (!validationInDb.IsValid)
                {
                    return validationInDb.ToString();
                }

                cartInDb.Quantity += request.Quantity;
                repository.Carts.Update(cartInDb);

            }
            else
            {
                repository.Carts.Create(cart);
            }

            repository.Save();
            return null;

        }

        public string? RemoveItemFromCart(int userId, int giftId)
        {
            Cart? cart = repository.Carts.FindByCondition(c => c.UserId == userId && c.GiftId == giftId).FirstOrDefault();
            if (cart == null)
            {
                return "This gift is not in cart";
            }

            repository.Carts.Delete(cart);
            repository.Save();
            return null;
        }

        public string? UpdateQuantity(int userId, int giftId, int quantity)
        {
            Cart? cart = repository.Carts.FindByCondition(c => c.UserId == userId && c.GiftId == giftId).FirstOrDefault();
            if (cart == null)
            {
                return "This gift is not in cart";
            }

            ValidationResult result = validator.Validate(new CartRequest { UserId = userId, GiftId = giftId, Quantity = quantity });
            if (!result.IsValid)
            {
                return result.ToString();
            }

            cart.Quantity = quantity;
            repository.Carts.Update(cart);
            repository.Save();
            return null;
        }

        public string? IncreaseQuantity(int userId, int giftId)
        {
            Cart? cart = repository.Carts.FindByCondition(c => c.UserId == userId && c.GiftId == giftId).FirstOrDefault();
            if (cart == null)
            {
                return "This gift is not in cart";
            }

            ValidationResult result = validator.Validate(new CartRequest { UserId = userId, GiftId = giftId, Quantity = cart.Quantity + 1 });
            if (!result.IsValid)
            {
                return result.ToString();
            }

            cart.Quantity++;
            repository.Carts.Update(cart);
            repository.Save();
            return null;
        }

        public string? DecreaseQuantity(int userId, int giftId)
        {
            Cart? cart = repository.Carts.FindByCondition(c => c.UserId == userId && c.GiftId == giftId).FirstOrDefault();
            if (cart == null)
            {
                return "This gift is not in cart";
            }

            cart.Quantity--;
            if (cart.Quantity <= 0)
            {
                return RemoveItemFromCart(userId, giftId);
            }

            repository.Carts.Update(cart);
            repository.Save();
            return null;
        }

        public List<CartRespone> GetCart(int userId)
        {
            Cart? cartItem = repository.Carts.FindByCondition(c => c.UserId == userId).FirstOrDefault();
            if (cartItem == null)
            {
                return null;
            }

            List<CartRespone> cart = repository.Carts.FindByCondition(c => c.UserId == userId)
                .Include(c => c.Gift)
                .Select(c => new CartRespone
                {
                    UserId = c.UserId,
                    GiftName = c.Gift.Name,
                    Quantity = c.Quantity,
                    Status = c.Status
                }).ToList();

            return cart;
        }
    }
}
