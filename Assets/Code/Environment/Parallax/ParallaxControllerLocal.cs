using Code.System;
using Code.System.Areas;
using UnityEngine;

namespace Code.Environment.Parallax
{
    public class ParallaxControllerLocal : ParallaxController
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
