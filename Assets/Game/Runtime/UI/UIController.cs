using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private GameController gameController;
    [SerializeField]
    private DungeonListView dungeonListView;
    [SerializeField]
    private PartyListView partyListView;
    [SerializeField]
    private CharacterListView characterListView;
    [SerializeField]
    private DetailPanel detailPanel;
    [SerializeField]
    private MainInfoPanel mainInfoPanel;
    private VisualElement root;
    private VisualElement mainMenu;
    private VisualElement mapScreen;
    private VisualElement pauseMenu;
    private VisualElement EndGameScreen;
    private List<Button> mapButtons = new List<Button>();
    public Stack<VisualElement> FocusedElements = new();
    private DungeonInstance currentDungeon;
    private Character currentCharacter;
    private Party currentParty;
    private List<MissionResult> currentMissionList = new();
    private int missionReportIndex = 0;
    private System.Action onConfirm;
    
    public void Setup()
    {
        currentCharacter = null;
        currentDungeon = null;
        currentParty = null;

        if (root == null)
        {
            root = GetComponent<UIDocument>().rootVisualElement;
            mapScreen = root.Q<VisualElement>("MapLayer");
            mainMenu = root.Q<VisualElement>("MainMenuLayer");
            pauseMenu = root.Q<VisualElement>("PauseMenuLayer");
            EndGameScreen = root.Q<VisualElement>("GameOverMenuLayer");
            
            LoadMainMenu();
            SetupButtons();
            SetupUIEvents();
        }
    }

    private void LoadMainMenu()
    {        
        mainMenu.style.display = DisplayStyle.Flex;
        mapScreen.style.display = DisplayStyle.None;
        pauseMenu.style.display = DisplayStyle.None;
        EndGameScreen.style.display = DisplayStyle.None;
    }
    private void SetupButtons()
    {
        List<Button> _menuButtons = root.Query<Button>(className:"MenuButton").ToList();
        mapButtons = root.Query<Button>(className:"MapButton").ToList();
        List<Button>  _exitButtons = root.Query<Button>(className:"ExitButtons").ToList();
        List<Button>  _characterButtons = root.Query<Button>(className:"CharacterButton").ToList();
        List<Button>  _headerButtons = root.Query<Button>(className:"HeaderButtons").ToList();

        foreach (Button button in _menuButtons)
        {
            var btn = button;
            switch(btn.name)
            {
                case "NextButton":
                    btn.clicked += () => OnNextButtonClicked();
                    break;
                case "ResumeButton":
                    btn.clicked += () => CloseFocusedElement();
                    break;
                case "StartOverButton":
                    btn.clicked += () => gameController.Restart();
                    break;
                case "SendPartyButton":
                    //will need to implement party selection screens later
                    btn.clicked += () => gameController.DungeonStart(currentDungeon, currentParty);
                    break;
                case "ConfirmButton":
                    btn.clicked += () => CloseFocusedElement();
                    break;
                case "SavePartyConfig":
                    btn.clicked += () => detailPanel.SavePartyConfig();
                    break;
                default:
                    btn.clicked += () => OnMenuClicked(btn.name);
                    break;
            } 
        }
        foreach (Button button in mapButtons)
        {
            Button btn = button;
            btn.clicked += () => OnMapClicked(btn.name);
        }
        foreach (Button button in _exitButtons)
        {
            Button btn = button;
            btn.clicked += () => OnExitClicked(btn.name);
        }
        foreach (Button button in _characterButtons)
        {
            Button btn = button;
            btn.clicked += () => OnCharacterButtonClicked(btn.name);
        }
        foreach (Button button in _headerButtons)
        {
            Button btn = button;
            btn.clicked += () => OnHeaderClicked(btn.name);
        }

        Button _dialogConfirm = root.Q<Button>("DialogConfirm");
        Button _dialogCancel = root.Q<Button>("DialogCancel");
        _dialogConfirm.clicked += OnConfirmClicked;
        _dialogCancel.clicked += () =>  CloseFocusedElement();

        Button _mainInfoConfirm = root.Q<Button>("MainInfoConfirmButton");
        _mainInfoConfirm.clicked +=  OnConfirmClicked;

        Button _reportBack = root.Q<Button>("ReportBackButton");
        _reportBack.clicked += () => OnReportButtonClicked("ReportBackButton");
        Button _reportForward = root.Q<Button>("ReportForwardButton");
        _reportBack.clicked += () => OnReportButtonClicked("ReportForwardButton");
    }
    private void SetupUIEvents()
    {
        dungeonListView.OnDungeonSelected += DungeonSelected;
        partyListView.OnPartySelected += PartySelected;
        characterListView.OnCharacterSelected += CharacterSelected;
    }
    
    public void LoadMapScreen(int gold, int week)
    {
        mainMenu.style.display = DisplayStyle.None;
        mapScreen.style.display = DisplayStyle.Flex;
        EndGameScreen.style.display = DisplayStyle.None;

        VisualElement _header = root.Q<VisualElement>("InfoHeader");

        Button _week = _header.Q<Button>("WeekButton");
        _week.text = "Week# " + gameController.State.WeekCount.ToString();

        Label _gold = _header.Q<Label>("GoldNumber");
        _gold.text = gold.ToString();

        Button _guildRank = _header.Q<Button>("GuildRankButton");
        _guildRank.text = "Guild Rank: " + gameController.State.CurrentGuildRank;
    }
    
    #region Buttons
    private void OnMenuClicked(string name)
    {
        switch(name)
        {
            case "PlayButton":
                gameController.StartGame();
                break;
            case "OptionButton":
                break;
            case "QuitButton":
                gameController.QuitGame();
                break;
        }
        currentParty = gameController.State.Parties[0]; // Will need to change this when party selection is added.
    } 
    private void OnMapClicked(string name)
    {
        if(FocusedElements.Count > 0)
        {
            CloseFocusedElement();
        }

        Debug.Log(name + "clicked!");
        var _sideboard = root.Q<VisualElement>("Sideboard");
        _sideboard.style.display = DisplayStyle.Flex;
        FocusedElements.Push(_sideboard);
        var _guildView = _sideboard.Q<VisualElement>("GuildLayer");
        var _dungeonView = _sideboard.Q<VisualElement>("DungeonLayer");

        if(name == "CityButton")
        {
            _guildView.style.display = DisplayStyle.Flex;
            _dungeonView.style.display = DisplayStyle.None;
            partyListView.ShowRoster(gameController.State.Parties);
        }
        else
        {
            _guildView.style.display = DisplayStyle.None;
            _dungeonView.style.display = DisplayStyle.Flex;
            var _listTitle = _sideboard.Q<Label>("BiomeName");
            switch (name)
            {
                case "DesertButton":
                    _listTitle.text = "Desert Dungeons";
                    break;
                case "PlainsButton":
                    _listTitle.text = "Plains Dungeons";
                    break;
                case "MountainButton":
                    _listTitle.text = "Mountains Dungeons";
                    break;
                case "FrozenWastesButton":
                    _listTitle.text = "Frozen Wastes Dungeons";
                    break;
                case "ForestButton":
                    _listTitle.text = "Forest Dungeons";
                    break;
                case "SwampButton":
                    _listTitle.text = "Swamp Dungeons";
                    break;
                case "SubterraneanButton":
                    _listTitle.text = "Subterranean Dungeons";
                    break;
                case "VolcanicButton":
                    _listTitle.text = "Volcanic Dungeons";
                    break;
            }
            
            List<DungeonInstance> list = GameStateQueries.GetFilteredDungeonList(name, gameController.State);
            dungeonListView.ShowDungeon(list);
        }
    } 
    private void OnNextButtonClicked()
    {
        Debug.Log("====== NEXT Week =======");
        gameController.ReadyForNextWeek();
    }
    private void OnConfirmClicked()
    {
        CloseFocusedElement();
        onConfirm?.Invoke();
        onConfirm = null;
    }
    public void EscapePressed()
    {
        if (FocusedElements.Count == 0)
        {
            OpenMenu();
        }
        else
        {
            CloseFocusedElement();
        }
    }
    private void OnExitClicked(string name)
    {
        Debug.Log(name);
        switch (name)
        {
            case "SideboardExitButton":
                root.Q<VisualElement>("Sideboard").style.display = DisplayStyle.None;
                root.Q<VisualElement>("DetailsLayer").style.display = DisplayStyle.None;
                FocusedElements.Clear();
                break;
            default:
                CloseFocusedElement();
                break;
        }
    }
    private void OnCharacterButtonClicked(string button)
    {
        switch(button)
        {
            case "HealButton":
                currentCharacter.MagicHeal();
                gameController.PayGold(currentCharacter.GetHealingCost(), $"Magical Healing for {currentCharacter.Name}");
                break;
            case "RankUpButton":
                if(currentCharacter.Rank != CharacterRank.S)
                {
                     DisplayMainInfoPanel("CharacterRankLayer");
                }
                break;
        }
    }
    private void OnReportButtonClicked(string button)
    {
        switch(button)
        {
            case "ReportForwardButton":  
                if(missionReportIndex < currentMissionList.Count -1)
                {
                    missionReportIndex += 1;
                }
                break;
            case "ReportBackButton":
                if(missionReportIndex > 0)
                {
                    missionReportIndex -= 1;
                }
                break;

        }
        DisplayMainInfoPanel("MissionLogLayer");
    }
    private void OnHeaderClicked(string name)
    {
        switch(name)
        {
            case "GoldButton":
                DisplayMainInfoPanel("GoldLedgerLayer");
                break;
            case "WeekButton":
                currentMissionList = new List<MissionResult>(gameController.State.MissionLog);
                missionReportIndex = 0;
                DisplayMainInfoPanel("MissionLogLayer");
                break;
            case "GuildRankButton":
                DisplayMainInfoPanel("GuildRankLayer");
                break;
        }
    }
    #endregion
    public void DisplayMapButtons(List<DungeonInstance> dungeonList)
    {
        foreach( var b in mapButtons)
        {
            if(b.name != "CityButton")
            {
                b.style.display = DisplayStyle.None;
            } 
        }
        //will need changes after dungeon generator changes
        foreach (var dungeon in dungeonList)
        {
             Button _btn;
            switch (dungeon.Biome.Name)
            {
                case "Desert":
                    _btn = mapButtons.FirstOrDefault(b => b.name == "DesertButton");
                    _btn.style.display = DisplayStyle.Flex;
                    break;
                case "Forest":
                    _btn = mapButtons.FirstOrDefault(b => b.name == "ForestButton");
                    _btn.style.display = DisplayStyle.Flex;
                    break;
                case "Frozen Wastes":
                    _btn = mapButtons.FirstOrDefault(b => b.name == "FrozenWastesButton");
                    _btn.style.display = DisplayStyle.Flex;
                    break;
                case "Mountains":
                    _btn = mapButtons.FirstOrDefault(b => b.name == "MountainButton");
                    _btn.style.display = DisplayStyle.Flex;
                    break;
                case "Plains":
                    _btn = mapButtons.FirstOrDefault(b => b.name == "PlainsButton");
                    _btn.style.display = DisplayStyle.Flex;
                    break;
                case "Subterranean":
                    _btn = mapButtons.FirstOrDefault(b => b.name == "SubterraneanButton");
                    _btn.style.display = DisplayStyle.Flex;
                    break;
                case "Swamp":
                    _btn = mapButtons.FirstOrDefault(b => b.name == "SwampButton");
                    _btn.style.display = DisplayStyle.Flex;
                    break;
                case "Volcanic":
                    _btn = mapButtons.FirstOrDefault(b => b.name == "VolcanicButton");
                    _btn.style.display = DisplayStyle.Flex;
                    break;
            }
        }
    }
   
    public void CloseFocusedElement()
    {
        var _fe = FocusedElements.Pop();
        Debug.Log(_fe.name + "Closed!");
        _fe.style.display = DisplayStyle.None;
    }

    public void OpenMenu()
    {
        var _menu = root.Q<VisualElement>("PauseMenuLayer");
        _menu.style.display = DisplayStyle.Flex;
        FocusedElements.Push(_menu);
    }
    private void DungeonSelected(DungeonInstance dungeon)
    {
        Debug.Log("Dungeon Selected");
        currentDungeon = dungeon;
        var _detailsBoard = root.Q<VisualElement>("DetailsLayer");
        _detailsBoard.style.display = DisplayStyle.Flex;
        var _container = _detailsBoard.Q<VisualElement>("DungeonLayer");
        _container.style.display = DisplayStyle.Flex;
        _detailsBoard.Q<VisualElement>("CharacterLayer").style.display = DisplayStyle.None;
        
        detailPanel.PopulateDungeonDetails(dungeon, _container, GameStateQueries.GetParty(gameController.State).GetSafetyRating(dungeon));
        _detailsBoard.Q<Button>("SendPartyButton").SetEnabled(GameStateQueries.GetUnassignedParties(gameController.State).Count > 0);
        if (FocusedElements.Count == 0 || FocusedElements.Peek() != _detailsBoard)
        {
            FocusedElements.Push(_detailsBoard);
        }
    }

    private void PartySelected(Party party)
    {
        Debug.Log("Party Selected");
        currentParty = party;
        var _detailsBoard = root.Q<VisualElement>("DetailsLayer");
        _detailsBoard.style.display = DisplayStyle.Flex;
        var _container = _detailsBoard.Q<VisualElement>("CharacterLayer");
        _container.style.display = DisplayStyle.Flex;
        _detailsBoard.Q<VisualElement>("DungeonLayer").style.display = DisplayStyle.None;

        detailPanel.PopulatePartyList(_container.Q<VisualElement>("PartyDetails"), party);
        if (FocusedElements.Count == 0 || FocusedElements.Peek() != _detailsBoard)
        {
            FocusedElements.Push(_detailsBoard);
        }
    }
        private void CharacterSelected(Character character)
    {
        Debug.Log("Character Selected");
        currentCharacter = character;
        var _detailsBoard = root.Q<VisualElement>("DetailsLayer");
        _detailsBoard.style.display = DisplayStyle.Flex;
        var _container = _detailsBoard.Q<VisualElement>("CharacterDetails");
        _container.style.display = DisplayStyle.Flex;

        detailPanel.PopulateCharacterDetails(character, _container, gameController.State.CurrentGold);
        if (FocusedElements.Count == 0 || FocusedElements.Peek() != _container)
        {
            FocusedElements.Push(_container);
        }  
        if(currentCharacter.Rank == CharacterRank.S)
        {
            _detailsBoard.Q<Button>("RankUpButton").SetEnabled(false);
        }
        else
        {
            _detailsBoard.Q<Button>("RankUpButton").SetEnabled(true);
        }
    }

    public void EndOfWeek(List<MissionResult> missions)
    {
        UpdateHeaderInfo();
        if (missions.Count > 0)
        {
            currentMissionList = new List<MissionResult>(missions);
            missionReportIndex = 0;
            DisplayMainInfoPanel("MissionLogLayer");
        }
    }
    public void UpdateHeaderInfo()
    {
        VisualElement _header = root.Q<VisualElement>("InfoHeader");

        _header.Q<Button>("WeekButton").text = "Week# " + gameController.State.WeekCount.ToString();
        _header.Q<Label>("GoldNumber").text = gameController.State.CurrentGold.ToString();
        _header.Q<Button>("GuildRankButton").text = gameController.State.CurrentGuildRank.ToString();

    } 

    private void DisplayMainInfoPanel(string view)
    {
        Debug.Log($"Main Info Panel Opened! view:{view}");

        var _mainInfoPanel = root.Q<VisualElement>("MainInfoPanel");
        _mainInfoPanel.style.display = DisplayStyle.Flex;
        if (FocusedElements.Count == 0 || FocusedElements.Peek() != _mainInfoPanel)
        {
            FocusedElements.Push(_mainInfoPanel);
        }
        HideAllInfoPanelLayers(_mainInfoPanel);

        switch(view)
        {
            case "MissionLogLayer":
                mainInfoPanel.ShowMissionReport(_mainInfoPanel.Q<VisualElement>("MissionLogLayer"), currentMissionList[missionReportIndex]);
                root.Q<Button>("ReportBackButton").style.display = (currentMissionList.Count > 1) ? DisplayStyle.Flex : DisplayStyle.None;
                root.Q<Button>("ReportForwardButton").style.display = (currentMissionList.Count > 1) ? DisplayStyle.Flex : DisplayStyle.None;
                onConfirm = null;
                break;
            case "GuildRankLayer":
                GuildRankEvaluation _guildEval = GameStateQueries.GetGuildRankEvaluation(gameController.State);
                mainInfoPanel.ShowGuildRankInfo(_mainInfoPanel.Q<VisualElement>("GuildRankLayer"), _guildEval);
            
               _mainInfoPanel.Q<Button>("MainInfoConfirmButton").SetEnabled(_guildEval.CanPromote);
                
                onConfirm = () => gameController.RankUp(_guildEval);
                break;
            case "GoldLedgerLayer":
                mainInfoPanel.ShowLedger(_mainInfoPanel.Q<VisualElement>("GoldLedgerLayer"));
                onConfirm = null;
                break;
            case "CharacterRankLayer":
                CharacterRankEvaluation _characterEval = GameStateQueries.GetCharacterRankEvaluation(currentCharacter, gameController.State.CurrentGold);
                mainInfoPanel.ShowCharacterRankInfo(_mainInfoPanel.Q<VisualElement>("CharacterRankLayer"),currentCharacter, _characterEval);
                
                _mainInfoPanel.Q<Button>("MainInfoConfirmButton").SetEnabled(_characterEval.CanPromote);
                
                onConfirm = () => gameController.RankUp(_characterEval, currentCharacter);
                break;
        }
    }
    private void HideAllInfoPanelLayers(VisualElement container)
    {
        container.Q<VisualElement>("MissionLogLayer").style.display = DisplayStyle.None;
        container.Q<VisualElement>("GuildRankLayer").style.display = DisplayStyle.None;
        container.Q<VisualElement>("GoldLedgerLayer").style.display = DisplayStyle.None;
        container.Q<VisualElement>("CharacterRankLayer").style.display = DisplayStyle.None;
    }

    public void GameOver()
    {
        mainMenu.style.display = DisplayStyle.None;
        mapScreen.style.display = DisplayStyle.None;
        pauseMenu.style.display = DisplayStyle.None;
        EndGameScreen.style.display = DisplayStyle.Flex;
    }

    public void ShowDialogBox(string text, System.Action confirm)
    {
        VisualElement _dialogBox = mapScreen.Q<VisualElement>("DialogBox");
        _dialogBox.style.display = DisplayStyle.Flex;

        onConfirm = confirm;

        FocusedElements.Push(_dialogBox);
        _dialogBox.Q<Label>("DialogText").text = text;
    }

    public void ShowNoPartyAssignedWarning()
    {
        ShowDialogBox(
            "The party has not been assigned a mission! Are you sure you want to proceed?",
            confirm: () => gameController.EndWeek()
        );
    }

//remove after adding In Town Actions

    public void ShowAllPartiesAssigned()
    {
        ShowDialogBox(
            "All Parties have been assigned Missions. Would you like to end the week now?",
            confirm: () => gameController.EndWeek()
        );
    }

}



