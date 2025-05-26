using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Events;
using CounterStrikeSharp.API.Modules.Utils;
using CS2_Poor_BombsiteLimiter.Utils;

namespace CS2_Poor_BombsiteLimiter.Managers;

public class EventManager(CS2_Poor_BombsiteLimiter plugin)
{
    private readonly CS2_Poor_BombsiteLimiter _plugin = plugin;
    //private static CounterStrikeSharp.API.Modules.Timers.Timer? hudTimer;
    public bool ShowHud = true;
    public void RegisterEvents()
    {
        //Events:
        _plugin.RegisterEventHandler<EventRoundStart>(OnRoundStart, HookMode.Post);
        _plugin.RegisterEventHandler<EventRoundEnd>(OnRoundEnd);
        _plugin.RegisterEventHandler<EventRoundFreezeEnd>(OnRoundFreezeEnd);
        _plugin.RegisterEventHandler<EventPlayerPing>(OnPlayerPing);

        //Listeners:
        _plugin.RegisterListener<Listeners.OnServerPrecacheResources>((ResourceManifest manifest) =>
        {
            manifest.AddResource("models/props/de_nuke/hr_nuke/chainlink_fence_001/chainlink_fence_001_128_capped.vmdl");
            //manifest.AddResource("models/chicken/chicken.vmdl");
        });
        _plugin.RegisterListener<Listeners.OnMapStart>(OnMapStart);
        _plugin.RegisterListener<Listeners.OnTick>(OnTick);

    }


    public HookResult OnRoundStart(EventRoundStart @event, GameEventInfo info)
    {
        _plugin.BombsiteManager!.DisableBombsite();
        return HookResult.Continue;
    }
    public HookResult OnRoundEnd(EventRoundEnd @event, GameEventInfo info)
    {
        _plugin.BombsiteManager!.ResetBombsites();

        return HookResult.Continue;
    }


    private HookResult OnRoundFreezeEnd(EventRoundFreezeEnd @event, GameEventInfo info)
    {
        ShowHud = true;
        _plugin.AddTimer(_plugin.Config.HudTimer, () =>
        {
            ShowHud = false;
        });
        return HookResult.Continue;
    }


    private HookResult OnPlayerPing(EventPlayerPing @event, GameEventInfo info)
    {
        var player = @event.Userid;
        if (player == null) return HookResult.Continue;
        var pawn = player.PlayerPawn.Value;
        if (pawn == null || !pawn.IsValid) return HookResult.Continue;
        if (!AdminManager.PlayerHasPermissions(player, "@css/root")) return HookResult.Continue;
        if (pawn.PingServices == null || pawn.PingServices.PlayerPing.Value == null) return HookResult.Continue;
        var pingPos = pawn.PingServices!.PlayerPing.Value.AbsOrigin;

        var newpingPos = new Vector(pingPos!.X, pingPos.Y, pingPos.Z);
        
        var slot = player.Slot;
        var placingEntry = _plugin.PlacingPlayers.FirstOrDefault(p => p.Slot == slot);
        if (placingEntry != null)
        {
            if (placingEntry.PlacingAllowed)
            {
                var pos = pawn.AbsOrigin;
                var qangle = new QAngle(0, pawn.EyeAngles.Y, 0);
                
                _plugin.PropManager!.CreateProp(newpingPos!, qangle);
                _plugin.PropManager!.PushCordsToFile(newpingPos!, qangle, placingEntry!.site!);
                player.PrintToChat($"{Utils.BombsiteLimiter_Utilities.ReplaceMessageNewlines(_plugin.Localizer["Prefix"])}{Utils.BombsiteLimiter_Utilities.ReplaceMessageNewlines(_plugin.Localizer["NewEntity", _plugin.BombsiteManager!.bsToString(Convert.ToInt32(placingEntry.site))])}");
            }
        }
        return HookResult.Continue;
    }

    private void OnMapStart(string map)
    {
        _plugin.PropManager!._props.Clear();
        _plugin.AllowPlacingEntities = false;
        _plugin.PlacingPlayers.Clear();

        _plugin.PropManager!._mapName = map;
        _plugin.PropManager!._mapFilePath = Path.Combine(_plugin.ModuleDirectory, "maps", $"{map}.json");

        _plugin.PropManager!.GenerateJsonFile();

        Server.NextFrame(() =>
        {
            _plugin.PropManager!.LoadPropsFromMap();
        });

    }

    private void OnTick()
    {
        if (ShowHud)
        {
            if (_plugin.Config.TypeOfNotification != 1)
            {
                foreach (var player in Utilities.GetPlayers().Where(p => !p.IsBot && !p.IsHLTV))
                {
                    player.PrintToCenterHtml($"{_plugin.Localizer["HudMessageHTML", _plugin.BombsiteManager!.blockedSite!]}");
                }
            }
        }

    }
}