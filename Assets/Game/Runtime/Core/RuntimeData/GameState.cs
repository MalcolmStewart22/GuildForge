using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameState
{
    public int CurrentGold;
    public int DayCount = 1;
    public int MissionCount = 0;
    public GuildRank CurrentGuildRank;
    public List<Character> Characters = new();
    public List<Character> Recruited = new();
    public List<Party> Parties = new();
    public List<DungeonInstance> Dungeons = new();
    public List<MissionResult> MissionLog = new();
    public List<LedgerLine> Ledger = new();
}
