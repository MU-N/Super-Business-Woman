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
            transform.DORotate(rotationTraget, 1F, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
        }
        [ContextMenu("InterAct")]
        public void Interact()
        {

            
        }

       
    }
}
