using System.Collections.Generic;
using UnityEngine;

public class PartyGenerator
{
    public Party GenerateParty(CharacterGenerator generator, int partySize, int currentID, int partyID)
    {
        Debug.Log("======= Party Creation ========");
        Party party = new();
        for (int i = 0; i < partySize; i++)
        {
            party.PartyMembers.Add(generator.GenerateCharacter(currentID + i));
        }

        party.ID = partyID;
        party.PartyName = "test";
        party.CurrentMission = null;
        party.AssignedAction = PartyAction.Unassigned;
        party.UpdateParty();
        return party;
    }
}