using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Nasser.SBW.Core
{
    
    public class Money : MonoBehaviour,IIntercatable
    {
        [SerializeField] private Vector3 rotationTraget;
        
        private void Start()
        {
            //transform.DOLocalRotate(rotationTraget, 1).SetEase(Ease.InSine).SetLoops(-1);
        }
        [ContextMenu("InterAct")]
        public void Interact()
        {

            transform.DORotate(rotationTraget, 2.5F,RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1,LoopType.Restart);
        }

       
    }
}
