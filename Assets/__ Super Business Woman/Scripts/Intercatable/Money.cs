using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Nasser.SBW.Core
{
    public class Money : MonoBehaviour,IIntercatable
    {
        [ContextMenu("InterAct")]
        public void Interact()
        {

            transform.DORotate(Vector3.up, 1).SetEase(Ease.InSine).SetLoops(-1);
        }

       
    }
}
