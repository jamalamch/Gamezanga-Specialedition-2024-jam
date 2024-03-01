using UnityEngine;
using UnityEngine.UI;

public class RandomImage : MonoBehaviour
{
     [SerializeField] Sprite[] _npcImages;
    void Start()
    {
        GetComponent<Image>().sprite = _npcImages[Random.Range(0, _npcImages.Length)];
    }
}
