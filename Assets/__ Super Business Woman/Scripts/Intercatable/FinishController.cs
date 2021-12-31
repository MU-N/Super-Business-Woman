using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Nasser.SBW.Core;
using MoreMountains.NiceVibrations;

namespace Nasser.SBW
{
    public class FinishController : MonoBehaviour,IIntercatable
    {
       [SerializeField] ParticleSystem [] _particleSystem;
       [SerializeField] GameEvent reachFinish;



        public void Interact()
        {
            reachFinish.Raise();
            MMVibrationManager.Haptic(HapticTypes.MediumImpact);
            foreach (var item in _particleSystem)
            {

                item.GetComponent<ParticleSystem>().Play();
                foreach (Transform item2 in item.transform)
                {
                    item2.GetComponent<ParticleSystem>().Play();
                }
            }
        }
    }
}
