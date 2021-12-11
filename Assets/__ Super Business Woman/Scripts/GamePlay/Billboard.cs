using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nasser.SBW.Core
{
    public class Billboard : MonoBehaviour
    {
        [SerializeField] Transform cam;

        void LateUpdate()
        {
            transform.LookAt(cam);

        }
    }
}