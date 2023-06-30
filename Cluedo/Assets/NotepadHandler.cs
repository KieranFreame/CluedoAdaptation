using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotepadHandler : MonoBehaviour
{
    public static NotepadHandler inst;

    [Header("UI Transforms")]
    [SerializeField] private Transform suspectsParent;
    [SerializeField] private Transform roomsParent;
    [SerializeField] private Transform weaponsParent;

    private readonly Dictionary<Evidence, Toggle> checkboxes = new();

    private void Awake()
    {
        if (inst == null)
            inst = this;
        else
            Destroy(this);

        PopulateDictionary();
    }

    private void PopulateDictionary()
    {
        int j = 0;

        for (int i = 1; i < 25; i++)
        {
            if (i <= 6)
            {
                checkboxes.Add((Evidence)i, suspectsParent.GetChild(j).GetComponent<Toggle>());
                j++;
                continue;
            }

            j = 7;

            if (i >= 7 && i <= 15)
            {
                j = i - j;
                checkboxes.Add((Evidence)i, roomsParent.GetChild(j).GetComponent<Toggle>());
                continue;
            }

            j = 16;

            if (i >= 16)
            {
                j = i - j;
                checkboxes.Add((Evidence)i, weaponsParent.GetChild(j).GetComponent<Toggle>());
                continue;
            }
        }
    }

    public void CheckOffEvidence(Evidence evidence)
    {
        checkboxes[evidence].isOn = true;
    }
}
