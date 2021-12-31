using System;
using UnityEngine;
using UnityEngine.UI;
using Nasser.SBW.Core;
using DG.Tweening;
using UnityEngine.SceneManagement;

namespace Nasser.SBW.UI
{
    public class UiManager : MonoBehaviour
    {
        [Header("Ads")]
       // [SerializeField] AdsManager adsManager;

        [Header("Hand First touch")]
        [SerializeField] GameObject startTouchPannel;
        [SerializeField] GameObject handImage;
        [SerializeField] Vector3 handImageTraget;
        [SerializeField] GameObject settingPanel;

        [Header("Win")]
        [SerializeField] GameObject winPannel;

        [Header("Lose")]
        [SerializeField] GameObject losePannel;
        Vector3 handImageStart;
        float coins = 5;

        private void Start()
        {
            Time.timeScale = 1;
            startTouchPannel.SetActive(true);
            winPannel.SetActive(false);
            losePannel.SetActive(false);
            settingPanel.SetActive(false);
            //adsManager.PlayBannerAds();
            handImageStart = handImage.transform.position;
            handImage.transform.DOMove(handImageStart + handImageTraget, 1F).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        }
        public void OnWin()
        {
        }

        public void OnLose()
        {
           // adsManager.PlayInterstitialAds();
        }

        public void OnReward()
        {
          //  adsManager.PlayRewardedAds(OnDoubleCoins);
        }

        private void OnDoubleCoins()
        {
            coins *= 2;
        }

        public void HideFirstPaned()
        {
            startTouchPannel.SetActive(false);  
        }
        public void LoadSameScene()
        {
           SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }

       


    }
}
