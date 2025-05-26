using System.Text.Json.Serialization;
using CounterStrikeSharp.API.Core;

namespace CS2_Poor_BombsiteLimiter.Config
{
    public class PluginConfig : BasePluginConfig
    {
        [JsonPropertyName("Flag")]
        public string Flag { get; set; } = "@css/root";

        [JsonPropertyName("Type of Notification")]
        public int TypeOfNotification { get; set; } = 2;

        [JsonPropertyName("Hud timer")]
        public float HudTimer { get; set; } = 20;

        [JsonPropertyName("MinPlayers")]
        public int MinPlayers { get; set; } = 10;

        [JsonPropertyName("CountBots")]
        public bool CountBots { get; set; } = true;

        [JsonPropertyName("Team")]
        public int Team { get; set; } = 0;

        [JsonPropertyName("WhichSiteToBlock")]
        public int WhichSiteToBlock { get; set; } = 0;
        
        [JsonPropertyName("PlacingMode")]
        public bool PlacingMode { get; set; } = true;

        [JsonPropertyName("FenceModel")]
        public string FenceModel { get; set; } = "models/props/de_nuke/hr_nuke/chainlink_fence_001/chainlink_fence_001_128_capped.vmdl";

        [JsonPropertyName("DrawLasers")]
        public bool DrawLasers { get; set; } = true;

        [JsonPropertyName("BlockSiteLaser")]
        public string BlockSiteLaser { get; set; } = "Red";

        [JsonPropertyName("DrawOnUnlockedBombsite")]
        public bool DrawOnUnlockedBombsite { get; set; } = true;

        [JsonPropertyName("UnlockedSiteLaser")]
        public string UnlockedSiteLaser { get; set; } = "Green";
    }
}