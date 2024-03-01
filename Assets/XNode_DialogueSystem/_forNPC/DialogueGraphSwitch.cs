using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueGraphSwitch : MonoBehaviour
{ 
    //Switches which graph to use in the NodeParser's Graph array 
    public int graphNumber;
    private void OnTriggerEnter(Collider col){
        if(col.gameObject.tag == "Player"){
            GameObject player = GameObject.FindWithTag("Player");
            NodeParser dialogueNum = player.GetComponent<NodeParser>();
            dialogueNum.g = graphNumber;
        }
    }

   
}
