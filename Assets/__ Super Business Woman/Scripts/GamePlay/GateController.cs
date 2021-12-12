using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Nasser.SBW.Core;

namespace Nasser.SBW
{
    public class GateController : MonoBehaviour, IIntercatable
    {
        [SerializeField] GameEvent gameEvent;
        public void Interact()
        {
            gameEvent.Raise();  
        }

        
    }
}
