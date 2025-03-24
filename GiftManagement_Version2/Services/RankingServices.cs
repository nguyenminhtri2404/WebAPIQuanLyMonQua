using AutoMapper;
using GiftManagement_Version2.Data;
using GiftManagement_Version2.Models;
using GiftManagement_Version2.Repositories;

namespace GiftManagement_Version2.Services
{
    public interface IRankingServices
    {
        string? UpdateRanking();
        List<RankingRespone> GetRankings();
        //Lấy danh sách user có điểm tích lũy và xếp hạng của họ theo tháng
        List<RankingRespone> GetRankingsByMonth(int month, int year);
    }
    public class RankingServices : IRankingServices
    {
        private readonly IRepositoryWrapper repository;
        private readonly IMapper mapper;

        public RankingServices(IRepositoryWrapper repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public List<RankingRespone> GetRankings()
        {
            //Lấy danh sách xếp hạng và thông tin người dùng sắp xếp theo thứ hạng tăng dần
            List<RankingRespone> rankings = repository.Rankings.FindAll()
                .OrderBy(r => r.Rank)
                .Join(repository.Users.FindAll(),
                      ranking => ranking.UserId,
                      user => user.Id,
                      (ranking, user) => new RankingRespone
                      {
                          UserId = ranking.UserId,
                          Username = user.Username,
                          Month = ranking.Month,
                          Year = ranking.Year,
                          TotalPoint = ranking.TotalPoint,
                          LastOrderDate = ranking.LastOrderDate,
                          Rank = ranking.Rank,
                          IsFinalized = ranking.IsFinalized
                      })
                .ToList();
            return rankings;

        }

        public string? UpdateRanking()
        {
            int currentMonth = DateTime.Now.Month;
            int currentYear = DateTime.Now.Year;

            bool isFinalize = DateTime.Now.Day >= 29;

            if (isFinalize)
            {
                AutoFinalizeRankings();
                return "Rankings have been finalized";
            }

            // Lấy tất cả người dùng và đơn hàng của họ
            List<User> users = repository.Users.FindAll().ToList();
            List<Order> orders = repository.Orders.FindAll().ToList();

            // Sắp xếp users theo điểm tích lũy giảm dần và theo thời gian mua hàng cuối cùng (nếu điểm bằng nhau)
            List<RankingRespone> userRankings = users
                .Select(u => new RankingRespone
                {
                    UserId = u.Id,
                    TotalPoint = u.AccumulatedPoint,
                    LastOrderDate = orders
                        .Where(o => o.UserId == u.Id)
                        .OrderByDescending(o => o.OrderAt)
                        .Select(o => o.OrderAt)
                        .FirstOrDefault()
                })
                .OrderByDescending(u => u.TotalPoint)
                .ThenBy(u => u.LastOrderDate)
                .ToList();

            // Lưu xếp hạng vào bảng Ranking
            for (int i = 0; i < userRankings.Count; i++)
            {
                Ranking? existingRanking = repository.Rankings
                    .FindByCondition(r => r.UserId == userRankings[i].UserId && r.Month == currentMonth && r.Year == currentYear)
                    .FirstOrDefault();

                if (existingRanking != null)
                {
                    // Cập nhật xếp hạng nếu đã tồn tại
                    existingRanking.TotalPoint = userRankings[i].TotalPoint;
                    existingRanking.LastOrderDate = userRankings[i].LastOrderDate;
                    existingRanking.Rank = i + 1;  // Thứ hạng bắt đầu từ 1
                    existingRanking.IsFinalized = isFinalize ? 1 : 0;

                    repository.Rankings.Update(existingRanking);
                }
                else
                {
                    // Thêm mới xếp hạng nếu chưa tồn tại
                    Ranking userRanking = new()
                    {
                        UserId = userRankings[i].UserId,
                        Month = currentMonth,
                        Year = currentYear,
                        TotalPoint = userRankings[i].TotalPoint,
                        LastOrderDate = userRankings[i].LastOrderDate,
                        Rank = i + 1,  // Thứ hạng bắt đầu từ 1
                        IsFinalized = isFinalize ? 1 : 0
                    };

                    repository.Rankings.Create(userRanking);
                }
            }

            // Lưu các thay đổi vào database
            repository.Save();

            return null;
        }

        public List<RankingRespone> GetRankingsByMonth(int month, int year)
        {
            // Lấy danh sách xếp hạng và thông tin người dùng
            List<RankingRespone> rankings = repository.Rankings
                .FindByCondition(r => r.Month == month && r.Year == year)
                .OrderBy(r => r.Rank)
                .Join(repository.Users.FindAll(),
                      ranking => ranking.UserId,
                      user => user.Id,
                      (ranking, user) => new RankingRespone
                      {
                          UserId = ranking.UserId,
                          Username = user.Username,
                          Month = ranking.Month,
                          Year = ranking.Year,
                          TotalPoint = ranking.TotalPoint,
                          LastOrderDate = ranking.LastOrderDate,
                          Rank = ranking.Rank,
                          IsFinalized = ranking.IsFinalized
                      })
                .ToList();

            return rankings;
        }

        public void AutoFinalizeRankings()
        {
            int currentMonth = DateTime.Now.Month;
            int currentYear = DateTime.Now.Year;

            // Kiểm tra nếu ngày hiện tại là ngày 29 hoặc sau đó
            if (DateTime.Now.Day >= 29)
            {
                // Lấy tất cả các bản ghi xếp hạng của tháng hiện tại
                List<Ranking> rankingsToFinalize = repository.Rankings
                                                            .FindByCondition(r => r.Month == currentMonth && r.Year == currentYear && r.IsFinalized == 0)
                                                            .ToList();

                // Cập nhật trạng thái IsFinalized cho các bản ghi xếp hạng
                foreach (Ranking? ranking in rankingsToFinalize)
                {
                    ranking.IsFinalized = 1;
                    repository.Rankings.Update(ranking);
                }

                repository.Save();
            }
        }
    }
}
