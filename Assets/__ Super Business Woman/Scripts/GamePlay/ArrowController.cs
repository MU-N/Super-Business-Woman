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
         
        [SerializeField] ParticleSystem ps;
        [SerializeField] GameEvent upArrow;
        [SerializeField] GameEvent downArrow;

        MeshRenderer mesh;

        private void Start()
        {
                mesh = GetComponent<MeshRenderer>();
        }
        public void Interact()
        {
            ps.Play();
            mesh.enabled = false;
            if (arrowIndex == 0)
                upArrow.Raise();
            else
                upArrow.Raise();

        }

        
    }
}
