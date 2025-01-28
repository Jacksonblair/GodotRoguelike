using System;
using System.Collections.Generic;
using Godot;
using FileAccess = Godot.FileAccess;

namespace TESTCS.scripts.managers;

/**
 * Requirements:
 * - Able to persistently configure a scene that i want to be the first scene that loads when i press Play
 * - All the game scene related shit shouldn't load until i have a game scene loaded.
 *
 * --- I need to load this gamestate when the game starts, and update it as I go.
 *
 * Problems:
 * - Identify a packed scene using a string
 * - Track when scene we are in using string
 *      - Tell GameState what scene we are in when changing
 * - Load that scene again when we open game
 *      - Game should grab scene based on GameState, and put into the game
 */

[GlobalClass]
public partial class GameStateManager : Node
{
    private const string SaveFileDir = "user://";
    private const string GameStateFilePathPrefix = "gamestate_";
    private const string SlotPrefix = "slot";
    private const string TimestampFormat = "yyyyMMdd_HHmmss";

    private PackedScene StoneLevel;
    private PackedScene OtherLevel;
    
    [Export]
    public GameState GameState { get; set; }

    public static string FormatFileName(int slot, string timestamp)
    {
        return $"{SlotPrefix}{slot}_{GameStateFilePathPrefix}{timestamp}.tres";
    }
    
    public static string CreateNewFileName(int slot)
    {
        var timestamp = DateTime.Now.ToString(TimestampFormat);
        return FormatFileName(slot, timestamp);
    }
    
    public static List<string> GetSavedGameFiles(int slot)
    {
        var dir = DirAccess.Open(SaveFileDir);
        if (dir == null)
        {
            GD.Print("Failed to open directory.");
            return new List<string>();
        }

        List<string> saveFiles = new List<string>();
        var saveFilePrefix = $"{SlotPrefix}{slot}_{GameStateFilePathPrefix}";
        
        foreach (string fileName in dir.GetFiles())
        {
            if (fileName.StartsWith(saveFilePrefix) && fileName.EndsWith(".tres"))
            {
                saveFiles.Add(fileName);
            }
        }
        
        GD.Print($"Found {saveFiles.Count} save files.");

        return saveFiles;
    }
    
    public void SaveGameState(GameState gameState, int slot)
    {
        var fileName = CreateNewFileName(slot);
        var error = ResourceSaver.Save(gameState, SaveFileDir + fileName);
        
        if (error == Error.Ok)
        {
            GD.Print($"Saved game state for slot {slot} at {SaveFileDir}{fileName}");
        }
        else
        {
            GD.Print("Error saving gamestate: ", error.ToString());
        }
    }

    public void SaveCurrentGameState(int slot)
    {
        // TODO: Get SLOT
        SaveGameState(GameState, slot);
    }
    
    public bool LoadGameStateFile(string fileName)
    {
        var path = $"{SaveFileDir}{fileName}";
        // Load the saved game state or create a new one if none exists
        if (FileAccess.FileExists(path))
        {
            GameState = ResourceLoader.Load<GameState>(path);
            return true;
        }
        else
        {
            GD.Print("Gamestate not found, creating new gamestate");
            return false;
        }
    }
    
    public bool LoadMostRecentGameState(int slot)
    {
        var saveFiles = GetSavedGameFiles(slot);

        if (saveFiles.Count == 0)
        {
            GD.Print("No save files found.");
            return false;
        }
        
        // Sort the save files by timestamp (descending)
        saveFiles.Sort((a, b) =>
        {
            string aTimestamp = a.Replace(GameStateFilePathPrefix, "").Replace(".tres", "");
            string bTimestamp = b.Replace(GameStateFilePathPrefix, "").Replace(".tres", "");
            return bTimestamp.CompareTo(aTimestamp); // Descending order
        });

        // Load the most recent save file
        string mostRecentSave = saveFiles[0];
        GD.Print($"Loading most recent save for slot {slot}: {mostRecentSave}");
        return LoadGameStateFile(mostRecentSave);
    }

    public void CreateNewGameSave(int slot)
    {
        GD.Print("Creating new game save");
        SaveGameState(new GameState(), slot);
    }
}