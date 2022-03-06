using UnityEngine;

namespace _Prototype.Code.Environment.Parallax
{
    /// <summary>
    /// 
    /// </summary>
    public class WaterParallaxController : ParallaxController
    {
        public override void InitializeLayers(Vector3 camPos)
        {
            foreach (ParallaxLayer layer in layers) 
                layer.Initialize(camPos);
        }

        public override void Move(Vector3 pos)
        {
            foreach (ParallaxLayer layer in layers) 
                layer.MoveOnWater(pos);
        }
    }
}
