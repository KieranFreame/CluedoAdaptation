using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextLogItem : MonoBehaviour
{
    public void OnEnable()
    {
        TextLog.OnNewText += SetText;
    }
    public void SetText(string text)
    {
        GetComponent<TMP_Text>().text = text;
        TextLog.OnNewText -= SetText;
    }
}
