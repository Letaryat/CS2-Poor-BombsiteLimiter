using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Events;
using CounterStrikeSharp.API.Modules.Utils;
using CS2_Poor_BombsiteLimiter.Models;
using CS2_Poor_BombsiteLimiter.Utils;
using Microsoft.Extensions.Logging;

namespace CS2_Poor_BombsiteLimiter.Managers;

public class CommandsManager(CS2_Poor_BombsiteLimiter plugin)
{
    private readonly CS2_Poor_BombsiteLimiter _plugin = plugin;

    public void RegisterCommands()
    {
        _plugin.AddCommand("css_BsEntity", "Create Entity that will spawn each round when bombsite is limited", OnCreateEntity);
        _plugin.AddCommand("css_RemoveBsEntity", "Remove BS Entity using ID", OnRemoveEntity);
        _plugin.AddCommand("css_tpBsEntity", "Teleport to BS Entity using ID", TeleportToEntity);
        _plugin.AddCommand("css_bsentitylist", "Placing entity mode", ShowEntityList);
        _plugin.AddCommand("css_placingmode", "Placing entity mode", OnPlacingMode);
    }

    private void OnCreateEntity(CCSPlayerController? player, CommandInfo commandInfo)
    {
        if (player == null || player.PlayerPawn == null) return;
        if (!_plugin.Config.PlacingMode) return;
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat($"{Utils.BombsiteLimiter_Utilities.ReplaceMessageNewlines(_plugin.Localizer["Prefix"])}{Utils.BombsiteLimiter_Utilities.ReplaceMessageNewlines(_plugin.Localizer["NoPermission"])}");
            return;
        }

        var slot = player.Slot;
        var placingEntry = _plugin.PlacingPlayers.FirstOrDefault(p => p.Slot == slot);
        if (placingEntry != null)
        {
            if (placingEntry.PlacingAllowed)
            {
                var pawn = player.PlayerPawn.Value;
                var pos = pawn!.AbsOrigin;
                var qangle = new QAngle(0, pawn.EyeAngles.Y, 0);
                _plugin.PropManager!.CreateProp(pos!, qangle);
                _plugin.PropManager!.PushCordsToFile(pos!, qangle, placingEntry.site!);
                player.PrintToChat($"{Utils.BombsiteLimiter_Utilities.ReplaceMessageNewlines(_plugin.Localizer["Prefix"])}{Utils.BombsiteLimiter_Utilities.ReplaceMessageNewlines(_plugin.Localizer["NewEntity", _plugin.BombsiteManager!.bsToString(Convert.ToInt32(placingEntry.site))])}");
            }
            else
            {
                player.PrintToChat($"{Utils.BombsiteLimiter_Utilities.ReplaceMessageNewlines(_plugin.Localizer["Prefix"])}{Utils.BombsiteLimiter_Utilities.ReplaceMessageNewlines(_plugin.Localizer["TurnOnPlacing"])}");
                return;
            }
        }

    }

    private void OnRemoveEntity(CCSPlayerController? player, CommandInfo commandInfo)
    {
        if (player == null || player.PlayerPawn == null) return;
        if (!_plugin.Config.PlacingMode) return;
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat($"{Utils.BombsiteLimiter_Utilities.ReplaceMessageNewlines(_plugin.Localizer["Prefix"])}{Utils.BombsiteLimiter_Utilities.ReplaceMessageNewlines(_plugin.Localizer["NoPermission"])}");
            return;
        }

        if (commandInfo.ArgCount < 2)
        {
            player.PrintToChat($"{Utils.BombsiteLimiter_Utilities.ReplaceMessageNewlines(_plugin.Localizer["Prefix"])}{Utils.BombsiteLimiter_Utilities.ReplaceMessageNewlines(_plugin.Localizer["ErrorTpId", "!removebsentity 1"])}");
            return;
        }
        var arg = commandInfo.GetArg(1);

        _plugin.PropManager!.RemovePropFromFile(arg);
        player.PrintToChat($"{Utils.BombsiteLimiter_Utilities.ReplaceMessageNewlines(_plugin.Localizer["Prefix"])}{Utils.BombsiteLimiter_Utilities.ReplaceMessageNewlines(_plugin.Localizer["EntityRemoved", arg])}");
    }

    private void TeleportToEntity(CCSPlayerController? player, CommandInfo commandInfo)
    {
        if (player == null || player.PlayerPawn == null) return;
        if (!_plugin.Config.PlacingMode) return;
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat($"{Utils.BombsiteLimiter_Utilities.ReplaceMessageNewlines(_plugin.Localizer["Prefix"])}{Utils.BombsiteLimiter_Utilities.ReplaceMessageNewlines(_plugin.Localizer["NoPermission"])}");
            return;
        }

        if (commandInfo.ArgCount < 2)
        {
            player.PrintToChat($"{Utils.BombsiteLimiter_Utilities.ReplaceMessageNewlines(_plugin.Localizer["Prefix"])}{Utils.BombsiteLimiter_Utilities.ReplaceMessageNewlines(_plugin.Localizer["ErrorTpId", "!tpbsentity 2"])}");
            return;
        }
        var arg = commandInfo.GetArg(1);
        var id = Convert.ToInt32(arg);
        var prop = _plugin.PropManager!.GetPropById(id);

        //Adding + 25 since props have collision 
        var propPos = new Vector(prop!.posX + 25, prop!.posY + 25, prop!.posZ);

        player.PlayerPawn.Value!.Teleport(propPos);

    }


    private void ShowEntityList(CCSPlayerController? player, CommandInfo commandInfo)
    {
        if (player == null || player.PlayerPawn == null) return;
        if (!_plugin.Config.PlacingMode) return;
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat($"{Utils.BombsiteLimiter_Utilities.ReplaceMessageNewlines(_plugin.Localizer["Prefix"])}{Utils.BombsiteLimiter_Utilities.ReplaceMessageNewlines(_plugin.Localizer["NoPermission"])}");
            return;
        }

        foreach (var entity in _plugin.PropManager!._props)
        {
            player.PrintToConsole($"{entity.Id} | X: {entity.posX}, Y: {entity.posY}, Z: {entity.posZ} | Site: {entity.bs} ");
        }

        player.PrintToChat($"{Utils.BombsiteLimiter_Utilities.ReplaceMessageNewlines(_plugin.Localizer["Prefix"])}{Utils.BombsiteLimiter_Utilities.ReplaceMessageNewlines(_plugin.Localizer["PrintedInConsole"])}");
        return;
    }
    private void OnPlacingMode(CCSPlayerController? player, CommandInfo commandInfo)
    {
        if (player == null || player.PlayerPawn == null) return;
        if (!_plugin.Config.PlacingMode) return;
        if (!AdminManager.PlayerHasPermissions(player, "@css/root"))
        {
            player.PrintToChat($"{Utils.BombsiteLimiter_Utilities.ReplaceMessageNewlines(_plugin.Localizer["Prefix"])}{Utils.BombsiteLimiter_Utilities.ReplaceMessageNewlines(_plugin.Localizer["NoPermission"])}");
            return;
        }

        var slot = player.Slot;

        try
        {
            var placingEntry = _plugin.PlacingPlayers.FirstOrDefault(p => p.Slot == slot);
            if (placingEntry != null)
            {
                _plugin.PlacingPlayers.RemoveAt(slot);
                player.PrintToChat($"{Utils.BombsiteLimiter_Utilities.ReplaceMessageNewlines(_plugin.Localizer["Prefix"])}{Utils.BombsiteLimiter_Utilities.ReplaceMessageNewlines(_plugin.Localizer["PlacingOff"])}");
                return;
            }
            else
            {
                if (commandInfo.ArgCount < 2)
                {
                    player.PrintToChat($"{Utils.BombsiteLimiter_Utilities.ReplaceMessageNewlines(_plugin.Localizer["Prefix"])}{Utils.BombsiteLimiter_Utilities.ReplaceMessageNewlines(_plugin.Localizer["ErrorChooseSite", "!placingmode 1"])}");
                    return;
                }
                var arg = commandInfo.GetArg(1);
                var newId = Convert.ToInt32(arg);
                if (newId != 1 && newId != 2)
                {
                    player.PrintToChat($"{Utils.BombsiteLimiter_Utilities.ReplaceMessageNewlines(_plugin.Localizer["Prefix"])}{Utils.BombsiteLimiter_Utilities.ReplaceMessageNewlines(_plugin.Localizer["ErrorChooseSite", "!placingmode 1"])}");
                    return;
                }
                _plugin.PlacingPlayers.Add(new PlacingModel
                {
                    Slot = player.Slot,
                    PlacingAllowed = true,
                    site = arg
                });
                player.PrintToChat($"{Utils.BombsiteLimiter_Utilities.ReplaceMessageNewlines(_plugin.Localizer["Prefix"])}{Utils.BombsiteLimiter_Utilities.ReplaceMessageNewlines(_plugin.Localizer["PlacingOn"])}");
            }

        }
        catch (Exception ex)
        {
            _plugin.Logger.LogInformation($"{ex}");
        }

    }

}