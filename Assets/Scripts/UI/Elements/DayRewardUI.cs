using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayRewardUI : MonoBehaviour
{
    [SerializeField] GameObject _canClaim;
    [SerializeField] GameObject _claimed;
    [SerializeField] GameObject _claimParticle;

    public string skin; 
    public int coin; 
    public int crystale;

    public void CanClaim()
    {
        _canClaim.SetActive(true);
    }

    internal void Claimed()
    {
        _claimed.SetActive(true);
    }

    public void Claim()
    {
        _claimed.SetActive(true);
        _claimParticle.SetActive(true);
    }

    private void OnValidate()
    {
        _canClaim = transform.Find("Reward/CanGet").gameObject;
        _claimed = transform.Find("Reward/Claimed").gameObject;
        _claimParticle = transform.Find("Get_rewardEFF").gameObject;
    }

}
