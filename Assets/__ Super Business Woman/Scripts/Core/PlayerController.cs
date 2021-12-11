using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nasser.SBW.Core
{
    public class PlayerController : MonoBehaviour
    {



        [SerializeField] private float speed = 0.5f, computerSpeed, dir = -1f;
        [SerializeField] private float mapWidth = 2.5f;


        private bool touching = false;


        private float positionX, positionY;

        private Vector3 originPos;

        private Rigidbody rb;
        private Animator animator;

        private Touch initTouch = new Touch();
        void Start()
        {
            //Initializations
            rb = GetComponent<Rigidbody>();
            positionX = 0f;
            positionY = transform.localPosition.y;
            originPos = transform.localPosition;
        }

        void Update()
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)        //if finger touches the screen
                {
                    if (touching == false)
                    {
                        touching = true;
                        initTouch = touch;
                    }
                }
                else if (touch.phase == TouchPhase.Moved)       //if finger moves while touching the screen
                {
                    float deltaX = initTouch.position.x - touch.position.x;
                    positionX -= deltaX * Time.deltaTime * speed * dir;
                    positionX = Mathf.Clamp(positionX, -mapWidth, mapWidth);      //to set the boundaries of the player's position
                    transform.localPosition = new Vector3(positionX, positionY, 0f);
                    initTouch = touch;
                }
                else if (touch.phase == TouchPhase.Ended)       //if finger releases the screen
                {
                    initTouch = new Touch();
                    touching = false;
                }
            }

            //if you play on computer---------------------------------
            float x = Input.GetAxis("Horizontal") * Time.deltaTime * computerSpeed;     //you can move by pressing 'a' - 'd' or the arrow keys
            Vector3 newPosition = rb.transform.localPosition + Vector3.right * x;
            newPosition.x = Mathf.Clamp(newPosition.x, -mapWidth, mapWidth);
            transform.localPosition = newPosition;
            //--------------------------------------------------------
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
