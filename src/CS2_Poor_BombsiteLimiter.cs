using CounterStrikeSharp.API.Core;
using Microsoft.Extensions.Logging;
using CS2_Poor_BombsiteLimiter.Managers;
using CS2_Poor_BombsiteLimiter.Config;
using CS2_Poor_BombsiteLimiter.Utils;
using CS2_Poor_BombsiteLimiter.Models;
using CounterStrikeSharp.API;
namespace CS2_Poor_BombsiteLimiter;

public class CS2_Poor_BombsiteLimiter : BasePlugin, IPluginConfig<PluginConfig>
{
    public override string ModuleName => "Poor Bombsitelimiter";

    public override string ModuleAuthor => "Letaryat";
    public override string ModuleVersion => "1.0";

    public required PluginConfig Config { get; set; }
    public static CS2_Poor_BombsiteLimiter? Instance { get; private set; }
    public EventManager? EventManager { get; private set; }
    public BombsiteLimiter_Utilities? BombsiteUtils { get; private set; }

    public BombsiteManager? BombsiteManager { get; private set; }

    public CommandsManager? CommandsManager { get; private set; }

    public PropManager? PropManager { get; private set; }

    public List<PlacingModel> PlacingPlayers = new();
    public bool AllowPlacingEntities = false;
    public override void Load(bool hotReload)
    {
        //Logger.LogInformation("Poor bombsite limiter loaded!");

        Console.WriteLine("CS2_Poor_BombsiteLimiter loaded! HF!");

        Instance = this;
        EventManager = new EventManager(this);
        BombsiteUtils = new BombsiteLimiter_Utilities(this);
        CommandsManager = new CommandsManager(this);
        BombsiteManager = new BombsiteManager(this);
        PropManager = new PropManager(this);

        EventManager.RegisterEvents();
        CommandsManager.RegisterCommands();

    }

    public void OnConfigParsed(PluginConfig config)
    {
        Config = config;
    }
    public override void Unload(bool hotReload)
    {
        Console.WriteLine("CS2_Poor_BombsiteLimiter unloaded!");
    }

    public string DebugLog(string message)
    {
        if (Config.Debug)
        {
            Logger.LogInformation($"PoorBombsiteLimiter | {message}");
        }
        return string.Empty;
        
    }

}
