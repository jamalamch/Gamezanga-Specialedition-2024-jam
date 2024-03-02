using System;
using System.Collections;
using UIParty;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;


public class NodeParser : MonoBehaviour
{
    public PanelDialog panelDialog;

    public DialogueGraph[] graph;

    public int g;

    public GameObject Player;

    private ChoiceDialogueNode activeSegment;
    Coroutine _parser;

    //public Image speakerImage;
    int _answer;
    void Start()
    {
        try
        {
            foreach (var segment in graph)
            {
                segment.Start();
            }
        }
        catch (NullReferenceException)
        {
            Debug.LogError("ERROR: DialogueGraphs are not there");
        }
    }
    void Update()
    {
        //print("Graph Number:" + g);
    }

    public void AnswerClicked(int clickedIndex)
    { //Function when the choices button is pressed 
        panelDialog.ButtonContainerActive(false);
        BaseNode b = graph[g].current;
        var port = activeSegment.GetPort("Answers " + clickedIndex);

        if (_answer == clickedIndex)
            CurrentStart.instance.AddValue(1);

        if (port.IsConnected)
        {
            graph[g].current = port.Connection.node as BaseNode;
            _parser = StartCoroutine(ParseNode());
        }
        else
        {
            CLoseDilog();
            NextNode("input");
            Debug.LogError("ERROR: ChoiceDialogue port is not connected");
            //NextNode("exit"); 

        }
    }

    private void UpdateDialogue(ChoiceDialogueNode newSegment)
    {
        activeSegment = newSegment;
        panelDialog.SetDialog(newSegment.speakerName, newSegment.DialogueText, false);
        int answerIndex = 0;
        if (newSegment is ImageChoiceDialoueNode newSegmentImage)
        {
            panelDialog.SetImage(newSegmentImage.sprite);
        }
        else
            panelDialog.SetImage(null);

        _answer = newSegment.answersIndex;

        panelDialog.CleareButons();

        foreach (var answer in newSegment.Answers)
        {
            var index = answerIndex;
            panelDialog.AddAnswer(answer, () => AnswerClicked(index));
            answerIndex++;
        }
    }

    IEnumerator ParseNode()
    { //Node logic goes here
        BaseNode b = graph[g].current;
        string data = b.GetString();
        string[] dataParts = data.Split('/'); //array of strings 


        panelDialog.ClearDialog();
        panelDialog.CleareButons();


        if (dataParts[0] == "Start")
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            panelDialog.ClearDialog();
            panelDialog.CleareButons();
        }

        if (dataParts[0] == "ChoiceDialogueNode")
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            panelDialog.ButtonContainerActive(true);

            UpdateDialogue(b as ChoiceDialogueNode); //Instantiates the buttons          
        }
        if (dataParts[0] == "DialogueNode")
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            panelDialog.SetDialog(dataParts[1], dataParts[2]);

            yield return new WaitUntil(() => (panelDialog.isOpen));
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0)); //waits for left mouse click input then goes to next node
            yield return new WaitUntil(() => Input.GetMouseButtonUp(0));
            yield return new WaitForEndOfFrame();
            NextNode("exit");
        }
        if (dataParts[0] == "CloseDialogue_ExitNode")
        {
            CLoseDilog();
            graph[g].Start(); //loops back to the start node
        }

        if (dataParts[0] == "CloseDialogue_ExitNode_NoLoop_toStart")
        { //the name is self explanatory 
            CLoseDilog();
        }

        if (dataParts[0] == "CustomNode")
        { //rename here
            //Type whatever logic you want here. Right now, it's empty. 

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            panelDialog.Open();
            NextNode("exit");
        }
    }

    public void CLoseDilog()
    {
        Player.GetComponent<InteractionInstigator>().enabled = true;
        Player.GetComponent<RigidbodyFirstPersonController>().enabled = true;
        panelDialog.Close();
        panelDialog.ClearDialog();
        panelDialog.CleareButons();
    }

    public void ForceCLoseDilog()
    {
        //graph[g].Start();
        CLoseDilog();
    }

    public void NextNode(string fieldName)
    {
        panelDialog.ClearDialog();
        panelDialog.CleareButons();
        if (_parser != null)
        {
            StopCoroutine(_parser);
            _parser = null;
        }
        try
        {
            graph[g].SetCurrentName(fieldName);
        }
        catch (NullReferenceException)
        {
            Debug.LogError("ERROR: One of the elements on the Graph array at NodeParser is empty. Or, the Dialogue Graph is empty");
        }

        _parser = StartCoroutine(ParseNode());
    }

    internal void ChangeNodesGraph(int graphNumber)
    {
        g = graphNumber;
        if (_parser != null)
        {
            StopCoroutine(_parser);
            _parser = null;
        }
    }
}
