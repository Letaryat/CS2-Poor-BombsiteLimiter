using System.Text.Json.Serialization;

namespace CS2_Poor_BombsiteLimiter.Models
{
    public class PlacingModel
    {
        public int Slot { get; set; }
        public bool PlacingAllowed { get; set; }
        public string? site { get; set; }
    }
}