using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisproveHandler : MonoBehaviour
{
    private static DisproveHandler inst;

    private void Awake()
    {
        inst = this;
    }

    [Header("UI Elements")]
    [SerializeField] private GameObject disproveUI;
    [SerializeField] private TMP_Text suggestionText;
    [SerializeField] private Button suspectBtn;
    [SerializeField] private Button weaponBtn;
    [SerializeField] private Button roomBtn;

    private Solution _suggestion;
    private Evidence disprovingEvidence;
    private bool _suggestDisproved;

    public static IEnumerator SelectEvidence(Solution evidence, Solution suggestion, System.Action<Evidence> callback)
    {
        inst._suggestDisproved = false;
        inst._suggestion = suggestion;
        SetupUI(evidence);

        while (!inst._suggestDisproved)
            yield return null;

        callback(inst.disprovingEvidence);
        inst.disproveUI.SetActive(false);
    }

    public void ChooseSuspect()
    {
        disprovingEvidence = Suspects.GetEvidence(_suggestion.Suspect);
    }

    public void ChooseWeapon() 
    { 
        disprovingEvidence = Weapons.GetEvidence(_suggestion.Weapon); 
    }

    public void ChooseRoom()
    {
        disprovingEvidence = Rooms.GetEvidence(_suggestion.Room);
    }

    public void Confirm()
    {
        _suggestDisproved = true;
    }

    private static void SetupUI(Solution evidence)
    {
        inst.disproveUI.SetActive(true);

        inst.suggestionText.text = TurnManager.GetCurrentPlayer().name + "'s Suggestion: " + Suspects.GetSuspectName(inst._suggestion.Suspect) + " in the " + Rooms.GetRoomName(inst._suggestion.Room) + " with the " + Weapons.GetWeaponName(inst._suggestion.Weapon) + ".";

        if (evidence.Suspect != Suspect.None)
        {
            inst.suspectBtn.gameObject.SetActive(true);
            inst.suspectBtn.GetComponentInChildren<TMP_Text>().text = Suspects.GetSuspectName(evidence.Suspect);
        }
        else inst.suspectBtn.gameObject.SetActive(false);

        if (evidence.Weapon != Weapon.None)
        {
            inst.weaponBtn.gameObject.SetActive(true);
            inst.weaponBtn.GetComponentInChildren<TMP_Text>().text = evidence.Weapon.ToString();
        }
        else inst.weaponBtn.gameObject.SetActive(false);

        if (evidence.Room != Room.None)
        {
            inst.roomBtn.gameObject.SetActive(true);
            inst.roomBtn.GetComponentInChildren<TMP_Text>().text = Rooms.GetRoomName(evidence.Room);
        }
        else inst.roomBtn.gameObject.SetActive(false);
    }
}
