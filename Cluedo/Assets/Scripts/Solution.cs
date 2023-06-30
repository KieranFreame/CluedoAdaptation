using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Solution
{
    public Weapon Weapon { get; set; } = Weapon.None;
    public Suspect Suspect { get; set; } = Suspect.None;
    public Room Room { get; set; } = Room.None;

    public Solution() { }

    public Solution(Weapon w, Suspect s, Room r)
    {
        Weapon = w;
        Suspect = s;
        Room = r;
    }
}
