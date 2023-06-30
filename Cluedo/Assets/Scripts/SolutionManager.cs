using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolutionManager : MonoBehaviour
{
    private static SolutionManager inst;

    private void Awake()
    {
        if (inst == null)
            inst = this;
        else
            Destroy(this);
    }

    private Solution _solution;
    readonly List<Evidence> remainingEvidence = new();
    private readonly List<Evidence> _suspects = new() { Evidence.Scarlett, Evidence.Mustard, Evidence.Green, Evidence.Peacock, Evidence.Plum, Evidence.White };
    private readonly List<Evidence> _weapons = new() { Evidence.Poison, Evidence.Candlestick, Evidence.Dumbbell, Evidence.Bat, Evidence.Knife, Evidence.Pistol, Evidence.Trophy, Evidence.Rope, Evidence.Axe };
    private readonly List<Evidence> _rooms = new() { Evidence.Hall, Evidence.Patio, Evidence.Kitchen, Evidence.Spa, Evidence.Theater, Evidence.LivingRoom, Evidence.Observatory, Evidence.DiningRoom, Evidence.GuestHouse };

    private void Start()
    {
        CreateSolution();
        AssignEvidence();
    }

    private void CreateSolution()
    {
        int index = Random.Range(0, _suspects.Count);
        Suspect murderer = Suspects.GetSuspect(_suspects[index]);
        _suspects.RemoveAt(index);

        index = Random.Range(0, _weapons.Count);
        Weapon murderWeapon = Weapons.GetWeapon(_weapons[index]);
        _weapons.RemoveAt(index);

        index = Random.Range(0, _rooms.Count);
        Room murderRoom = Rooms.GetRoom(_rooms[index]);
        _rooms.RemoveAt(index);

        _solution = new Solution(murderWeapon, murderer, murderRoom);
    }

    private void AssignEvidence()
    {
        remainingEvidence.AddRange(_suspects);
        remainingEvidence.AddRange(_weapons);
        remainingEvidence.AddRange(_rooms);

        ShuffleEvidence(remainingEvidence);

        //3 or 6 players = stop at 0, 4 or 5 players = stop at 1
        while (remainingEvidence.Count >= TurnManager.inst.Players.Count)
        {
            foreach (Player p in TurnManager.inst.Players)
            {
                p.evidence.Add(remainingEvidence[0]);

                if (p is Human)
                    p.ReceiveEvidence(remainingEvidence[0]);

                remainingEvidence.RemoveAt(0);
            }
        }
    }

    public static bool CheckSolution(Solution accusation)
    {
        return accusation.Suspect == inst._solution.Suspect && accusation.Weapon == inst._solution.Weapon && accusation.Room == inst._solution.Room;
    }

    private void ShuffleEvidence(List<Evidence> deck)
    {
        System.Random r = new();
        int n = deck.Count;

        while (n > 1)
        {
            int k = r.Next(n);
            n--;
            (deck[n], deck[k]) = (deck[k], deck[n]);
        }
    }
}
