namespace GiftManagement_Version2.Models
{
    public class RankingRequest
    {
        public int UserId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int TotalPoint { get; set; }
        public DateTime LastOrderDate { get; set; }
        public int Rank { get; set; }
        public int IsFinalized { get; set; }
    }
}
