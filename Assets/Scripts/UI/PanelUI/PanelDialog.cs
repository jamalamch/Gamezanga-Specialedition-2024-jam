using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UIParty;
using UnityEngine;
using UnityEngine.UI;

public class PanelDialog : PanelUI
{
    public TextMeshProUGUI speaker;
    public TextMeshProUGUI dialogue;

    public GameObject buttonPrefab;
    public GameObject ButtonContainer;

    Transform buttonParent => ButtonContainer.transform;

    protected override float _BagroundImageFade => 1;

    private void Awake()
    {
        Init();
    }

    public void SetDialog(SpeakerName speakerTxt, string dialogueTxt)
    {
        SetDialog(DialogueNode.GetSpeakerName(speakerTxt), dialogueTxt);
    }

    public void SetDialog(string speakerTxt, string dialogueTxt)
    {
        speaker.text = speakerTxt;
        dialogue.text = new string(dialogueTxt.Reverse().ToArray());

        if (speaker.text == "")
        {
            Debug.LogError("ERROR: Speaker text for Dialog is empty");
        }
        if (dialogue.text == "")
        {
            Debug.LogError("ERROR: Dialogue text for Dialog is empty");
        }
    }

    public void AddAnswer(string answer, Action action)
    {
        var btn = Instantiate(buttonPrefab, buttonParent); //spawns the buttons 
        btn.GetComponentInChildren<TextMeshProUGUI>().text = new string(answer.Reverse().ToArray());
        btn.GetComponentInChildren<Button>().onClick.AddListener((() => action()));
    }

    internal void ButtonContainerActive(bool v)
    {
        ButtonContainer.SetActive(v);
    }

    internal void CleareButons()
    {
        foreach (Transform child in buttonParent)
        {
            Destroy(child.gameObject);
        }
    }

    internal void ClearDialog()
    {
        speaker.text = "";
        dialogue.text = "";
    }
}
