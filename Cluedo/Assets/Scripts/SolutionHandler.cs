using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.ComponentModel;
using UnityEngine.UI;
using Unity.VisualScripting;

public class SolutionHandler : MonoBehaviour
{
    private static SolutionHandler inst;

    private void Awake()
    {
        inst = this;
    }

    [SerializeField] private GameObject solutionUI;
    [SerializeField] private TMP_Text solutionText;

    private readonly Solution suggestionToReturn = new();
    private bool isAccusation;
    private bool solutionCompleted = false;

    private void Update()
    {
        if (solutionUI.activeSelf)
            if (isAccusation)
                solutionText.text = "It was " + Suspects.GetSuspectName(suggestionToReturn.Suspect) + " in the " + Rooms.GetRoomName(suggestionToReturn.Room) + " with the " + Weapons.GetWeaponName(suggestionToReturn.Weapon) + ".";
            else
                solutionText.text = "I think it was " + Suspects.GetSuspectName(suggestionToReturn.Suspect) + " in the " + Rooms.GetRoomName(suggestionToReturn.Room) + " with the " + Weapons.GetWeaponName(suggestionToReturn.Weapon) + ".";
    }

    public static IEnumerator CreateSolution(Player p, System.Action<Solution> callback, bool accusation = false)
    {
        inst.solutionCompleted = false;
        inst.solutionUI.SetActive(true);
        inst.isAccusation = accusation;

        if (!inst.isAccusation)
            inst.suggestionToReturn.Room = p.CurrRoom;
        else
        {
            foreach (Transform c in inst.solutionUI.transform.Find("RoomPanel"))
            {
                c.GetComponent<Button>().interactable = true;
            }
        }

        while (!inst.solutionCompleted)
            yield return null;

        callback(inst.suggestionToReturn);
    }

    public void SelectSuspect(int suspect) => suggestionToReturn.Suspect = (Suspect)suspect;
    public void SelectWeapon(int weapon) => suggestionToReturn.Weapon = (Weapon)weapon;
    public void SelectRoom(int room) => suggestionToReturn.Room = (Room)room;

    public void SubmitSolution()
    {
        solutionCompleted = true;

        foreach (Transform c in inst.solutionUI.transform.Find("RoomPanel"))
        {
            c.GetComponent<Button>().interactable = false;
        }

        solutionUI.SetActive(false);
    }
}
