using HopeMain.Code.System;
using HopeMain.Code.World.Areas;
using UnityEngine;

namespace HopeMain.Code.Environment.Parallax
{
    public class LocalParallaxController : ParallaxController
    {
        private Area myArea;
        
        private void Awake()
        {
            myArea = Managers.I.Areas.GetAreaByCoords(transform.position);
        }
        
        public override void InitializeLayers(Vector3 camPos)
        {
            foreach (ParallaxLayer layer in layers) 
                layer.InitializeLocal(camPos, myArea);
        }

        public override void Move(Vector3 pos)
        {
            foreach (ParallaxLayer layer in layers) {
                layer.MoveLocally(pos, myArea);
            }
        }
    }
}
