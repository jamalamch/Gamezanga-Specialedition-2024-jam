using System.Collections;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class PanelCoinEffect : MonoBehaviour
{
    [SerializeField] Transform[] _effectCoin;
    [SerializeField] Transform _tragteCoin;
    [SerializeField] float _effectSpeed;
    [SerializeField] float _effectForce = 5;

    private void Awake()
    {
        foreach (var item in _effectCoin)
        {
            item.localScale = Vector3.zero;
        }
    }

    public void StartEffect(Vector3 stratP, Vector3 targetP, int count =  20,string sound = "CollectCoin")
    {

        for (int i = 0; i < _effectCoin.Length && i < count; i++)
        {
            Transform item = _effectCoin[i];
            item.position = stratP;
            item.localScale = Vector3.zero;
            float r = Random.value * Mathf.PI * 2;
            Vector3 direction = stratP + new Vector3(Mathf.Sin(r), Mathf.Cos(r), 0) * (_effectForce * Random.value);
            float dt = Vector3.Distance(stratP, direction) / _effectSpeed;
            item.DOKill();
            item.DOScale(1, 0.15f);
            item.DOMove(direction, dt).SetEase(Ease.InQuad).OnComplete(() => 
            {
                AudioManager.Play(sound);
                float dt1 = Vector3.Distance(targetP, item.position) / _effectSpeed;
                item.DOMove(targetP, dt1).SetEase(Ease.OutQuad).OnComplete(() => item.localScale = Vector3.zero);
            });
        }
    }

    public void FromPositionToCoin(Vector3 position, int count = 20, string sound = "CollectCoin")
    {
        StartEffect(position, _tragteCoin.position , count, sound);
    }

    [Button]
    public void TestFromCentreToCoin()
    {
        FromPositionToCoin(transform.position);
    }
}
