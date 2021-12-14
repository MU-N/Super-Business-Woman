using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Nasser.SBW.Core
{
    public class CinemaController : MonoBehaviour
    {
        [SerializeField] CinemachineVirtualCamera cam;
        [SerializeField] CinemachineVirtualCamera cam1;
        private void Awake()
        {
            cam1.gameObject.SetActive(false);
        }
        public void switchToCam2()
        {
            cam1.gameObject.SetActive(true);
            cam.Priority = 0;   
            cam1.Priority = 10;
            cam.gameObject.SetActive(false);    
        } 
    }
}
