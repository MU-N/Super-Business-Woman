using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Nasser.SBW.Core;
using MoreMountains.NiceVibrations;

namespace Nasser.SBW
{
    public class FinalStep : MonoBehaviour,IIntercatable
    {
        [SerializeField] GameEvent gameEvent;
        public void Interact()
        {
            MMVibrationManager.Haptic(HapticTypes.LightImpact);
            gameEvent.Raise();
        }

       
    }
}
