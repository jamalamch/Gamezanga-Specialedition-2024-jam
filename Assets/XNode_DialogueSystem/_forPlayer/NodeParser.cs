using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XNode; 
using TMPro;
using System;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.Events;
using System.Linq;
using System.Reflection;


public class NodeParser : MonoBehaviour
{
    public PanelDialog panelDialog;

    public DialogueGraph[] graph; 

    public int g;

    public GameObject Player;

    private string answer;

    private ChoiceDialogueNode activeSegment;
    Coroutine _parser;
    
    //public Image speakerImage;

    void Start(){ 
        try{
            foreach (BaseNode b in graph[g].nodes){  
                if (b.GetString() == "Start"){ //"b" is a reference to whatever node it's found next. It's an enumerator variable 
                    graph[g].current = b; //Make this node the starting point. The [g] sets what graph to use in the array OnTriggerEnter
                    break;      
                }    
            }
        }
        catch(NullReferenceException){
            Debug.LogError("ERROR: DialogueGraphs are not there");
        }
        _parser = StartCoroutine(ParseNode()); 
    
    }
    void Update(){
        //print("Graph Number:" + g);
    }
    
    public void AnswerClicked(int clickedIndex){ //Function when the choices button is pressed 
        panelDialog.ButtonContainerActive(false);
        BaseNode b = graph[g].current; 
        var port = activeSegment.GetPort("Answers " + clickedIndex);
        
        if (port.IsConnected){
            graph[g].current = port.Connection.node as BaseNode; 
            _parser = StartCoroutine(ParseNode());    
        }
        else{
            Player.GetComponent<InteractionInstigator>().enabled = true;
            Player.GetComponent<RigidbodyFirstPersonController>().enabled = true;
            panelDialog.Close();
            NextNode("input"); 
            Debug.LogError("ERROR: ChoiceDialogue port is not connected");
            //NextNode("exit"); 
         
        }       
    }
    
    private void UpdateDialogue(ChoiceDialogueNode newSegment){
        activeSegment = newSegment;
        panelDialog.SetDialog(newSegment.speakerName, newSegment.DialogueText);
        int answerIndex = 0;

        panelDialog.CleareButons();

        foreach (var answer in newSegment.Answers){
            var index = answerIndex;
            panelDialog.AddAnswer(answer, () =>  AnswerClicked(index));
            answerIndex++;
        }
    }
         
    IEnumerator ParseNode(){ //Node logic goes here
        BaseNode b = graph[g].current; 
        string data = b.GetString(); 
        string[] dataParts = data.Split('/'); //array of strings 


        panelDialog.ClearDialog();
        panelDialog.CleareButons();


        if (dataParts[0] == "Start"){
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            panelDialog.ClearDialog();
            panelDialog.CleareButons();
        }

        if (dataParts[0] == "ChoiceDialogueNode"){
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            panelDialog.ButtonContainerActive(true);

            UpdateDialogue(b as ChoiceDialogueNode); //Instantiates the buttons          
        }
        if (dataParts[0] == "DialogueNode"){
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            panelDialog.SetDialog(dataParts[1], dataParts[2]);

            yield return new WaitUntil(() => (panelDialog.isOpen)); 
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0)); //waits for left mouse click input then goes to next node
            yield return new WaitUntil(() => Input.GetMouseButtonUp(0));
            NextNode("exit");
        }
        if (dataParts[0] == "CloseDialogue_ExitNode"){
            Player.GetComponent<InteractionInstigator>().enabled = true;
            Player.GetComponent<RigidbodyFirstPersonController>().enabled = true;
            panelDialog.Close();
            graph[g].Start(); //loops back to the start node
            panelDialog.ClearDialog();
            panelDialog.CleareButons();
        }

        if (dataParts[0] == "CloseDialogue_ExitNode_NoLoop_toStart"){ //the name is self explanatory 
            Player.GetComponent<InteractionInstigator>().enabled = true;
            Player.GetComponent<RigidbodyFirstPersonController>().enabled = true;
            panelDialog.Close();
            panelDialog.ClearDialog();
            panelDialog.CleareButons();
        }

        if (dataParts[0] == "CustomNode"){ //rename here
            //Type whatever logic you want here. Right now, it's empty. 

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            panelDialog.Open();
            NextNode("exit");
        }
    }

    public void NextNode(string fieldName){
        panelDialog.ClearDialog();
        panelDialog.CleareButons();
        if (_parser != null){
            StopCoroutine(_parser); 
            _parser = null;
        }
        try{
            foreach (NodePort p in graph[g].current.Ports){   
                try{
                    if (p.fieldName == fieldName){
                        graph[g].current = p.Connection.node as BaseNode;
                        break;     
                    }
                } 
                catch (NullReferenceException){
                    Debug.LogError("ERROR: Port is not connected");
                }            
            }
        }
        catch (NullReferenceException){
            Debug.LogError("ERROR: One of the elements on the Graph array at NodeParser is empty. Or, the Dialogue Graph is empty");
        }
            
        _parser = StartCoroutine(ParseNode());
        
    }
}
