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
            transform.DORotate(rotationTraget, 1.75f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
        }
        [ContextMenu("InterAct")]
        public void Interact()
        {
            transform.DOScale(transform.localScale * 2, 0.1f).SetEase(Ease.InOutBounce).SetLoops(1).OnComplete(() =>
            {
                transform.DOScale(new Vector3(0, 0, 0), 0.1f).SetEase(Ease.InBounce).SetLoops(1);
            });
            
        }

       
    }
}
