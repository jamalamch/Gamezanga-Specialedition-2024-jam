using DG.Tweening;
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
    public TextMeshProUGUI dialogue, info;

    public GameObject buttonPrefab;
    public GameObject ButtonContainer;

    public GameObject InfoContainer, QuisContainer;

    public Image imageIcon, imageCharcter;

    [SerializeField] Sprite[] _iconChacter;
    Transform buttonParent => ButtonContainer.transform;

    protected override float _BagroundImageFade => 1;

    private void Awake()
    {
        Init();
    }

    public void SetDialog(SpeakerName speakerTxt, string dialogueTxt, bool infoText = true)
    {
        SetCharterImage(_iconChacter[(int)speakerTxt]);
        SetDialog(DialogueNode.GetSpeakerName(speakerTxt), dialogueTxt, infoText);
    }

    public void SetDialog(string speakerTxt, string dialogueTxt,bool infoText = true)
    {
        InfoContainer.SetActive(infoText);
        QuisContainer.SetActive(!infoText);

        speaker.text = speakerTxt;
        info.text = dialogue.text = new string(dialogueTxt.Reverse().ToArray());

        if (speaker.text == "")
        {
            Debug.LogError("ERROR: Speaker text for Dialog is empty");
        }
        if (dialogue.text == "")
        {
            Debug.LogError("ERROR: Dialogue text for Dialog is empty");
        }
    }

    public void SetImage(Sprite image)
    {
        if (image == null && imageIcon.sprite)
        {
            imageIcon.gameObject.SetActive(false);
            ((RectTransform)buttonParent).DOSizeDelta(new Vector2(600, 200), 0);
        }
        else if(imageIcon.sprite == null && image != null)
        {
            imageIcon.gameObject.SetActive(true);
            imageIcon.sprite = image;
            ((RectTransform)buttonParent).DOSizeDelta(new Vector2(417, 200), 0);
        }
    }

    public void SetCharterImage(Sprite icon)
    {
        imageCharcter.sprite = icon;
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
        InfoContainer.SetActive(false);
        QuisContainer.SetActive(true);
        speaker.text = info.text = dialogue.text = "";
    }
}
