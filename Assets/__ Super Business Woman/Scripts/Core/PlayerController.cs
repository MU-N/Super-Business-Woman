using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.NiceVibrations;
using UnityEngine.UI;
using PathCreation.Examples;
using DG.Tweening;

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

        [Header("First Event")]
        [SerializeField] private GameEvent firstTouchEvent;
        private float currentSliderAmount;


        private bool touching = false;
        private bool firstTouch = false;
        private bool isWinBool = false;


        private float positionX, positionY;
        private float defultSpeed;
        

        private int currentGirlVisualIndex = 0;


        private int animIsWalking;
        private int animIsWin;
        private int animIsLose;

        private Vector3 originPos;

        private PathFollower pathFollower;
        private Rigidbody rb;
        [SerializeField] private Animator[] animator;

        private Touch initTouch = new Touch();

        WaitForSeconds waitFor50ms = new WaitForSeconds(.5f);
        WaitForSeconds waitFor150ms = new WaitForSeconds(2.5f);
        void Start()
        {
            //Initializations
            rb = GetComponent<Rigidbody>();

            pathFollower = GetComponentInParent<PathFollower>();
            defultSpeed = pathFollower.speed;
            positionX = 0f;
            positionY = transform.localPosition.y;
            originPos = transform.localPosition;

            pathFollower.speed = 0;

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
                           pathFollower.speed = defultSpeed;
                            animator[0].SetBool(animIsWalking, true);
                            animator[1].SetBool(animIsWalking, true);
                            firstTouch = true;
                            firstTouchEvent.Raise();
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
                        transform.localPosition = new Vector3(positionY, positionX, 0f);
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

                item.transform.localPosition = transform.position;
            }
        }

        private void CheckForCurrentGirlVisual()
        {
            if (currentSliderAmount >= 50)
            {
                if (currentGirlVisualIndex == 0)
                {
                    PlayEffect();
                    TakeBusiness();
                }
                currentGirlVisualIndex = 1;


            }
            else
            {
                if (currentGirlVisualIndex == 1)
                {
                    PlayEffect();
                    TakeBusiness();
                }
                currentGirlVisualIndex = 0;
            }

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
            playerSlider.transform.GetChild(0).GetChild(0).GetComponent<Image>().color = Color.Lerp(Color.red, Color.green,playerSlider.value);
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
            pathFollower.speed = 0;
            animator[0].SetBool(animIsWin, true);
            animator[1].SetBool(animIsWin, true);
            isWinBool = true;
        }

        public void TakeBusiness()
        {
            girlVisuals[0].transform.DORotate(new Vector3(0,360+90,0), 1F, RotateMode.FastBeyond360).SetEase(Ease.InOutSine).SetLoops(1);
            girlVisuals[0].transform.DORotate(new Vector3(0, 360+90, 0), 1F, RotateMode.FastBeyond360).SetEase(Ease.InOutSine).SetLoops(1);

            

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
