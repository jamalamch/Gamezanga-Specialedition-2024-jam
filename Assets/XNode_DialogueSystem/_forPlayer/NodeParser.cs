using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XNode; 
using TMPro;
using System;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.Events;


public class NodeParser : MonoBehaviour
{

    public DialogueGraph[] graph; 
    public TextMeshProUGUI speaker;
    public TextMeshProUGUI dialogue; 
    public int g;

    public GameObject DialogueBox;
    public GameObject buttonPrefab;
    public GameObject ButtonContainer;
    public GameObject Player;

    public Transform buttonParent;
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
        ButtonContainer.SetActive(false);
        BaseNode b = graph[g].current; 
        var port = activeSegment.GetPort("Answers " + clickedIndex);
        
        if (port.IsConnected){
            graph[g].current = port.Connection.node as BaseNode; 
            _parser = StartCoroutine(ParseNode());    
        }
        else{
            Player.GetComponent<InteractionInstigator>().enabled = true;
            Player.GetComponent<RigidbodyFirstPersonController>().enabled = true;
            DialogueBox.SetActive(false);
            NextNode("input"); 
            Debug.LogError("ERROR: ChoiceDialogue port is not connected");
            //NextNode("exit"); 
         
        }       
    }
    
    private void UpdateDialogue(ChoiceDialogueNode newSegment){
        activeSegment = newSegment;
        dialogue.text = newSegment.DialogueText;
        speaker.text = newSegment.speakerName;
        int answerIndex = 0;
        foreach (Transform child in buttonParent){
            Destroy(child.gameObject);
        }

        foreach (var answer in newSegment.Answers){
            var btn = Instantiate(buttonPrefab, buttonParent); //spawns the buttons 
            btn.GetComponentInChildren<TextMeshProUGUI>().text = answer;

            var index = answerIndex;
            btn.GetComponentInChildren<Button>().onClick.AddListener((() => { AnswerClicked(index);}));
            answerIndex++;
        }
    }
         
    IEnumerator ParseNode(){ //Node logic goes here
        BaseNode b = graph[g].current; 
        string data = b.GetString(); 
        string[] dataParts = data.Split('/'); //array of strings 

        speaker.text =""; //Resets the text 
        dialogue.text = "";
    
        foreach (Transform child in buttonParent){ //Destroys the buttons when going to the next node 
            Destroy(child.gameObject);
        }

        if (dataParts[0] == "Start"){
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            speaker.text ="";
            dialogue.text = "";
            foreach (Transform child in buttonParent){
                Destroy(child.gameObject);
            }
        }

        if (dataParts[0] == "ChoiceDialogueNode"){
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            ButtonContainer.SetActive(true);
            speaker.text = dataParts[1];
            dialogue.text = dataParts[2];

            UpdateDialogue(b as ChoiceDialogueNode); //Instantiates the buttons 

            if(speaker.text == ""){
                Debug.LogError("ERROR: Speaker text for ChoiceDialogueNode is empty");
            }
            if(dialogue.text == ""){
                Debug.LogError("ERROR: Dialogue text for ChoiceDialogueNode is empty");
            }
         
        }
        if (dataParts[0] == "DialogueNode"){
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            speaker.text = dataParts[1];
            dialogue.text = dataParts[2];

            if(speaker.text == ""){
                Debug.LogError("ERROR: Speaker text for DialogueNode is empty");
            }
            if(dialogue.text == ""){
                Debug.LogError("ERROR: Dialogue text for DialogueNode is empty");
            }
            
            yield return new WaitUntil(() => (DialogueBox.activeSelf)); 
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0)); //waits for left mouse click input then goes to next node
            yield return new WaitUntil(() => Input.GetMouseButtonUp(0));
            NextNode("exit");

           
          

            
          

        }
        if (dataParts[0] == "CloseDialogue_ExitNode"){
            Player.GetComponent<InteractionInstigator>().enabled = true;
            Player.GetComponent<RigidbodyFirstPersonController>().enabled = true;
            DialogueBox.SetActive(false);
            graph[g].Start(); //loops back to the start node
            speaker.text ="";
            dialogue.text = "";
            foreach (Transform child in buttonParent){
                Destroy(child.gameObject);
            }
        }

        if (dataParts[0] == "CloseDialogue_ExitNode_NoLoop_toStart"){ //the name is self explanatory 
            Player.GetComponent<InteractionInstigator>().enabled = true;
            Player.GetComponent<RigidbodyFirstPersonController>().enabled = true;
            DialogueBox.SetActive(false);
            speaker.text ="";
            dialogue.text = "";
            foreach (Transform child in buttonParent){
                Destroy(child.gameObject);
            }
        }

        if (dataParts[0] == "CustomNode"){ //rename here
            //Type whatever logic you want here. Right now, it's empty. 

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            DialogueBox.SetActive(true);
            NextNode("exit");
        }
    }

    public void NextNode(string fieldName){
        speaker.text ="";
        dialogue.text = "";
        foreach (Transform child in buttonParent){
            Destroy(child.gameObject);
        }
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
