using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Nasser.SBW.Core;
using DG.Tweening;
using MoreMountains.NiceVibrations;

namespace Nasser.SBW
{
    public class ArrowController : MonoBehaviour, IIntercatable
    {
        [SerializeField] GameEvent downGrade;
        [SerializeField] Vector3 targetRotation;


        MeshRenderer mesh;

        private void Start()
        {
            mesh = GetComponent<MeshRenderer>();
            transform.DORotate(targetRotation, 1.75f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
        }
        public void Interact()
        {
            downGrade.Raise();
            MMVibrationManager.Haptic(HapticTypes.LightImpact);
            transform.DOScale(transform.localScale * 2, 0.1f).SetEase(Ease.InOutBounce).SetLoops(1).OnComplete(() =>
            {
                transform.DOScale(new Vector3(0, 0, 0), 0.1f).SetEase(Ease.InBounce).SetLoops(1);
            });

        }


    }
}
