using System;
using UnityEngine;
using UnityEngine.UI;
using Nasser.SBW.Core;
using DG.Tweening;

namespace Nasser.SBW.UI
{
    public class UiManager : MonoBehaviour
    {
        [SerializeField] AdsManager adsManager;
        [SerializeField]  GameObject handImage;
        [SerializeField]  Vector3 handImageTraget;
        Vector3 handImageStart;
        float coins = 5;
        private void Start()
        {
            adsManager.PlayBannerAds();
            handImageStart = handImage.transform.position;
            handImage.transform.DOMove(handImageStart+ handImageTraget, 1F).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        }
        public void OnWin()
        {
            
        }

        public void OnLose()
        {
            adsManager.PlayInterstitialAds();   
        }

        public void OnReward()
        {
            adsManager.PlayRewardedAds(OnDoubleCoins);
        }

        private void OnDoubleCoins()
        {
           coins *= 2;
        }

    }
}
