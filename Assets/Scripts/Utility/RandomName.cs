using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomName : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshPro nameTxt;
    [SerializeField] TextAsset namesFile;

    private static string[] names;

    private void Start()
    {
        if(names == null)
        {
            names = namesFile.ToString().Split('\n');
        }
        nameTxt.text = names[Random.Range(0, names.Length)];
    }
}
