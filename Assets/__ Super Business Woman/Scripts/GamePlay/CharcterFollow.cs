using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Nasser.SBW.Core;
//using MoreMountains.NiceVibrations;

namespace Nasser.SBW
{
    public class CharcterFollow : MonoBehaviour, IIntercatable
    {
        // 0 businesx 1 boy
        [SerializeField] Transform followLocation;
        [HideInInspector] public Transform myParent;
        [SerializeField] private int index;
        [SerializeField] private GameEvent business;
        [SerializeField] private GameEvent boy;


        Animator animator;
        Collider[] _collider;
        private int animIsWalk;
        // Start is called before the first frame update
        void Start()
        {
            animator = GetComponent<Animator>();
            animIsWalk = Animator.StringToHash("isWalk");
            myParent = transform.parent;
            _collider = GetComponents<Collider>();
        }

        public void Interact()
        {
          //  MMVibrationManager.Haptic(HapticTypes.LightImpact);
            if (followLocation.childCount > 0)
            {
                followLocation.GetChild(0).gameObject.SetActive(false);
                followLocation.GetChild(0).parent = followLocation.GetChild(0).GetComponent<CharcterFollow>().myParent;
            }
            transform.position = followLocation.position;
            transform.rotation = followLocation.rotation;
            transform.parent = followLocation;
            animator.SetBool(animIsWalk, true);
            if (index == 0)
                business.Raise();
            else
                boy.Raise();

            _collider[0].enabled = false;
            _collider[1].enabled = false;
        }
    }



    // todo add investment 
    // todo add arrows 
    // todo add finsih effect 
    // todo : Add ui manager
}
