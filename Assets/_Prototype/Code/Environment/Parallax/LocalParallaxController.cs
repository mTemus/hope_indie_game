using _Prototype.Code.System;
using _Prototype.Code.World.Areas;
using UnityEngine;

namespace _Prototype.Code.Environment.Parallax
{
    /// <summary>
    /// 
    /// </summary>
    public class LocalParallaxController : ParallaxController
    {
        private Area _myArea;
        
        private void Awake()
        {
            _myArea = Managers.I.Areas.GetAreaByCoords(transform.position);
        }
        
        public override void InitializeLayers(Vector3 camPos)
        {
            foreach (ParallaxLayer layer in layers) 
                layer.InitializeLocal(camPos, _myArea);
        }

        public override void Move(Vector3 pos)
        {
            foreach (ParallaxLayer layer in layers) {
                layer.MoveLocally(pos, _myArea);
            }
        }
    }
}
