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
        [SerializeField] private GameObject girlVisual;


        [Header("Effects")]
        [SerializeField] private ParticleSystem[] gateEffect;
        [Header("Slider")]
        [SerializeField] private Slider playerSlider;
        [SerializeField] private float maxSliderAmount;

        [Header("Texures")]
        [SerializeField] Texture[] m_MainTexture;
        [SerializeField] Renderer[] m_Renderer;
        private int texureIndex;

        [Header("First Event")]
        [SerializeField] private GameEvent firstTouchEvent;
        private float currentSliderAmount;

        [Header("Finsh")]
        [SerializeField] private GameObject[] finishStepsLocation;

        [Header("Win lose events ")]
        [SerializeField] GameEvent winEvent;
        [SerializeField] GameEvent loseEvent;

        private bool touching = false;
        private bool firstTouch = false;
        private bool isWinBool = false;


        private float positionX, positionY;
        private float defultSpeed;


        private int animIsWalking;
        private int animIsWin;
        private int animIsLose;
        private int tempTextureIndex;

        private Vector3 originPos;

        private PathFollower pathFollower;
        private Rigidbody rb;
        private Animator animator;

        private Touch initTouch = new Touch();

        WaitForSeconds waitFor50ms = new WaitForSeconds(.5f);
        WaitForSeconds waitFor150ms = new WaitForSeconds(1.5f);
        WaitForSeconds waitFor500ms = new WaitForSeconds(5f);
        void Start()
        {

            //Initializations
            rb = GetComponent<Rigidbody>();
            animator = GetComponentInChildren<Animator>();
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

            playerSlider.value = currentSliderAmount;


            texureIndex = 0;
            UpdateTexture(texureIndex);

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
                            animator.SetBool(animIsWalking, true);
                            firstTouch = true;
                            firstTouchEvent.Raise();
                        }
                        if (touching == false)
                        {
                            touching = true;
                            initTouch = touch;
                        }
                    }
                    else if (touch.phase == TouchPhase.Moved)
                    {
                        float deltaX = initTouch.position.x - touch.position.x;
                        positionX -= deltaX * Time.deltaTime * speed * dir;
                        positionX = Mathf.Clamp(positionX, -mapWidth, mapWidth);      //to set the boundaries of the player's position
                        transform.localPosition = new Vector3(positionY, positionX, 0f);
                        initTouch = touch;
                    }
                    else if (touch.phase == TouchPhase.Ended)
                    {
                        initTouch = new Touch();
                        touching = false;
                    }
                }
            }

            //if you play on computer---------------------------------
            float x = Input.GetAxis("Horizontal") * Time.deltaTime * computerSpeed;
            Vector3 newPosition = rb.transform.localPosition + Vector3.right * x;
            newPosition.x = Mathf.Clamp(newPosition.x, -mapWidth, mapWidth);
            transform.localPosition = newPosition;
        }



        private void UpdateTexture(int index)
        {

            m_Renderer[0].material.SetTexture("_BaseMap", m_MainTexture[index]);
            m_Renderer[1].material.SetTexture("_BaseMap", m_MainTexture[index]);
        }

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
            }
        }

        private void CheckForCurrentGirlVisual()
        {
            tempTextureIndex = texureIndex;
            if (currentSliderAmount <= 25 && currentSliderAmount > 0)
                texureIndex = 0;
            else if (currentSliderAmount <= 50 && currentSliderAmount > 25)
                texureIndex = 1;
            else if (currentSliderAmount <= 75 && currentSliderAmount > 50)
                texureIndex = 2;
            else if (currentSliderAmount <= 100 && currentSliderAmount > 75)
                texureIndex = 3;

            UpdateTexture(texureIndex);
            if (tempTextureIndex != texureIndex)
            {
                VisualyUpdateRotaion();
                PlayEffect();
            }

        }


        public void CheckForEndStep()
        {
            if (currentSliderAmount <= 12.5 && currentSliderAmount > 0)
                transform.DOMove(finishStepsLocation[0].transform.position, 1F).SetEase(Ease.InOutSine).OnComplete(Win);
            else if (currentSliderAmount <= 25 && currentSliderAmount > 12.5)
                transform.DOMove(finishStepsLocation[1].transform.position, 2F).SetEase(Ease.InOutSine).OnComplete(Win);
            else if (currentSliderAmount <= 37.5 && currentSliderAmount > 25)
                transform.DOMove(finishStepsLocation[2].transform.position, 3F).SetEase(Ease.InOutSine).OnComplete(Win);
            else if (currentSliderAmount <= 50 && currentSliderAmount > 37.5)
                transform.DOMove(finishStepsLocation[3].transform.position, 4F).SetEase(Ease.InOutSine).OnComplete(Win);
            else if (currentSliderAmount <= 62.5 && currentSliderAmount > 50)
                transform.DOMove(finishStepsLocation[4].transform.position, 5F).SetEase(Ease.InOutSine).OnComplete(Win);
            else if (currentSliderAmount <= 75 && currentSliderAmount > 62.5)
                transform.DOMove(finishStepsLocation[5].transform.position, 6F).SetEase(Ease.InOutSine).OnComplete(Win);
            else if (currentSliderAmount <= 87.5 && currentSliderAmount > 75)
                transform.DOMove(finishStepsLocation[6].transform.position, 7F).SetEase(Ease.InOutSine).OnComplete(Win);
            else if (currentSliderAmount <= 100 && currentSliderAmount > 87.5)
                transform.DOMove(finishStepsLocation[7].transform.position, 10F).SetEase(Ease.InOutSine).OnComplete(Win);

            else
                transform.DOMove(finishStepsLocation[8].transform.position, 10F).SetEase(Ease.InOutSine).OnComplete(Lose);

        }
        private void UpdateSlider()
        {
            playerSlider.value = currentSliderAmount / maxSliderAmount;
            playerSlider.transform.GetChild(0).GetChild(0).GetComponent<Image>().color = Color.Lerp(Color.red, Color.green, playerSlider.value);
            CheckForCurrentGirlVisual();

        }

        public void Add5Points()
        {
            if (currentSliderAmount < 100)
                currentSliderAmount += 3.5f;
            else
                currentSliderAmount = 100;
            UpdateSlider();
        }

        public void Add10Points()
        {
            if (currentSliderAmount < 100)
                currentSliderAmount += 7;
            else
                currentSliderAmount = 100;
            UpdateSlider();
        }

        public void Sub5Points()
        {
            if (currentSliderAmount > 0)
                currentSliderAmount -= 3.5f;
            else
                currentSliderAmount = 0;
            UpdateSlider();
        }

        public void Sub10Points()
        {
            if (currentSliderAmount > 0)
                currentSliderAmount -= 7;
            else
                currentSliderAmount = 0;
            UpdateSlider();
        }

        public void Win()
        {
            animator.SetBool(animIsWin, true);
            StartCoroutine(CallWinOrLose(0));

        }

        public void Lose()
        {
            animator.SetBool(animIsLose, true);
            StartCoroutine(CallWinOrLose(1));
        }

        public void ReachFinalStep()
        {
            pathFollower.speed = 0;
            isWinBool = true;
        }

        public void VisualyUpdateRotaion()
        {
            girlVisual.transform.DORotate(new Vector3(0, 360 + 90, 0), 1F, RotateMode.FastBeyond360).SetEase(Ease.InOutSine).SetLoops(1);
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

        IEnumerator CallWinOrLose( int index)
        {
            yield return waitFor500ms;
            if (index == 0)
                winEvent.Raise();
            else
                loseEvent.Raise();
             
            
        }



    }
}
