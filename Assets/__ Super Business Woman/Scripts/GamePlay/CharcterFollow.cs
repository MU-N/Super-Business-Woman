using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Nasser.SBW.Core;

namespace Nasser.SBW
{
    public class CharcterFollow : MonoBehaviour, IIntercatable
    {
        [SerializeField] Transform followLocation;
        [HideInInspector] public  Transform myParent;


        Animator animator;
        private int animIsWalk;
        // Start is called before the first frame update
        void Start()
        {
            animator = GetComponent<Animator>();
            animIsWalk = Animator.StringToHash("isWalk");
            myParent = transform.parent;
        }

        public void Interact()
        {
            if(followLocation.childCount > 0)
            {
                followLocation.GetChild(0).gameObject.SetActive(false);
                followLocation.GetChild(0).parent = followLocation.GetChild(0).GetComponent<CharcterFollow>().myParent;
                
            }
            transform.position = followLocation.position;
            transform.rotation = followLocation.rotation;
            transform.parent = followLocation;
            animator.SetBool(animIsWalk,true);
        }
    }



    // todo add gui 
    // todo add sound 
    // todo add investment 
    // todo add arrows 
    // todo add finsih effect 
    // todo add player  slider
    // 
}
