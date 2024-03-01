using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UIParty
{
    public class PanelCheckConnectionUI : PanelUI
    {
        public override void Init()
        {
            base.Init();
            StartCoroutine(TryToConnect());
        }

        IEnumerator TryToConnect()
        {
            while (true)
            {
                yield return null;
                yield return null;
                yield return null;
                bool reReachable = Application.internetReachability == NetworkReachability.NotReachable;
                if (reReachable)
                    Open();
                else
                    Close();
            }
        }
    }
}
