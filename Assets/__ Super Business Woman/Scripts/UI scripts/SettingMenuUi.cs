//using MoreMountains.NiceVibrations;
using UnityEngine;
using Nasser.SBW.Core;

namespace Nasser.SBW.UI
{
    public class SettingMenuUi : MonoBehaviour
    {


        bool isSoundOn = true;
        bool isMusicOn = true;
        bool isHapticOn = true;


        public void CancelSettingMenu()
        {
            Time.timeScale = 1;
        }

        public void ActivetSound()
        {

            //todo : Mute or unmute sound
            isSoundOn = !isSoundOn;
           // MMVibrationManager.Haptic(HapticTypes.LightImpact);
            AudioManager.instance.ActivateSound(0, isSoundOn);
        }
        public void ActivetMusic()
        {
            //todo : Mute or unmute music
            isMusicOn = !isMusicOn;
         //   MMVibrationManager.Haptic(HapticTypes.LightImpact);
            AudioManager.instance.ActivateSound(1, isMusicOn);
        }
        public void ActivetHaptic()
        {
            //todo : Mute or unmute haptic
            isHapticOn = !isHapticOn;
           // MMVibrationManager.Haptic(HapticTypes.LightImpact);
          //  MMVibrationManager.SetHapticsActive(isHapticOn);
        }
    }
}