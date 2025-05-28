using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Events;
using CounterStrikeSharp.API.Modules.Utils;
using CS2_Poor_BombsiteLimiter.Utils;
using Microsoft.Extensions.Logging;

namespace CS2_Poor_BombsiteLimiter.Managers;

public class BombsiteManager(CS2_Poor_BombsiteLimiter plugin)
{
    private readonly CS2_Poor_BombsiteLimiter _plugin = plugin;
    public string? blockedSite;


    public void ResetBombsites()
    {
        var Bombsites = Utilities.FindAllEntitiesByDesignerName<CBaseEntity>("func_bomb_target");
        foreach (var bs in Bombsites)
        {
            bs.AcceptInput("Enable");
        }
    }

    public int BlockSiteByIndex()
    {
        int site;
        if (_plugin.Config.WhichSiteToBlock == 0)
        {
            Random random = new Random();
            site = random.Next(1, 3);
        }
        else
        {
            site = _plugin.Config.WhichSiteToBlock;
        }

        return site;
    }

    public void DisableBombsite()
    {
        if (Utils.BombsiteLimiter_Utilities.GameRules().WarmupPeriod) return;

        var Bombsites = Utilities.FindAllEntitiesByDesignerName<CBaseEntity>("func_bomb_target");
        //var blocked = BlockSiteByIndex();
        var AllPlayers = _plugin.BombsiteUtils!.GetAllPlayers();

        var PerMap = _plugin.Config.PerMap;
        var mapName = _plugin.PropManager!._mapName;

        int blocked;
        if (PerMap != null && PerMap.TryGetValue(mapName!, out var value))
        {
            blocked = bsToInt(value.ToUpper());
            _plugin.DebugLog($"Blocking site from CFG: {value} for map: {mapName}");
        }
        else
        {
            blocked = BlockSiteByIndex();
            _plugin.DebugLog($"Blocking random bombsite: {bsToString(blocked)}");
        }

        blockedSite = bsToString(blocked);



        if (AllPlayers >= _plugin.Config.MinPlayers)
        {
            var message = Utils.BombsiteLimiter_Utilities.ReplaceMessageNewlines(_plugin.Localizer["NoBlocked", blockedSite, _plugin.Config.MinPlayers]);
            Server.PrintToChatAll($"{Utils.BombsiteLimiter_Utilities.ReplaceMessageNewlines(_plugin.Localizer["Prefix"])}{message}");
            return;
        }

        if (_plugin.Config.TypeOfNotification != 2)
        {
            var message = Utils.BombsiteLimiter_Utilities.ReplaceMessageNewlines(_plugin.Localizer["Blocked", blockedSite, _plugin.Config.MinPlayers]);
            Server.PrintToChatAll($"{Utils.BombsiteLimiter_Utilities.ReplaceMessageNewlines(_plugin.Localizer["Prefix"])}{message}");
        }

        foreach (var bs in Bombsites)
        {
            var site = new CBombTarget(NativeAPI.GetEntityFromIndex((int)bs.Index));
            int entitySite = site.IsBombSiteB ? 2 : 1;
            if (_plugin.Config.MinPlayers > AllPlayers)
            {
                if (entitySite == blocked)
                {
                    _plugin.PropManager!.SpawnProps(blocked);
                    if (bs.IsValid)
                    {
                        bs.AcceptInput("Disable");
                        if (_plugin.Config.DrawLasers)
                        {
                            var mins = bs.AbsOrigin! + bs.Collision!.Mins;
                            var maxs = bs.AbsOrigin! + bs.Collision!.Maxs;

                            _plugin.BombsiteUtils!.DrawWireframe3D(mins, maxs, _plugin.Config.BlockSiteLaser);
                        }
                    }
                }
                else
                {
                    if (_plugin.Config.DrawOnUnlockedBombsite)
                    {
                        var mins = bs.AbsOrigin! + bs.Collision!.Mins;
                        var maxs = bs.AbsOrigin! + bs.Collision!.Maxs;
                        _plugin.BombsiteUtils!.DrawWireframe3D(mins, maxs, _plugin.Config.UnlockedSiteLaser);
                    }
                }
            }
        }
    }

    public string bsToString(int bs)
    {
        return bs == 2 ? "B" : "A";
    }

    public int bsToInt(string bs)
    {
        return bs == "B" ? 2 : 1;
    }

}