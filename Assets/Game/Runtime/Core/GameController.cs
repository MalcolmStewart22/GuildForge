using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.Linq;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private GameState gameState;
    public SO_GameConfig Config;
    public SO_TagLibrary TagLibrary;
    public SO_EventLibrary EventLibrary;
    public SO_TraitLibrary TraitLibrary;
    [SerializeField]
    private UIController ui;
    [SerializeField]
    private InputController input;


    public GameState State => gameState;
    private DungeonResolver dungeonResolver;
    private EventResolver eventResolver;

    public void Start()
    {
        gameState = new GameState();
        ui.Setup();
    }

    public void StartGame()
    {
        Debug.Log("Starting Game");
        CharacterGenerator _cg = new CharacterGenerator();
        DungeonGenerator _dg = new DungeonGenerator();
        dungeonResolver = new();
        eventResolver = new();

        Debug.Log("======= Character Creation ========");
        for (int i = 0; i < Config.NumberOfCharacters; i++)
        {
            gameState.Characters.Add(_cg.GenerateCharacter(gameState.Characters.Count, Config.SyllableSet, TraitLibrary));
        }
        //temp for MVP
        gameState.Recruited = new List<Character>(gameState.Characters);
        Party _party = new();
        _party.PartyMembers = new List<Character>(gameState.Recruited);
        gameState.Parties.Add(_party);

        Debug.Log("======= Dungeon Creation ========");
        for (int i = 0; i < Config.NumberOfDungeons; i++)
        {
            gameState.Dungeons.Add(_dg.GenerateDungeon(gameState.Dungeons.Count,TagLibrary));
        }

        Debug.Log("======= Data Creation Finished ========");

        gameState.CurrentGold = Config.startingGold;
        ui.LoadMapScreen(gameState.CurrentGold, gameState.DayCount);
        ui.DisplayMapButtons(gameState.Dungeons);

        Debug.Log("======= Game Start ========");
    }
    public void Restart()
    {
        gameState = new();
        StartGame();
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void DungeonStart(DungeonInstance dungeon)
    {
        //will need to implement party selection
        if (gameState.Parties[0].IsOnMission == false)
        {
            gameState.Parties[0].IsOnMission = true;
            gameState.Parties[0].CurrentMission = dungeon;
            gameState.Parties[0].PrepareParty();
        }
        ui.CloseFocusedElement();
        ui.CloseFocusedElement();

        //remove after adding In Town Actions
        bool _AllPartiesAssigned = true;
        foreach(var party in gameState.Parties)
        {
            if(party.IsOnMission == false)
            {
                _AllPartiesAssigned = false;
                break;
            }
        }
        if(_AllPartiesAssigned)
        {
            ui.ShowAllPartiesAssigned();
        }
    }

    public void ReadyForNextDay()
    {
        bool _isAPartyAssigned = false;
        foreach(var party in gameState.Parties)
        {
            if(party.IsOnMission == true)
            {
                _isAPartyAssigned = true;
                break;
            }
        }
        if(_isAPartyAssigned)
        {
            EndDay();
        }
        else
        {
            ui.ShowNoPartyAssignedWarning();
        }
    }

    public void EndDay()
    {
        while (ui.FocusedElements.Count > 0)
        {
            ui.CloseFocusedElement();
        }

        foreach(var party in gameState.Parties)
        {
            if(party.IsOnMission == true)
            {
                MissionResult newMission = dungeonResolver.EnterDungeon(party.CurrentMission, party, EventLibrary, eventResolver, Config.OutcomeOptions, gameState.DayCount);
                gameState.MissionLog.Add(newMission);
                gameState.CurrentGold += newMission.GoldGained;
            }
        }

        foreach (var character in gameState.Characters)
        {
            //add rank costs later
            if(character.IsAlive)
            {
                if(character.IsResting)
                {                
                    gameState.CurrentGold -= 3;
                    character.HealthCheck(Config.HPRefusalThreshold, Config.HPReadyForActionThreshold, Config.BaseRestHeal);
                }
                else
                {
                    gameState.CurrentGold -= 2;
                    character.HealthCheck(Config.HPRefusalThreshold, Config.HPReadyForActionThreshold, Config.BaseRestHeal);
                }
            }
        }
        // will need to update logic when we add recruitment
        if(gameState.CurrentGold < 1 || IsEveryoneDead())
        {
            Debug.Log("Game Over!");
            ui.GameOver();
            
        }
        else
        {
            gameState.DayCount += 1;
            ui.UpdateForEndOfDay(gameState.CurrentGold, gameState.DayCount, gameState.MissionLog[gameState.MissionLog.Count -1]); 
        }
    }

    private bool IsEveryoneDead()
    {
        foreach(var c in gameState.Recruited)
        {
            if(!c.IsAlive)
            {
                gameState.Recruited.Remove(c);
            }
        }

        return gameState.Recruited.Count == 0;
    }
}
