using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogmanager : MonoBehaviour
{
    public TMP_Text text;
    public GameObject DialogSystem;
    void Start()
    {
       
    }

    string[] words;
    int number;
    public void ShowMessage(string Message)
    {
        GameState.isDialogueActive = true;
        number = 0;
        words = Message.Split(',');
        Time.timeScale = 0f;
        DialogSystem.SetActive(true);
        Skip();
    }

    public void Skip()
    {
        if (number < words.Length)
        {
            text.text = words[number];
            number += 1;
        }
        else
        {
            number = 0;
            GameState.isDialogueActive = false;
            Time.timeScale = 1f;
            DialogSystem.SetActive(false);
        }
        ChangeTextColor();
    }

    void ChangeTextColor()
    {
        string modifiedText = text.text.Replace("(", "<color=red>");
        text.text = modifiedText;
        modifiedText = text.text.Replace(")", "</color>");
        text.text = modifiedText;
    }
}