using System.Collections.Generic;
using UnityEngine;

namespace _Prototype.Code.Environment.Parallax
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class ParallaxController : MonoBehaviour
    {
       [SerializeField] protected List<ParallaxLayer> layers;
       
       /// <summary>
       /// 
       /// </summary>
       /// <param name="camPos"></param>
        public abstract void InitializeLayers(Vector3 camPos);
       
       /// <summary>
       /// 
       /// </summary>
       /// <param name="pos"></param>
        public abstract void Move(Vector3 pos);
    }
}
