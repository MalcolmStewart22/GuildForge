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

    public void Start()
    {
        gameState = new GameState();
        GameStateQueries.Setup(Config);
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
        for (int i = 0; i < Config.MaxPartySize; i++)
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
        gameState.Ledger.Add(new LedgerLine(gameState.DayCount, gameState.CurrentGold, gameState.CurrentGold, "Guild Open"));
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
        gameState.Ledger.Add(new LedgerLine(gameState.DayCount, gameState.CurrentGold, 0 - amount, reason));
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

        List<MissionResult> _todaysMissions = new();
        foreach(var party in gameState.Parties)
        {
            if(party.IsOnMission == true)
            {
                MissionResult newMission = dungeonResolver.EnterDungeon(party.CurrentMission, party, EventLibrary, eventResolver, Config.OutcomeOptions, gameState.DayCount);
                gameState.MissionLog.Add(newMission);
                _todaysMissions.Add(newMission);
                gameState.CurrentGold += newMission.GoldGained;
                gameState.Ledger.Add(new LedgerLine(gameState.DayCount, gameState.CurrentGold, newMission.GoldGained, "Dungeon Income"));
            }
        }
        
        

        foreach (var character in gameState.Characters)
        {
            int _wage = 0;
            //add rank costs later
            if(character.IsAlive)
            {
                if(character.IsResting)
                {                
                    _wage = GameStateQueries.GetWage(character.Rank) + Config.RestingCost;
                    character.HealthCheck(Config.HPRefusalThreshold, Config.HPReadyForActionThreshold, Config.BaseRestHeal);
                }
                else
                {
                    _wage = GameStateQueries.GetWage(character.Rank);
                    character.HealthCheck(Config.HPRefusalThreshold, Config.HPReadyForActionThreshold, Config.BaseRestHeal);
                }
            }
            PayGold(_wage, $"Wages for {character.Name}");
            
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
            ui.EndOfDay( _todaysMissions); 
        }
    }

    private bool IsEveryoneDead()
    {
        foreach(var c in gameState.Characters)
        {
            if(!c.IsAlive)
            {
                gameState.Recruited.Remove(c);
            }
        }

        return gameState.Recruited.Count == 0;
    }
}
