using System.Drawing;
using System.Runtime.InteropServices.Marshalling;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Entities.Constants;
using CounterStrikeSharp.API.Modules.Utils;


namespace CS2_Poor_BombsiteLimiter.Utils;

public class BombsiteLimiter_Utilities(CS2_Poor_BombsiteLimiter plugin)
{
    private readonly CS2_Poor_BombsiteLimiter _plugin = plugin;



    public int GetAllPlayers()
    {
        var playersList = Utilities.GetPlayers().Where(p => !p.IsHLTV && p.Connected == PlayerConnectedState.PlayerConnected && (p.Team == CsTeam.Terrorist || p.Team == CsTeam.CounterTerrorist)).ToList();

        if (!_plugin.Config.CountBots)
        {
            playersList.RemoveAll(p => p.IsBot);
        }

        if (_plugin.Config.Team == 1 || _plugin.Config.Team == 2)
        {
            playersList.RemoveAll(p => p.TeamNum != _plugin.Config.Team + 1);
        }

        return playersList.Count();
    }

    /*
    * These two functions below were yoinked from SharpTimer by Dea
    * DrawLaserBetween was mostly copied from CounterStrikeSharp discord, from code-snippset https://discord.com/channels/1160907911501991946/1175947333880524962/1189384833646985276
    * DrawWireFrame3D was yoinked entirely with some changes from sharptimer since i was too lazy to make it by myself: https://github.com/Letaryat/poor-sharptimer/blob/main/src/Plugin/Utils.cs#L482
    */

    static public void DrawLaserBetween(Vector startPos, Vector endPos, string _color)
    {
        CBeam beam = Utilities.CreateEntityByName<CBeam>("beam")!;
        if (beam == null) { return; }

        //remove +5 from pos1 and pos2 if you want to beam to be at the bottom.
        var pos1 = new Vector(startPos.X, startPos.Y, startPos.Z + 5);
        var pos2 = new Vector(endPos.X, endPos.Y, endPos.Z + 5);

        beam.Render = Color.FromName(_color);
        beam.Width = 1.0f;
        beam.Teleport(pos1, new QAngle(), new Vector());
        beam.EndPos.Add(pos2);
        beam.DispatchSpawn();
    }
    public void DrawWireframe3D(Vector corner1, Vector corner8, string _color)
    {
        Vector corner2 = new(corner1.X, corner8.Y, corner1.Z);
        Vector corner3 = new(corner8.X, corner8.Y, corner1.Z);
        Vector corner4 = new(corner8.X, corner1.Y, corner1.Z);

        Vector corner5 = new(corner8.X, corner1.Y, corner8.Z + 0);
        Vector corner6 = new(corner1.X, corner1.Y, corner8.Z + 0);
        Vector corner7 = new(corner1.X, corner8.Y, corner8.Z + 0);

        //bottom square
        DrawLaserBetween(corner5, corner6, _color);
        DrawLaserBetween(corner6, corner7, _color);
        DrawLaserBetween(corner7, corner8, _color);
        DrawLaserBetween(corner8, corner5, _color);

        if (_plugin.Config.ThreeDeeBox)
        {
            //top square
            DrawLaserBetween(corner1, corner2, _color);
            DrawLaserBetween(corner2, corner3, _color);
            DrawLaserBetween(corner3, corner4, _color);
            DrawLaserBetween(corner4, corner1, _color);

            //connect them both to build a cube
            DrawLaserBetween(corner1, corner6, _color);
            DrawLaserBetween(corner2, corner7, _color);
            DrawLaserBetween(corner3, corner8, _color);
            DrawLaserBetween(corner4, corner5, _color);
        }

    }

    public static string ReplaceMessageNewlines(string input)
    {
        return input.Replace("\n", "\u2029");
    }

}