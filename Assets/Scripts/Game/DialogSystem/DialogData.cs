using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu()]
public class DialogData : ScriptableObject
{
    public DilogTextData[] dilogTextDatas;

    [System.Serializable]
    public struct DilogTextData
    {
        public int id;
        public string dilogText;
        public bool isOption;
        [ShowIf("isOption")]
        public DilogTextDataOption[] option;
        public int characterIndex;
        public int nextDilogId;
    }


    [System.Serializable]
    public struct DilogTextDataOption
    {
        public string textOption;
        public int nextDilogId;
    }
}
