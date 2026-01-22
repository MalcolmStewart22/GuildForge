using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.Linq;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private GameState gameState;
    [SerializeField]
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
    private CharacterGenerator characterGenerator;
    private DungeonGenerator dungeonGenerator;
    private PartyGenerator partyGenerator;

    private void Start()
    {
        gameState = new GameState();
        GameStateQueries.Setup(Config);
        ui.Setup();
    }

    public void StartGame()
    {
        Debug.Log("Starting Game");
        
        dungeonGenerator = new();
        characterGenerator = new();
        dungeonResolver = new();
        eventResolver = new();
        partyGenerator = new();
        
        CreateParty();
        CreateDungeons();
        Debug.Log("======= Data Creation Finished ========");

        gameState.CurrentGold = Config.startingGold;
        gameState.Ledger.Add(new LedgerLine(gameState.WeekCount, gameState.CurrentGold, gameState.CurrentGold, "Guild Open"));
        ui.LoadMapScreen(gameState.CurrentGold, gameState.WeekCount);
        ui.DisplayMapButtons(gameState.Dungeons);

        Debug.Log("======= Game Start ========");
    }

    private void CreateParty()
    {
        characterGenerator.library = new SO_TraitLibrary(TraitLibrary);
        Party _party = partyGenerator.GenerateParty(characterGenerator, Config.MaxPartySize, gameState.Characters.Count, gameState.Parties.Count);
        gameState.Parties.Add(_party);

        foreach (Character c in _party.PartyMembers)
        {
            gameState.Characters.Add(c);
            gameState.Recruited.Add(c);
        }
    }
    private void CreateDungeons() //Change once Mission systems implemented
    {
        Debug.Log("======= Dungeon Creation ========");
        for (int i = 0; i < Config.NumberOfDungeons; i++)
        {
            gameState.Dungeons.Add(dungeonGenerator.GenerateDungeon(gameState.Dungeons.Count,TagLibrary));
        }
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
    public void RankUp(GuildRankEvaluation guildEval)
    {
        gameState.CurrentGuildRank = guildEval.NewRank;
        PayGold(guildEval.PromotionCost, "Guild Promotion");
        ui.UpdateHeaderInfo();
    }
    public void RankUp(CharacterRankEvaluation characterEval, Character character)
    {
        character.RankUp();
        PayGold(characterEval.PromotionCost, $"Promotion for {character.Name}");
        ui.UpdateHeaderInfo();
    }

    public void PayGold(int amount, string reason)
    {
        gameState.CurrentGold -= amount;
        gameState.Ledger.Add(new LedgerLine(gameState.WeekCount, gameState.CurrentGold, 0 - amount, reason));
    }
    public void AddGold(int amount, string reason)
    {
        gameState.CurrentGold += amount;
        gameState.Ledger.Add(new LedgerLine(gameState.WeekCount, gameState.CurrentGold, amount, reason));
    }
    public void DungeonStart(DungeonInstance dungeon, Party party)
    {
        //will need to implement party selection
        if (party.AssignedAction == PartyAction.Unassigned)
        {
            party.PrepareParty(dungeon);
        }
        ui.CloseFocusedElement();
        ui.CloseFocusedElement();

        //remove after adding In Town Actions
        bool _AllPartiesAssigned = true;
        foreach(var p in gameState.Parties)
        {
            if(p.AssignedAction == PartyAction.Unassigned)
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

    public void ReadyForNextWeek()
    {
        bool _isAPartyAssigned = false;
        foreach(var party in gameState.Parties)
        {
            if(party.AssignedAction == PartyAction.Dungeon)
            {
                _isAPartyAssigned = true;
                break;
            }
        }
        if(_isAPartyAssigned)
        {
            EndWeek();
        }
        else
        {
            ui.ShowNoPartyAssignedWarning();
        }
    }  

    public void EndWeek()
    {
        while (ui.FocusedElements.Count > 0)
        {
            ui.CloseFocusedElement();
        }

        List<MissionResult> _weeksMissions = new List<MissionResult>(RunDungeons());            

        foreach (Party p in gameState.Parties)
        {
            PayGold(p.GetWages(), $"Wages for {p.PartyName}");
            p.HealthCheck(Config.HPConfig);
        }
        
        if(IsTheGameOver())
        {
            Debug.Log("Game Over!");
            ui.GameOver(); 
        }
        else
        {
            gameState.WeekCount += 1;
            ui.EndOfWeek( _weeksMissions); 
        }
    }

    private List<MissionResult> RunDungeons()
    {
        List<MissionResult> _results = new();
        foreach(var party in gameState.Parties)
        {
            if(party.AssignedAction == PartyAction.Dungeon)
            {
                MissionResult newMission = dungeonResolver.EnterDungeon(party.CurrentMission, party, EventLibrary, eventResolver, Config.OutcomeOptions, gameState.WeekCount);
                gameState.MissionLog.Add(newMission);
                _results.Add(newMission);
                AddGold(newMission.GoldGained, "Dungeon Income");
            }
            else
            {
                foreach (Character c in party.PartyMembers)
                {
                    c.Heal(Config.HPConfig.BaseRestHeal);
                }
            }
        }
        return _results;
    }

    private bool IsTheGameOver()
    {
        bool _outOfMoney = gameState.CurrentGold < 0; //will need to change this to the recruitment cost once its implemented.

        foreach(var c in gameState.Characters)
        {
            if(!c.IsAlive)
            {
                gameState.Recruited.Remove(c);
            }
        }

        return _outOfMoney && gameState.Recruited.Count == 0;
    }
}
