using System.Text.Json.Serialization;

namespace CS2_Poor_BombsiteLimiter.Models
{
    public class PropModel
    {
        public int Id { get; set; }
        public int bs { get; set; }
        public float posX { get; set; }
        public float posY { get; set; }
        public float posZ { get; set; }
        public float angleY { get; set; }
    }
}