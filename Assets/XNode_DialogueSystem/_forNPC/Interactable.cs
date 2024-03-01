using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    private UnityEvent m_OnInteraction;

    public void DoInteraction()
    {
        m_OnInteraction.Invoke();
    }
}