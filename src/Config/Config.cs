using System.Text.Json.Serialization;
using CounterStrikeSharp.API.Core;

namespace CS2_Poor_BombsiteLimiter.Config
{
    public class PluginConfig : BasePluginConfig
    {
        [JsonPropertyName("Flag")]
        public string Flag { get; set; } = "@css/root";

        [JsonPropertyName("Type of Notification")]
        public int TypeOfNotification { get; set; } = 0;

        [JsonPropertyName("Hud timer")]
        public float HudTimer { get; set; } = 20;

        [JsonPropertyName("Min Players")]
        public int MinPlayers { get; set; } = 10;

        [JsonPropertyName("Count Bots")]
        public bool CountBots { get; set; } = true;

        [JsonPropertyName("Team")]
        public int Team { get; set; } = 0;

        [JsonPropertyName("Which Site To Block")]
        public int WhichSiteToBlock { get; set; } = 0;

        [JsonPropertyName("Placing Mode")]
        public bool PlacingMode { get; set; } = true;

        [JsonPropertyName("Fence Model")]
        public string FenceModel { get; set; } = "models/props/de_nuke/hr_nuke/chainlink_fence_001/chainlink_fence_001_128_capped.vmdl";

        [JsonPropertyName("Draw Lasers")]
        public bool DrawLasers { get; set; } = true;

        [JsonPropertyName("3D Box")]
        public bool ThreeDeeBox { get; set; } = true;

        [JsonPropertyName("Laser width")]
        public float LaserWidth { get; set; } = 1;

        [JsonPropertyName("Laser Distance from Ground")]
        public float LaserDistance { get; set; } = 5;

        [JsonPropertyName("Block Site Laser")]
        public string BlockSiteLaser { get; set; } = "Red";

        [JsonPropertyName("Draw On Unlocked Bombsite")]
        public bool DrawOnUnlockedBombsite { get; set; } = true;

        [JsonPropertyName("Unlocked Site Laser")]
        public string UnlockedSiteLaser { get; set; } = "Green";

        [JsonPropertyName("Per Map")]
        public Dictionary<string, string> PerMap { get; set; } = new Dictionary<string, string>();

        [JsonPropertyName("Debug")]
        public bool Debug { get; set; } = true;
    }
}