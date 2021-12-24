using System;
using UnityEngine;
using Nasser.SBW.Core;

namespace Nasser.SBW.UI
{
    public class UiManager : MonoBehaviour
    {
        [SerializeField] AdsManager adsManager;

        private void Start()
        {
            adsManager.PlayBannerAds(); 
        }
        public void OnWin()
        {

        }

        public void OnLose()
        {
            adsManager.PlayInterstitialAds();   
        }

        private void OnReward()
        {
            adsManager.PlayRewardedAds(OnDoubleCoins);
        }

        private void OnDoubleCoins()
        {

        }

    }
}
