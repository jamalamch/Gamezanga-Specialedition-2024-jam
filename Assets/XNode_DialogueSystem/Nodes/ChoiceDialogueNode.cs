using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[Serializable]
public struct Connection {}
public class ChoiceDialogueNode : BaseNode {
    [Input] public Connection input;
    //[Output] public Connection exit;
    [Output(dynamicPortList = true)] public List<string> Answers;
    public SpeakerName speakerName; 
    [TextArea] public string DialogueText;
    public int answersIndex;

    public override string GetString(){
		return "ChoiceDialogueNode/" + DialogueNode.GetSpeakerName(speakerName) + "/" + DialogueText + "/" + Answers[0];
	}

    public override object GetValue(NodePort port){
        return null;
    }


  
}
