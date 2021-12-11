using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Nasser.SBW.Core;

namespace Nasser.SBW
{
    public class ArrowController : MonoBehaviour,IIntercatable
    {
        // 0 for green 1 for red
        [SerializeField] int arrowIndex;
        [SerializeField] ParticleSystem ps;
        public void Interact()
        {
            
        }

        
    }
}
