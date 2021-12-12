using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Nasser.SBW.Core;

namespace Nasser.SBW
{
    public class ArrowController : MonoBehaviour,IIntercatable
    {
        // 0 for green 1 for red
        [SerializeField] int arrowIndex;
         MeshRenderer  mesh;
        [SerializeField] ParticleSystem ps;

        private void Start()
        {
                mesh = GetComponent<MeshRenderer>();
        }
        public void Interact()
        {
            ps.Play();
            mesh.enabled = false;
            Debug.Log("Interact");
        }

        
    }
}
