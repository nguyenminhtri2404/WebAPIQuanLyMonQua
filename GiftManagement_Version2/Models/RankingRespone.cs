using System.Text.Json.Serialization;

namespace GiftManagement_Version2.Models
{
    public class RankingRespone
    {
        [JsonIgnore]
        public int UserId { get; set; }
        public string Username { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int TotalPoint { get; set; }
        [JsonIgnore]
        public DateTime LastOrderDate { get; set; }
        public int Rank { get; set; }
        public int IsFinalized { get; set; }
    }
}
