using System.Drawing;
using System.Text.Json;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;
using CS2_Poor_BombsiteLimiter.Models;
using Microsoft.Extensions.Logging;
using Serilog.Core;

namespace CS2_Poor_BombsiteLimiter.Managers;

public class PropManager(CS2_Poor_BombsiteLimiter plugin)
{
    private readonly CS2_Poor_BombsiteLimiter _plugin = plugin;
    public string? _mapName;
    public string? _mapFilePath;
    public readonly List<PropModel> _props = [];

    private static readonly object _fileLock = new();

    public void GetMapInfoOnReload()
    {
        var map = Server.MapName;
        _mapName = map;
        _mapFilePath = Path.Combine(_plugin.ModuleDirectory, "maps", $"{map}.json");
    }
    public void GenerateJsonFile()
    {
        string directoryPath = Path.Combine(_plugin.ModuleDirectory, "maps");
        try
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            if (!File.Exists(_mapFilePath))
            {
                File.WriteAllText(_mapFilePath!, "[]");
            }
        }
        catch (Exception ex)
        {
            _plugin.DebugLog($"{ex}");
        }
    }

    public void PushCordsToFile(Vector pos, QAngle angle, string bs)
    {
        lock (_fileLock)
        {
            if (pos == null || angle == null) return;

            int newId = _props.Count();
            int newBs = 0;
            if (bs == "A" || bs == "1") newBs = 1;
            else if (bs == "B" || bs == "2") newBs = 2;

            _props.Add(new PropModel
            {
                Id = newId,
                bs = newBs,
                posX = pos.X,
                posY = pos.Y,
                posZ = pos.Z,
                angleY = angle.Y
            });

            var options = new JsonSerializerOptions { WriteIndented = true };
            File.WriteAllText(_mapFilePath!, JsonSerializer.Serialize(_props, options));

            LoadPropsFromMap();
        }
    }

    public void LoadPropsFromMap()
    {
        if (File.Exists(_mapFilePath))
        {
            string json = File.ReadAllText(_mapFilePath);
            if (!string.IsNullOrWhiteSpace(json))
            {
                _props.Clear();
                var loadedProps = JsonSerializer.Deserialize<List<PropModel>>(json) ?? [];
                _props.AddRange(loadedProps);
            }
        }
    }

    public void CreateProp(Vector pos, QAngle angle)
    {
        if (pos == null || angle == null) return;
        var model = _plugin.Config.FenceModel;
        CDynamicProp prop;

        prop = Utilities.CreateEntityByName<CDynamicProp>("prop_dynamic_override")!;
        prop.Collision.SolidType = SolidType_t.SOLID_VPHYSICS;

        prop.CBodyComponent!.SceneNode!.Owner!.Entity!.Flags &= unchecked((uint)~(1 << 2));

        prop.SetModel(model);
        prop.DispatchSpawn();

        prop.Teleport(pos, angle);
    }
    public void SpawnProps(int bs)
    {
        if (!_plugin.Config.PlacingMode) return;
        foreach (var prop in _props.Where(p => p.bs == bs))
        {
            CreateProp(new Vector(prop.posX, prop.posY, prop.posZ), new QAngle(0, prop.angleY, 0));
        }
    }

    public PropModel? GetPropById(int id)
    {
        return id >= 0 && id < _props.Count ? _props[id] : null;
    }
    public void RemovePropFromFile(string idstring)
    {
        int id = Convert.ToInt32(idstring);
        if (id < 0 || id >= _props.Count()) return;
        _props.RemoveAt(id);

        for (int i = 0; i < _props.Count; i++)
        {
            _props[i].Id = i;
        }
        var options = new JsonSerializerOptions { WriteIndented = true };
        File.WriteAllText(_mapFilePath!, JsonSerializer.Serialize(_props, options));
    }

    public void CreateBombsiteSprite(CBaseEntity bs)
    {
        if (string.IsNullOrWhiteSpace(_plugin.Config.BombsiteSprite)) return;
        if (bs == null) return;
        var mid = _plugin.BombsiteUtils!.CalculateMid(bs);
        if (mid == null) return;
        CParticleSystem particle = Utilities.CreateEntityByName<CParticleSystem>("info_particle_system")!;

        particle.EffectName = _plugin.Config.BombsiteSprite;
        particle.Teleport(new Vector(mid!.X, mid.Y, mid.Z + _plugin.Config.BombsiteSpriteHeight), QAngle.Zero, Vector.Zero);

        particle.StartActive = true;
        particle.DispatchSpawn();

        particle.AcceptInput("Start");
    }

    public void CreateBombsiteText(CBaseEntity bs)
    {
        if (!_plugin.Config.TextDisplay) return;
        var mid = _plugin.BombsiteUtils!.CalculateMid(bs);
        if (mid == null) return;
        CPointWorldText textEnt = Utilities.CreateEntityByName<CPointWorldText>("point_worldtext")!;
        textEnt.MessageText = _plugin.Config.TextMessage;
        textEnt.Enabled = true;
        textEnt.DepthOffset = 0.0f;
        textEnt.WorldUnitsPerPx = 1f;
        textEnt.FontName = _plugin.Config.FontName;
        textEnt.FontSize = _plugin.Config.FontSize;
        textEnt.Color = Color.FromName(_plugin.Config.TextColor);
        textEnt.JustifyHorizontal = PointWorldTextJustifyHorizontal_t.POINT_WORLD_TEXT_JUSTIFY_HORIZONTAL_CENTER;
        textEnt.JustifyVertical = PointWorldTextJustifyVertical_t.POINT_WORLD_TEXT_JUSTIFY_VERTICAL_CENTER;
        textEnt.ReorientMode = PointWorldTextReorientMode_t.POINT_WORLD_TEXT_REORIENT_AROUND_UP;
        textEnt.Spawnflags = (uint)(
            WorldTextPanelOrientation_t.WORLDTEXT_ORIENTATION_FACEUSER | WorldTextPanelOrientation_t.WORLDTEXT_ORIENTATION_FACEUSER_UPRIGHT
        );
        textEnt.Teleport(new Vector(mid!.X, mid.Y, mid.Z + _plugin.Config.TextHeight), new QAngle(0, 0, 100), Vector.Zero);
        textEnt.DispatchSpawn();
    }

}