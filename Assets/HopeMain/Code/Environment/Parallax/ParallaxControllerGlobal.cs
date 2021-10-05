using UnityEngine;

namespace HopeMain.Code.Environment.Parallax
{
    public class ParallaxControllerGlobal : ParallaxController
    {
        public override void InitializeLayers(Vector3 camPos)
        {
            foreach (ParallaxLayer layer in layers) 
                layer.Initialize(camPos);
        }

        public override void Move(Vector3 pos)
        {
            foreach (ParallaxLayer layer in layers) {
                layer.MoveGlobally(pos);
            }
        }
    }
}
