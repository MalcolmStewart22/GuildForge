using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private GameController master;
    [SerializeField]
    private DungeonListView dungeonListView;
    [SerializeField]
    private GuildListView guildListView;
    [SerializeField]
    private DetailPanel detailPanel;
    [SerializeField]
    private AfterActionReport afterActionReport;

    private VisualElement root;
    private VisualElement mainMenu;
    private VisualElement mapScreen;
    private VisualElement pauseMenu;
    private VisualElement EndGameScreen;
    private List<Button> mapButtons = new List<Button>();
    public Stack<VisualElement> FocusedElements = new();
    private DungeonInstance currentDungeon;
    private Character currentCharacter;
    private System.Action onConfirm;
    
    public void Setup()
    {
        currentCharacter = null;
        currentDungeon = null;
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
        var _menuButtons = root.Query<Button>(className:"MenuButton").ToList();
        mapButtons = root.Query<Button>(className:"MapButton").ToList();
        var _exitButtons = root.Query<Button>(className:"ExitButtons").ToList();
        var _characterButtons = root.Query<Button>(className:"CharacterButton").ToList();
    

        foreach (var button in _menuButtons)
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
                     btn.clicked += () => master.Restart();
                    break;
                case "SendPartyButton":
                //will need to implement party selection screens later
                    Debug.Log("Send Party!");
                    btn.clicked += () => master.DungeonStart(currentDungeon);
                    break;
                case "ConfirmButton":
                     btn.clicked += () => CloseFocusedElement();
                    break;
                default:
                    btn.clicked += () => OnMenuClicked(btn.name);
                    break;
            } 
        }
        foreach (var button in mapButtons)
        {
            var btn = button;
            btn.clicked += () => OnMapClicked(btn.name);
        }
        foreach (var button in _exitButtons)
        {
            var btn = button;
            btn.clicked += () => OnExitClicked(btn.name);
        }
        foreach (var button in _characterButtons)
        {
            var btn = button;
            btn.clicked += () => OnCharacterButtonClicked(btn.name);
        }
        var _dialogConfirm = root.Q<Button>("DialogConfirm");
        var _dialogCancel = root.Q<Button>("DialogCancel");
        _dialogConfirm.clicked += OnConfirmClicked;
        _dialogCancel.clicked += () =>  CloseFocusedElement();
    }
    private void SetupUIEvents()
    {
        dungeonListView.OnDungeonSelected += DungeonSelected;
        guildListView.OnCharacterSelected += CharacterSelected;
    }
    
    public void LoadMapScreen(int gold, int day)
    {
        mainMenu.style.display = DisplayStyle.None;
        mapScreen.style.display = DisplayStyle.Flex;
        EndGameScreen.style.display = DisplayStyle.None;

        VisualElement _header = root.Q<VisualElement>("InfoHeader");

        Label _day = _header.Q<Label>("DayNumber");
        _day.text = day.ToString();

        Label _gold = _header.Q<Label>("GoldNumber");
        _gold.text = gold.ToString();
    }
    
    #region Buttons
    private void OnMenuClicked(string name)
    {
        Debug.Log(name + "clicked!");
        switch(name)
        {
            case "PlayButton":
                master.StartGame();
                break;
            case "OptionButton":
                break;
            case "QuitButton":
                master.QuitGame();
                break;
        }
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
            List<Character> roster = GameStateQueries.GetRoster(master.State);
            guildListView.ShowRoster(roster);
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
            
            List<DungeonInstance> list = GameStateQueries.GetFilteredDungeonList(name, master.State);
            dungeonListView.ShowDungeon(list);
        }
    } 
    private void OnNextButtonClicked()
    {
        Debug.Log("====== NEXT DAY =======");
        master.ReadyForNextDay();
    }
    private void OnConfirmClicked()
    {
        CloseFocusedElement();
        onConfirm?.Invoke();

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
                root.Q<VisualElement>("DetailsBoard").style.display = DisplayStyle.None;
                FocusedElements.Clear();
                break;
            case "DetailsExitButton":
                root.Q<VisualElement>("DetailsBoard").style.display = DisplayStyle.None;
                break;
        }
    }
    private void OnCharacterButtonClicked(string button)
    {
        switch(button)
        {
            case "HealButton":
                currentCharacter.MagicHeal();
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
        var _detailsBoard = root.Q<VisualElement>("DetailsBoard");
        _detailsBoard.style.display = DisplayStyle.Flex;
        var _container = _detailsBoard.Q<VisualElement>("DungeonDetails");
        _container.style.display = DisplayStyle.Flex;
        _detailsBoard.Q<VisualElement>("CharacterDetails").style.display = DisplayStyle.None;

        //will change when adding party selection screen
        GameStateQueries.GetParty(master.State).PrepareParty();
        
        detailPanel.PopulateDungeonDetails(dungeon, _container, GameStateQueries.GetParty(master.State).GetSafetyRating(dungeon));
        if (FocusedElements.Count == 0 || FocusedElements.Peek() != _detailsBoard)
        {
            FocusedElements.Push(_detailsBoard);
        }
    }

    private void CharacterSelected(Character character)
    {
        Debug.Log("Character Selected");
        currentCharacter = character;
        var _detailsBoard = root.Q<VisualElement>("DetailsBoard");
        _detailsBoard.style.display = DisplayStyle.Flex;
        var _container = _detailsBoard.Q<VisualElement>("CharacterDetails");
        _container.style.display = DisplayStyle.Flex;
        _detailsBoard.Q<VisualElement>("DungeonDetails").style.display = DisplayStyle.None;

        detailPanel.PopulateCharacterDetails(character, _container, GameStateQueries.GetCurrentGold(master.State));
        if (FocusedElements.Count == 0 || FocusedElements.Peek() != _detailsBoard)
        {
            FocusedElements.Push(_detailsBoard);
        }   
    }

    //will need to pass lists of missions in the future when multiple parties is implemented.
    public void UpdateForEndOfDay(int gold, int day, MissionResult mission)
    {
        VisualElement _header = root.Q<VisualElement>("InfoHeader");

        Label _day = _header.Q<Label>("DayNumber");
        _day.text = day.ToString();

        Label _gold = _header.Q<Label>("GoldNumber");
        _gold.text = gold.ToString();

        DisplayAfterActionReport(mission);
    }

    public void DisplayAfterActionReport(MissionResult mission)
    {
        var _actionReport = root.Q<VisualElement>("AfterAction");
        _actionReport.style.display = DisplayStyle.Flex;

        afterActionReport.ShowReport(_actionReport, mission);
        FocusedElements.Push(_actionReport);
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
            confirm: () => master.EndDay()
        );
    }

//remove after adding In Town Actions

    public void ShowAllPartiesAssigned()
    {
        ShowDialogBox(
            "All Parties have been assigned Missions. Would you like to end the day now?",
            confirm: () => master.EndDay()
        );
    }
}
