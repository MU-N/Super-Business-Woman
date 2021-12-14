using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;
using MoreMountains.NiceVibrations;
using UnityEngine.UI;

namespace Nasser.SBW.Core
{
    public class PlayerController : MonoBehaviour
    {



        [SerializeField] private float speed = 0.5f, computerSpeed, dir = -1f;
        [SerializeField] private float mapWidth = 2.5f;
        [Header("Visuals")]
        [SerializeField] private GameObject[] girlVisuals;

        [Header("Effects")]
        [SerializeField] private ParticleSystem[] gateEffect;
        [Header("Slider")]
        [SerializeField] private Slider playerSlider;
        [SerializeField] private float maxSliderAmount;
        private float currentSliderAmount;


        private bool touching = false;
        private bool firstTouch = false;
        private bool isWinBool = false;


        private float positionX, positionY;

        private int currentGirlVisualIndex = 0;


        private int animIsWalking;
        private int animIsWin;
        private int animIsLose;

        private Vector3 originPos;

        private SplineFollower splineFollower;
        private Rigidbody rb;
        [SerializeField] private Animator [] animator;

        private Touch initTouch = new Touch();

        WaitForSeconds waitFor50ms = new WaitForSeconds(.5f);
        WaitForSeconds waitFor150ms = new WaitForSeconds(1.5f);
        void Start()
        {
            //Initializations
            rb = GetComponent<Rigidbody>();
            
            splineFollower = GetComponentInParent<SplineFollower>();
            positionX = 0f;
            positionY = transform.localPosition.y;
            originPos = transform.localPosition;

            splineFollower.follow = false;

            animIsWalking = Animator.StringToHash("isWalk");
            animIsWin = Animator.StringToHash("isWin");
            animIsLose = Animator.StringToHash("isLose");

            currentSliderAmount = 0;
            currentGirlVisualIndex = 0;

            playerSlider.value = currentSliderAmount;
            for (int i = 0; i < 2; i++)
            {
                if (i == currentGirlVisualIndex)
                    girlVisuals[i].SetActive(true);
                else
                    girlVisuals[i].SetActive(false);
            }
        }

        void Update()
        {
            if (!isWinBool)
            {
                foreach (Touch touch in Input.touches)
                {
                    if (touch.phase == TouchPhase.Began)        //if finger touches the screen
                    {
                        if (!firstTouch)
                        {
                            splineFollower.follow = true;
                            animator[0].SetBool(animIsWalking, true);
                            animator[1].SetBool(animIsWalking, true);
                            firstTouch = true;
                        }
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
            StartCoroutine(nameof(WaitForPlayms));
        }

        private void PlayEffect()
        {
            LeanTween.moveLocalY(gateEffect[1].gameObject, 1.65f, .75f).setLoopPingPong();
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
                item.transform.position = transform.position;
                foreach (Transform item2 in item.transform)
                {
                    item2.GetComponent<ParticleSystem>().Stop();
                }
            }
        }

        private void CheckForCurrentGirlVisual()
        {
            if(currentSliderAmount>=50)
                currentGirlVisualIndex = 1;
            else
                currentGirlVisualIndex = 0;

            for (int i = 0; i < 2; i++)
            {
                if (i == currentGirlVisualIndex)
                    girlVisuals[i].SetActive(true);
                else
                    girlVisuals[i].SetActive(false);
            }
            animator[0].SetBool(animIsWalking, true);
            animator[1].SetBool(animIsWalking, true);


        }
        private void UpdateSlider()
        {
            playerSlider.value = currentSliderAmount / maxSliderAmount;
            CheckForCurrentGirlVisual();

        }

        public void Add5Points()
        {
            currentSliderAmount += 5;
            UpdateSlider();
        }

        public void Add10Points()
        {
            currentSliderAmount += 10;
            UpdateSlider();
        }

        public void Sub5Points()
        {
            currentSliderAmount -= 5;
            UpdateSlider();
        }

        public void Sub10Points()
        {
            currentSliderAmount -= 10;
            UpdateSlider();
        }

        public void win()
        {
            splineFollower.follow = false;
            animator[0].SetBool(animIsWin, true);
            animator[1].SetBool(animIsWin, true);
            isWinBool = true;
            //LeanTween.rotateY(transform.GetChild(0).gameObject, 360.0f, 1.0f).setRepeat(1);
            //LeanTween.rotateAround(gameObject, Vector3.up, 360, 2.5f).setLoopClamp().setLoopOnce() ;
        }

        [ContextMenu("Rotate")]
        public void TakeBusiness()
        {
            //LeanTween.rotateY(transform.GetChild(0).gameObject, 360.0f, 1.0f).setRepeat(1);
            LeanTween.rotateAround(girlVisuals[0], Vector3.up, 360, 1f).setLoopClamp().setLoopOnce();
            LeanTween.rotateAround(girlVisuals[1], Vector3.up, 360, 1f).setLoopClamp().setLoopOnce();

        }


        IEnumerator WaitFor75ms()
        {
            yield return waitFor150ms;
            StopGateParticleEffect();
        }
        IEnumerator WaitForPlayms()
        {
            yield return waitFor50ms;
            PlayEffect();
        }



    }
}
