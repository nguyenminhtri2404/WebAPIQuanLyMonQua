using AutoMapper;
using GiftManagement_Version2.Data;
using GiftManagement_Version2.Models;
using GiftManagement_Version2.Repositories;

namespace GiftManagement_Version2.Services
{
    public interface IOrderServices
    {
        //Checkout
        string? Checkout(int userId);
        //Xem lịch sử đơn hàng
        List<OrderRespone> GetOrderHistory(int userId);
        //GiftToUser
        string? GiftToUser(int userId, int giftId, int quantity);

        string? GiftToTopUsers(int gifId);

    }
    public class OrderServices : IOrderServices
    {
        private readonly IRepositoryWrapper repository;
        private readonly IMapper mapper;

        public OrderServices(IRepositoryWrapper repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public string? Checkout(int userId)
        {
            // Kiểm tra xem giỏ hàng của người dùng có rỗng không
            List<Cart> cartItems = repository.Carts.FindByCondition(c => c.UserId == userId && c.Status == 0).ToList();
            if (cartItems == null || !cartItems.Any())
            {
                return "Cart is empty.";
            }

            // Tính tổng số điểm của giỏ
            int totalPrice = 0;
            int totalAccumulatedPoints = 0; // lưu trữ tổng điểm tích lũy
            foreach (Cart cartItem in cartItems)
            {
                Gift? gift = repository.Gifts.FindByCondition(g => g.Id == cartItem.GiftId).FirstOrDefault();
                if (gift == null)
                {
                    return "Gift not found.";
                }
                totalPrice += gift.Point * cartItem.Quantity;
                totalAccumulatedPoints += (int)(gift.Point * 0.1); // Tính điểm tích lũy
            }

            // Kiểm tra user tồn tại không
            User? user = repository.Users.FindByCondition(u => u.Id == userId).FirstOrDefault();
            if (user == null)
            {
                return "User not found.";
            }

            // Kiểm tra số điểm của user có đủ không
            if (user.Point < totalPrice)
            {
                return "Not enough points.";
            }

            // Kiểm tra số lượng của các món quà trong giỏ hàng
            foreach (Cart cartItem in cartItems)
            {
                Gift? gift = repository.Gifts.FindByCondition(g => g.Id == cartItem.GiftId).FirstOrDefault();
                if (gift == null || gift.Quantity < cartItem.Quantity)
                {
                    return $"Insufficient quantity for gift: {gift?.Name ?? "Unknown"}";
                }

                // Kiểm tra xem món quà có BonusGift đi kèm không
                Promotion? promotion = repository.Promotions.FindByCondition(p => p.MainGiftId == cartItem.GiftId).FirstOrDefault();
                if (promotion != null)
                {
                    Gift? bonusGift = repository.Gifts.FindByCondition(g => g.Id == promotion.BonusGiftId).FirstOrDefault();
                    if (bonusGift == null || bonusGift.Quantity < 1)
                    {
                        // Nếu không có quà tặng, vẫn tiếp tục mà không có quà đi kèm
                        promotion = null;
                    }
                }
            }

            // Cập nhật điểm của user
            user.Point -= totalPrice;
            user.AccumulatedPoint += totalAccumulatedPoints; // Cộng điểm tích lũy
            repository.Users.Update(user);

            foreach (Cart cartItem in cartItems)
            {
                // Tạo đơn hàng cho quà chính
                Order order = new()
                {
                    UserId = userId,
                    GiftId = cartItem.GiftId,
                    Quantity = cartItem.Quantity,
                    OrderAt = DateTime.Now,
                    OrderType = 1, // Mua
                    TotalPrice = totalPrice,
                };

                repository.Orders.Create(order);
                repository.Save();

                // Update sô lượng món quà trong kho
                Gift? gift = repository.Gifts.FindByCondition(g => g.Id == cartItem.GiftId).FirstOrDefault();
                if (gift != null)
                {
                    gift.Quantity -= cartItem.Quantity;
                    repository.Gifts.Update(gift);
                }

                // Kiểm tra xem món quà có BonusGift đi kèm không
                Promotion? promotion = repository.Promotions.FindByCondition(p => p.MainGiftId == cartItem.GiftId).FirstOrDefault();
                if (promotion != null)
                {
                    Gift? bonusGift = repository.Gifts.FindByCondition(g => g.Id == promotion.BonusGiftId).FirstOrDefault();
                    //Kiểm tra số lượng BonusGift còn không
                    if (bonusGift != null && bonusGift.Quantity >= 1)
                    {
                        // Tạo đơn hàng cho BonusGift
                        Order bonusOrder = new()
                        {
                            UserId = userId,
                            GiftId = promotion.BonusGiftId,
                            Quantity = 1,
                            OrderAt = DateTime.Now,
                            OrderType = 0, // Received
                            MainGiftId = order.TransactionId, // Link to the main gift order
                            TotalPrice = 0,
                        };

                        repository.Orders.Create(bonusOrder);

                        // Cập nhật số lượng BonusGift trong kho
                        bonusGift.Quantity -= 1;
                        repository.Gifts.Update(bonusGift);
                    }
                }
            }

            // Save all changes
            repository.Save();

            // Clear the user's cart
            foreach (Cart cartItem in cartItems)
            {
                repository.Carts.Delete(cartItem);
            }

            repository.Save();

            return null;
        }

        public string? GiftToUser(int userId, int giftId, int quantity)
        {
            // Retrieve the gift item from the inventory
            Gift? gift = repository.Gifts.FindByCondition(g => g.Id == giftId).FirstOrDefault();
            if (gift == null)
            {
                return "Gift not found.";
            }

            // Kiểm tra số lượng cuả quà chính có đủ không
            if (gift.Quantity < quantity)
            {
                return "Insufficient gift quantity.";
            }

            // Kiểm tra quà chính có BonusGift đi kèm không
            Promotion? promotion = repository.Promotions.FindByCondition(p => p.MainGiftId == giftId).FirstOrDefault();
            Gift? bonusGift = null;
            if (promotion != null)
            {
                bonusGift = repository.Gifts.FindByCondition(g => g.Id == promotion.BonusGiftId).FirstOrDefault();
                //Kiểm tra số lượng BonusGift còn không
                if (bonusGift != null && bonusGift.Quantity < 1)
                {
                    bonusGift = null;
                }
            }

            Order order = new()
            {
                UserId = userId,
                GiftId = giftId,
                Quantity = quantity,
                OrderAt = DateTime.Now,
                OrderType = 0 // 'Received'
            };

            repository.Orders.Create(order);
            repository.Save();

            gift.Quantity -= quantity;
            repository.Gifts.Update(gift);

            // Kiẻm tra có BonusGift đi kèm  và còn không
            if (promotion != null && bonusGift != null)
            {
                // Create an order for the BonusGift
                Order bonusOrder = new()
                {
                    UserId = userId,
                    GiftId = promotion.BonusGiftId,
                    Quantity = 1,
                    OrderAt = DateTime.Now,
                    OrderType = 0, // 0 means 'Received'
                    MainGiftId = order.TransactionId
                };

                repository.Orders.Create(bonusOrder);

                // Update số lượng BonusGift trong kho
                bonusGift.Quantity -= 1;
                repository.Gifts.Update(bonusGift);
            }

            repository.Save();

            return null;
        }

        //Tặng quà cho top 3 user có điểm tích thứ hạng cao nhất
        public string? GiftToTopUsers(int giftId)
        {
            // Kiểm tra đó có phải là quà để tặng không GiftType = 1
            Gift? gift = repository.Gifts.FindByCondition(g => g.Id == giftId && g.GiftType == 1).FirstOrDefault();
            if (gift == null)
            {
                return "Gift not found or not a gift for gifting.";
            }
            //Kiểm tra số lượng quà còn không
            if (gift.Quantity < 3)
            {
                return "Insufficient gift quantity.";
            }

            // Lấy top 3 user có rank cao nhất
            List<Ranking> topUsers = repository.Rankings.FindAll()
                                                        .OrderBy(r => r.Rank)
                                                        .Take(3)
                                                        .ToList();


            foreach (Ranking user in topUsers)
            {
                Order order = new()
                {
                    UserId = user.UserId,
                    GiftId = giftId,
                    Quantity = 1,
                    OrderAt = DateTime.Now,
                    OrderType = 0 // 'Received'
                };

                repository.Orders.Create(order);
                repository.Save();

                gift.Quantity -= 1;
                repository.Gifts.Update(gift);
            }

            repository.Save();

            return null;

        }

        public List<OrderRespone> GetOrderHistory(int userId)
        {
            List<Order> orders = repository.Orders.FindByCondition(o => o.UserId == userId).ToList();
            return mapper.Map<List<OrderRespone>>(orders);
        }

    }
}
