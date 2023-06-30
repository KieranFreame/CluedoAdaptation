using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TextLog : MonoBehaviour
{
    public static TextLog inst;

    private void Awake()
    {
        if (inst == null)
            inst = this;
        else
            Destroy(this);
    }

    [SerializeField] private GameObject textTemplate;
    private readonly List<GameObject> textItems = new();

    public static event UnityAction<string> OnNewText;

    public void LogText(string text)
    {
        if (textItems.Count == 20)
        {
            GameObject tempItem = textItems[0];
            Destroy(tempItem);
            textItems.Remove(tempItem);
        }

        GameObject newText = Instantiate(textTemplate);
        newText.SetActive(true);

        OnNewText?.Invoke(text);
        newText.transform.SetParent(textTemplate.transform.parent, false);

        textItems.Add(newText);
    }
}
