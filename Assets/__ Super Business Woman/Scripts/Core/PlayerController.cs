using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nasser.SBW.Core
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] float moveSpeed;


        Rigidbody rb;
        Animator animator;
        void Start()
        {
            rb = GetComponent<Rigidbody>(); 
            animator = GetComponent<Animator>();
        }

        void Update()
        {
            GetInput();
        }


        private void FixedUpdate()
        {
            
        }
        void GetInput()
        {
            
        }


        // ------------- 
        private void OnTriggerEnter(Collider other)
        {
            var interactable = other.GetComponent<IIntercatable>();
            if (interactable == null) return;
            interactable.Interact();
        }
    }
}
