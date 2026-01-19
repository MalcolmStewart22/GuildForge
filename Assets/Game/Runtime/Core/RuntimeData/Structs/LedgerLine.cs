using System;
using UnityEngine;


public class LedgerLine
{
    public int Day;
    public int Gold;
    public string Reason;
    public int GoldDelta;

    public LedgerLine (int day, int gold, int delta, string reason)
    {
        Day = day;
        Gold = gold;
        Reason = reason;
        GoldDelta = delta;
    }
}