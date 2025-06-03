# CS2-Poor-BombsiteLimiter
Bombsite Limiter for CS2. Main logic is based on [CS2_BombsitesRestrict by NockyCZ](https://github.com/NockyCZ/CS2_BombsitesRestrict) with a few new functions.<br/>
[![ko-fi](https://ko-fi.com/img/githubbutton_sm.svg)](https://ko-fi.com/H2H8TK0L9)

## [🛠️] New functions
- The plugin can draw laser beams around bombsites to indicate to players whether a bombsite is open or closed. If both bombsites are enabled, no lasers will be generated.
- Option to add a custom particle that appears at the center of a blocked bombsite and always faces the players.
- Option to add a custom point_worldtext at the center of a blocked bombsite that also faces the players.
- When PlacingMode is enabled, the plugin will generate a .json file for each map. Server owners can create entities or props to physically block access to closed bombsites. These props will automatically spawn at the start of each round if the bombsite is blocked.
- Server owners can choose to block a specific bombsite on a per-map basis.

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
| Which Site To Block (int) | Which bombsite should be blocked (0 - Random, 1 - A, 2 - B) |
| Placing Mode (bool) | If placing mode should be enabled (Allows admin to place props which block way for players to get on bombsite) |
| Fence Model (string) | Model for a prop |
| Draw Lasers (bool) | If lasers around bombsites should be drawn |
| 3D Box (bool) | If lasers should create a 3D box around bombsites |
| Laser width (float) | Width of laser |
| Laser Distance from Ground (float) | Laser distance from ground |
| Block Site Laser (string) | What color blocked bombsite laser should be |
| Draw On Unlocked Bombsite (bool) | If laser should be drawn of bombsite which is not blocked |
| Unlocked Site Laser (string) | What color open bombsite laser should be | 
| Bombsite Sprite (string) | Can be null. It spawns a sprite in the center of a blocked bombsite |
| Bombsite Sprite Height (float) | Height from the ground of a sprite |
| Text Display (bool) | If point_worldtext should be created that follows player camera |
| Text Message (string) | Message of a point_worldtext that is created in the center of a blocked bombsite |
| Font Size (float) | Size of a point_worldtext |
| Font Name (string) | Font name of a point_worldtext |
| Text Color (string) | Color of a point_worldtext |
| Text Height (float) | Height from the ground of a point_worldtext |
| Per Map (Dictionary<string, string>) | If map should have one specific bombsite always disabled. |
| Debug (bool) | If plugin should log information |

- In lang/ "NoBlockedHTML" can be empty if you don't want HTMLHud to show when both bombsites are enabled.
```
	"NoBlockedHTML": "",
```

### [📝] Config example:
```
{
  "Flag": "@css/root",
  "Type of Notification": 0,
  "Hud timer": 15,
  "Min Players": 5,
  "Count Bots": true,
  "Team": 0,
  "Which Site To Block": 0,
  "Placing Mode": true,
  "Fence Model": "models/props/de_nuke/hr_nuke/chainlink_fence_001/chainlink_fence_001_128_capped.vmdl",
  "Draw Lasers": true,
  "3D Box": true,
  "Laser width": 1,
  "Laser Distance from Ground": 5,
  "Block Site Laser": "Red",
  "Draw On Unlocked Bombsite": true,
  "Unlocked Site Laser": "Green",
  "BombsiteSprite": "particles/bombsites/cat.vpcf",
  "Bombsite Sprite Height": 200,
  "Text Display": true,
  "Text Message": "Blocked!",
  "Font Size": 35,
  "Font Name": "Arial",
  "Text Color": "Red",
  "Text Height": 100,
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

### [🚨] Plugin might be poorly written and have some issues. I have no idea what I am doing, but when tested it worked fine.