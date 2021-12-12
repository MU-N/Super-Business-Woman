using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;
using MoreMountains.NiceVibrations;

namespace Nasser.SBW.Core
{
    public class PlayerController : MonoBehaviour
    {



        [SerializeField] private float speed = 0.5f, computerSpeed, dir = -1f;
        [SerializeField] private float mapWidth = 2.5f;
        [SerializeField] private ParticleSystem []gateEffect;


        private bool touching = false;
        private bool firstTouch = false;


        private float positionX, positionY;

        private int animIsWalking;

        private Vector3 originPos;

        private SplineFollower splineFollower;
        private Rigidbody rb;
        private Animator animator;

        private Touch initTouch = new Touch();

        WaitForSeconds waitFor125ms = new WaitForSeconds(1.25f);
        void Start()
        {
            //Initializations
            rb = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();    
            splineFollower = GetComponentInParent<SplineFollower>();
            positionX = 0f;
            positionY = transform.localPosition.y;
            originPos = transform.localPosition;
            splineFollower.follow = false;
            animIsWalking = Animator.StringToHash("isWalk");
        }

        void Update()
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)        //if finger touches the screen
                {
                    if(!firstTouch)
                    {
                        splineFollower.follow = true;
                        animator.SetBool(animIsWalking, true);
                        firstTouch = true;
                    }
                    if (touching == false)
                    {
                        touching = true;
                        initTouch = touch;
                    }
                }
                else if (touch.phase == TouchPhase.Moved )       //if finger moves while touching the screen
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

        public void PlayGateParticleEffect()
        {
            LeanTween.move(gateEffect[0].gameObject,new Vector3( transform.position.x , transform.position.y+2 , transform.position.z), 1f).setLoopPingPong().setLoopOnce() ;
            foreach (var item in gateEffect)
            {
                
                item.GetComponent<ParticleSystem>().Play();
                foreach (Transform item2 in item.transform)
                {
                    item2.GetComponent<ParticleSystem>().Play();
                }
            }
            StartCoroutine(nameof(WaitFor75ms));
        }

        private void StopGateParticleEffect()
        {
            foreach (var item in gateEffect)
            {
                item.GetComponent<ParticleSystem>().Stop();
                foreach (Transform item2 in item.transform)
                {
                    item2.GetComponent<ParticleSystem>().Stop();
                }
            }
        }


        IEnumerator WaitFor75ms()
        {
            yield return waitFor125ms;
            StopGateParticleEffect();
        }



    }
}
