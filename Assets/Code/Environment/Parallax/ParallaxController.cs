using System.Collections.Generic;
using UnityEngine;

namespace Code.Environment.Parallax
{
    public abstract class ParallaxController : MonoBehaviour
    {
       [SerializeField] protected List<ParallaxLayer> layers;
       
        public abstract void InitializeLayers(Vector3 camPos);
        public abstract void Move(Vector3 pos);
    }
}
