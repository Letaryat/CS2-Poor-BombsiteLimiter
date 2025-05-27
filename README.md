# CS2-Poor-BombsiteLimiter
Bombsite Limiter for CS2. Main logic is based on [CS2_BombsitesRestrict by NockyCZ](https://github.com/NockyCZ/CS2_BombsitesRestrict) with a few new functions.<br/>
[![ko-fi](https://ko-fi.com/img/githubbutton_sm.svg)](https://ko-fi.com/H2H8TK0L9)

## [🛠️] New functions
- Plugin can draw lasers around bombsites to indicate players if the bombsite is open or closed. If both bombsites are disabled, laser will be not generated.
- If PlacingMode is enabled, plugin will generate .json file for each map. Owners can create Entities / Props which will block way to get to the blocked bombsites. These props will spawn on Round Start if bombsite is blocked. 
- Possibility for server owners to block one specific bombsite depending on a map.

## [📺] Video presentation
This video shows some of the new functions of the plugin:
https://youtu.be/2T1KsozkhEo?si=CvsS-OqQ1QMIxUlq

## [📌] Setup
- Download latest release,
- Drag files to /plugins/
- Restart your server,
- Config file should be created in configs/plugins/
- Edit to your liking,

## [📝] Configuration
| Option  | Description |
| ------------- | ------------- |
| Flag (string) | Which flag will have access to all of the commands  |
| Type of Notification (int) | 0 - Both Chat and HTMLHud, 1 - Chat, 2 - HTML Hud  |
| Hud Timer (float) | For how many seconds, Hud timer should be shown  |
| Min Players (int) | Min players for both sites to be enabled  |
| Count Bots (bool) | If bots should be counted as MinPlayers  |
| Team (int) | Which team should be counted as MinPlayers (0 - both, 1 - TT, 2 - CT) |
| Which Site To Block (int) | Which bombsite should be blocked (0 - all, 1 - A, 2 - B) |
| Placing Mode (bool) | If placing mode should be enabled (Allows admin to place props which block way for players to get on bombsite) |
| Fence Model (string) | Model for a prop |
| Draw Lasers (bool) | If lasers around bombsites should be drawned |
| 3D Box (bool) | If lasers should create a 3D box around bombsites |
| Block Site Laser (string) | What color blocked bombsite laser should be |
| Draw On Unlocked Bombsite (bool) | If laser should be drawn of bombsite which is not blocked |
| Unlocked Site Laser (string) | What color open bombsite laser should be | 
| Per Map (Dictionary) | If map should have one specific bombsite always disabled. |
| Debug (bool) | If plugin should log information |

**Config** example:
```
{
  "Flag": "@css/root",
  "Type of Notification": 0,
  "Hud timer": 20,
  "MinPlayers": 10,
  "CountBots": true,
  "Team": 0,
  "WhichSiteToBlock": 0,
  "PlacingMode": true,
  "FenceModel": "models/props/de_nuke/hr_nuke/chainlink_fence_001/chainlink_fence_001_128_capped.vmdl",
  "DrawLasers": true,
  "3DBox": true,
  "BlockSiteLaser": "Red",
  "DrawOnUnlockedBombsite": true,
  "UnlockedSiteLaser": "Green",
  "PerMap": {
	"de_dust2": "B",
	"de_vertigo": "A"
  },
  "Debug": true,
  "ConfigVersion": 1
}
```

## [🛡️] Admin commands
| Command  | Description |
| ------------- | ------------- |
| css_placingmode 1 / 2 (1 = A, 2 = B) | Allows admin to place entity / prop using command !bsentity or using Ping (By default mouse3 button). To turn placingmode off, use this command again. |
| css_bsentity | Create prop/entity that will spawn each round when bombsite is limited |
| css_removebsentity **ID**| If you placed your entity wrong, you can removed it using ID of that entity |
| css_tpbsentity **ID** | Teleport to the entity that you placed |
| css_bsentitylist | Prints all entities on map in player console |

