using UnityEngine;
using UnityEngine.UIElements;

public class RankEvaluationPanel : MonoBehaviour
{
    public void ShowRankEvaluation(VisualElement container, Character character, RankEvaluation rankEval)
    {
        Label _name = container.Q<Label>("RankUpName");
        _name.text = "Promote " + character.Name + "?";

        //Requirements
        //Level
        Label _reqlevel = container.Q<Label>("ReqLevel");
        _reqlevel.text = rankEval.RequiredLevel.ToString();
        Label _curlevel = container.Q<Label>("curLevel");
        _curlevel.text = character.Level.ToString();
        //Primary
        Label _primaryStat = container.Q<Label>("PrimaryStat");
        _primaryStat.text = rankEval.PrimaryStat.ToString();
        Label _reqPrimary = container.Q<Label>("ReqPrimaryStat");
        _reqPrimary.text = rankEval.PrimaryStatRequirement.ToString();
        Label _curPrimary = container.Q<Label>("CurPrimaryStat");
        _curPrimary.text = character.Base.GetStat(rankEval.PrimaryStat).ToString();
        //Secondary
        Label _secondaryStat = container.Q<Label>("SecondaryStat");
        _primaryStat.text = rankEval.SecondaryStat.ToString();
        Label _reqSecondary = container.Q<Label>("ReqSecondaryStat");
        _reqPrimary.text = rankEval.SecondaryStatRequirement.ToString();
        Label _curSecondary = container.Q<Label>("CurSecondaryStat");
        _curPrimary.text = character.Base.GetStat(rankEval.SecondaryStat).ToString();
        //Gold
        Label _gold = container.Q<Label>("CostToRankUp");
        _gold.text = rankEval.PromotionCost.ToString();

        //Effects
        //Rank
        Label _rank = container.Q<Label>("NewRank");
        _rank.text = rankEval.RequiredLevel.ToString();
        //Wage
        Label _wage = container.Q<Label>("NewWage");
        _wage.text = rankEval.RequiredLevel.ToString();
        //LevelCap
        Label _levelCap = container.Q<Label>("NewLevelCap");
        _levelCap.text = rankEval.RequiredLevel.ToString();


        //add in conditionals

        if(rankEval.MeetsRequiredLevel)
        {
            Toggle _levelToggle = container.Q<Toggle>("LevelToggle");
            _levelToggle.value = true;

            _reqlevel.style.color = Color.gray;
            _curlevel.style.color = Color.gray;
        }
        if(rankEval.MeetsRequiredPrimaryStat)
        {
            Toggle _primaryToggle = container.Q<Toggle>("PrimaryToggle");
            _primaryToggle.value = true;

            _primaryStat.style.color = Color.gray;
            _reqPrimary.style.color = Color.gray;
            _curPrimary.style.color = Color.gray;
        }
        if(rankEval.MeetsRequiredSecondaryStat)
        {
            Toggle _secondaryToggle = container.Q<Toggle>("SecondaryToggle");
            _secondaryToggle.value = true;

            _secondaryStat.style.color = Color.gray;
            _reqSecondary.style.color = Color.gray;
            _curSecondary.style.color = Color.gray;
        }
        if(rankEval.CanPromote)
        {
            container.Q<Button>("ConfirmRankUpButton").SetEnabled(true);
        }
        else
        {
             container.Q<Button>("ConfirmRankUpButton").SetEnabled(false);
        }
    }
}