using UnityEditorInternal;
using UnityEngine;
using XNode;
using XNodeEditor;

[CustomNodeEditor(typeof(ChoiceDialogueNode))]
public class ChoiceDialogueNodeEditor : NodeEditor{
    public override void OnBodyGUI()
        {
            
            serializedObject.Update();
            

            var segment = serializedObject.targetObject as ChoiceDialogueNode;

            NodeEditorGUILayout.PortField(segment.GetPort("input"));

             //These are a bit buggy when you enable this code and write in text. 
             //It selects all nodes when key press A. Pressing F will focus on the node and interrupts your writing.
            /* GUILayout.Label("Speaker Name");
            segment.speakerName = GUILayout.TextArea(segment.speakerName, new GUILayoutOption[]{GUILayout.MinHeight(20),});
            GUILayout.Label("Dialogue Text");
            segment.DialogueText = GUILayout.TextArea(segment.DialogueText, new GUILayoutOption[]{GUILayout.MinHeight(50),});  */
           
            GUILayout.Label("Speaker Name");
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("speakerName"), GUIContent.none);
            GUILayout.Label("Dialogue Text");
            GUILayout.Label("", new GUILayoutOption[]{GUILayout.Height(-20),}); //Sets Answers and Dialogue textfield position
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("DialogueText"), GUIContent.none);
          
            NodeEditorGUILayout.DynamicPortList(
                "Answers", // field name
                typeof(string), // field type
                serializedObject, // serializable object
                NodePort.IO.Input, // new port i/o
                Node.ConnectionType.Override, // new port connection type
                Node.TypeConstraint.None,
                OnCreateReorderableList); // onCreate override. This is where the magic 
           
            
            
            foreach (XNode.NodePort dynamicPort in target.DynamicPorts) {
                if (NodeEditorGUILayout.IsDynamicPortListPort(dynamicPort)) continue;
                NodeEditorGUILayout.PortField(dynamicPort);
            }

            serializedObject.ApplyModifiedProperties();
        }
        /* public override int GetWidth() {
            return 250;
        } */

        void OnCreateReorderableList(ReorderableList list)
        {
            list.elementHeightCallback = (int index) => {return 60;};
            
            // Override drawHeaderCallback to display node's name instead
            list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                var segment = serializedObject.targetObject as ChoiceDialogueNode;

                NodePort port = segment.GetPort( "Answers " + index);

                segment.Answers[index] = GUI.TextArea(rect, segment.Answers[index]);
                

                if (port != null) {
                    Vector2 pos = rect.position + (port.IsOutput?new Vector2(rect.width + 6, 0) : new Vector2(-36, 0));
                    NodeEditorGUILayout.PortField(pos, port);
                }
            };
        }
 
}
