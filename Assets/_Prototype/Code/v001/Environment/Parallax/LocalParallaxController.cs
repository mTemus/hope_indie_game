using _Prototype.Code.v001.System;
using _Prototype.Code.v001.World.Areas;
using UnityEngine;

namespace _Prototype.Code.v001.Environment.Parallax
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
