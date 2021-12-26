using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Nasser.SBW.Core;
using DG.Tweening;

namespace Nasser.SBW
{
    public class ArrowController : MonoBehaviour, IIntercatable
    {
        // 0 for green 1 for red
        [SerializeField] int arrowIndex;

        [SerializeField] ParticleSystem ps;
        [SerializeField] GameEvent upArrow;
        [SerializeField] GameEvent downArrow;
        [SerializeField] Vector3 targetRotation;


        MeshRenderer mesh;

        private void Start()
        {
            mesh = GetComponent<MeshRenderer>();
            transform.DORotate(targetRotation, 1.75f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
        }
        public void Interact()
        {
            //ps.Play();
            transform.DOScale(new Vector3(0,0,0), .2F).SetEase(Ease.InBounce).SetLoops(1);
            if (arrowIndex == 0)
                upArrow.Raise();
            else
                downArrow.Raise();

        }


    }
}
