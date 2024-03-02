using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateAssetMenu]
public class DialogueGraph : NodeGraph { 

    public BaseNode start;
    public BaseNode current; //very similar to function declaration
    public BaseNode initNode;

    public void Start(){
        foreach(BaseNode node in nodes)
        {
            if (node is StartNode)
            {
                initNode = node;
                break;
            }
        }
        start = initNode; //loops back to the start node
        current = initNode;
    }

    internal void SetCurrentName(string fieldName)
    {
        foreach (var item in current.Ports)
        {
            try
            {
                if (item.fieldName == fieldName)
                {
                    current = item.Connection.node as BaseNode;
                    break;
                }
            }
            catch (NullReferenceException)
            {
                Debug.LogError("ERROR: Port is not connected");
            }
        }
    }
}