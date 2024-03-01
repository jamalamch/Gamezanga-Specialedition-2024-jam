using System;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson; //need this to access the RigidbodyFirstPersonController
using TMPro;

public class InteractionInstigator : MonoBehaviour{

    public PanelDialog DialogueBox;
    public GameObject Player;
    private List<Interactable> m_NearbyInteractables = new List<Interactable>();

    public bool HasNearbyInteractables(){
        return m_NearbyInteractables.Count != 0;
    }
    private void Start(){
        DialogueBox.Close();
    }

    private void Update(){
        if (HasNearbyInteractables() && Input.GetButtonDown("Submit")){
            DialogueBox.Open();
            Player.GetComponent<InteractionInstigator>().enabled = false;
            Player.GetComponent<RigidbodyFirstPersonController>().enabled = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Player.GetComponent<NodeParser>().NextNode("exit");  //makes sure that StartNode is not activated automatically    
        }
    }

    private void OnTriggerEnter(Collider other){
        Interactable interactable = other.GetComponent<Interactable>();
        if (interactable != null){
            m_NearbyInteractables.Add(interactable);
        }
    }

    private void OnTriggerExit(Collider other){
        Interactable interactable = other.GetComponent<Interactable>();
        if (interactable != null){
            m_NearbyInteractables.Remove(interactable);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}