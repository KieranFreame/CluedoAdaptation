using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;

public class TurnManager : MonoBehaviour
{
    public static TurnManager inst;

    private void Awake()
    {
        inst = this;
    }

    private void Start()
    {
        Players.AddRange(FindObjectsOfType<Player>());
        StartTurn();
    }

    public List<Player> Players { get; private set; } = new(6); //Six Suspects in Total so only allow 6 players
    public int CurrPlayerIndex { get; private set; } = 0;
    public bool GameActive { get; set; } = true;
    private void ChangeTurnPlayer()
    {
        if (Players.FindAll(x => !x.Eliminated).Count() == 0)
            return;

        CurrPlayerIndex++;

        if (CurrPlayerIndex >= Players.Count)
            CurrPlayerIndex = 0;

        StartTurn();
    }

    private void StartTurn()
    {
        if (!GetCurrentPlayer().Eliminated)
        {
            TextLog.inst.LogText(GetCurrentPlayer().name + "'s Turn");
            StartCoroutine(MovePlayer());
        }
        else
            ChangeTurnPlayer();
    }

    private IEnumerator MovePlayer()
    {
        yield return StartCoroutine(GetCurrentPlayer().Move());

        if (GetCurrentPlayer().CurrRoom == Room.EvidenceRoom)
            StartCoroutine(PlayerAccuses());
        else
            StartCoroutine(PlayerSuggests());            
    }

    private IEnumerator PlayerSuggests()
    {
        Solution suggestion = new();
        yield return StartCoroutine(GetCurrentPlayer().Suggest(sugg => { suggestion = sugg; }));

        TextLog.inst.LogText($"{GetCurrentPlayer().name}'s Suggestion: {Suspects.GetSuspectName(suggestion.Suspect)} with the {suggestion.Weapon} in the {Rooms.GetRoomName(suggestion.Room)}");

        StartCoroutine(PlayersDisprove(suggestion));
    }
    
    private IEnumerator PlayerAccuses()
    {
        Solution accusation = new();
        yield return StartCoroutine(GetCurrentPlayer().Accuse(sugg => { accusation = sugg; }));

        if (SolutionManager.CheckSolution(accusation))
        {
            TextLog.inst.LogText(GetCurrentPlayer().name + " has cracked the case!"); //end the game;
            Time.timeScale = 0;
        }
        else
        {
            TextLog.inst.LogText(GetCurrentPlayer().name + " is incorrect, and has been eliminated");
            GetCurrentPlayer().Eliminated = true;
            ChangeTurnPlayer();
        }
    }

    private IEnumerator PlayersDisprove(Solution suggestion)
    {
        bool suggDisproved = false;

        for (int i = 0; i < Players.Count; i++)
        {
            if (i != CurrPlayerIndex)
            {
                yield return StartCoroutine(Players[i].Disprove(suggestion, evidence =>
                {
                    if (evidence != Evidence.None)
                    {
                        suggDisproved = true;
                        GetCurrentPlayer().ReceiveEvidence(evidence);
                        TextLog.inst.LogText(GetCurrentPlayer().name + "'s suggestion has been disproven by " + Players[i].name);
                    }
                }));

                if (suggDisproved) break;
            }
        }

        if (!suggDisproved)
            TextLog.inst.LogText(GetCurrentPlayer().name + "'s suggestion could not be disproved!");

        ChangeTurnPlayer();
    }

    public static Player GetCurrentPlayer()
    {
        return inst.Players[inst.CurrPlayerIndex];
    }
}
