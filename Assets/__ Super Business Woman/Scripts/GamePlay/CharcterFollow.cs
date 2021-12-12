using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Nasser.SBW.Core;

namespace Nasser.SBW
{
    public class CharcterFollow : MonoBehaviour, IIntercatable
    {
        [SerializeField] Transform followLocation;
        [SerializeField] float moveSpeed;
        private bool isFollow;

        Animator animator;
        private int animIsWalk;
        // Start is called before the first frame update
        void Start()
        {
            animator = GetComponent<Animator>();
            animIsWalk = Animator.StringToHash("isWalk");
        }

        // Update is called once per frame
        void Update()
        {
            if (!isFollow) return;

            animator.SetBool(animIsWalk, true);
            transform.position = Vector3.Slerp(transform.position, followLocation.position, Time.deltaTime * moveSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, followLocation.rotation, Time.deltaTime * moveSpeed);

        }

        public void Interact()
        {

            isFollow = true;
        }
    }
}
