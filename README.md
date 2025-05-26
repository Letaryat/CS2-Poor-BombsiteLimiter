# CS2-Poor-BombsiteLimiter
Bombsite Limiter for CS2. Main logic is based on [CS2_BombsitesRestrict by NockyCZ](https://github.com/NockyCZ/CS2_BombsitesRestrict) with a few new functions.<br/>
[![ko-fi](https://ko-fi.com/img/githubbutton_sm.svg)](https://ko-fi.com/H2H8TK0L9)

## [🛠️] New functions
- Plugin can draw lasers around bombsites to indicate players if the bombsite is open or closed. If both bombsites are disabled, laser will be not generated.
- If PlacingMode is enabled, plugin will generate .json file for each map. Owners can create Entities / Props which will block way to get to the blocked bombsites. These props will spawn on Round Start if bombsite is blocked. 

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
| MinPlayers (int) | Min players for both sites to be enabled  |
| CountBots (bool) | If bots should be counted as MinPlayers  |
| Team (int) | Which team should be counted as MinPlayers (0 - both, 1 - TT, 2 - CT) |
| WhichSiteToBlock (int) | Which bombsite should be blocked (0 - all, 1 - A, 2 - B) |
| PlacingMode (bool) | If placing mode should be enabled (Allows admin to place props which block way for players to get on bombsite) |
| FenceModel (string) | Model for a prop |
| DrawLasers (bool) | If lasers around bombsites should be drawned |
| BlockSiteLaser (string) | What color blocked bombsite laser should be |
| DrawOnUnlockedBombsite (bool) | If laser should be drawn of bombsite which is not blocked |
| UnlockedSiteLaser (string) | What color open bombsite laser should be | 

## [🛡️] Admin commands
| Command  | Description |
| ------------- | ------------- |
| css_placingmode 1 / 2 (1 = A, 2 = B) | Allows admin to place entity / prop using command !bsentity or using Ping (By default mouse3 button). To turn placingmode off, use this command again. |
| css_bsentity | Create prop/entity that will spawn each round when bombsite is limited |
| css_removebsentity **ID**| If you placed your entity wrong, you can removed it using ID of that entity |
| css_tpbsentity **ID** | Teleport to the entity that you placed |
| css_bsentitylist | Prints all entities on map in player console |

