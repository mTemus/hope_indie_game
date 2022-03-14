using UnityEngine;

namespace _Prototype.Code.v001.Environment.Parallax
{
    /// <summary>
    /// 
    /// </summary>
    public class GlobalParallaxController : ParallaxController
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
