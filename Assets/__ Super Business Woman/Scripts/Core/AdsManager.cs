using System.Collections;
using System;
using UnityEngine;
using UnityEngine.Advertisements;

namespace Nasser.SBW
{
    public class AdsManager : MonoBehaviour, IUnityAdsListener
    {
#if UNITY_IOS
string gameId = "4520814";
#else
        string gameId = "4520815";
#endif
        Action onRewardedAdsSuccess;
        
        WaitForSeconds waitFor1Second = new WaitForSeconds(1f);


        void Start()
        {
            Advertisement.Initialize(gameId);
            Advertisement.AddListener(this);

            PlayBannerAds();    
        }


        public void PlayInterstitialAds()
        {
            if (Advertisement.IsReady("Interstitial_Android"))
            {
                Advertisement.Show("Interstitial_Android");
            }
        }

        public void PlayRewardedAds( Action onSuccess)
        {
            onRewardedAdsSuccess = onSuccess;
            if (Advertisement.IsReady("Rewarded_Android"))
            {
                Advertisement.Show("Rewarded_Android");
            }
            else
            {
                Debug.Log("Rewarded not ready ");
            }
        }

        public void PlayBannerAds()
        {
            if (Advertisement.IsReady("Banner_Android"))
            {
                Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
                Advertisement.Banner.Show("Banner_Android");
            }
            else
            {
                StartCoroutine(RepeateShowBanner());
            }
        }

        public void StopBannerAds()
        {

            Advertisement.Banner.Hide();

        }


        IEnumerator RepeateShowBanner()
        {
            yield return waitFor1Second;
            PlayBannerAds();

        }



        public void OnUnityAdsReady(string placementId)
        {
            Debug.Log("ads ready ");
        }

        public void OnUnityAdsDidError(string message)
        {
            Debug.Log("ads eror " + message);
        }

        public void OnUnityAdsDidStart(string placementId)
        {
            Debug.Log("ads strarted ");
        }

        public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
        {
            if (placementId == "Rewarded" && showResult == ShowResult.Finished)
            {
                Debug.Log("Rewarded to player ");
                onRewardedAdsSuccess.Invoke();  
            }
        }


    }
}
