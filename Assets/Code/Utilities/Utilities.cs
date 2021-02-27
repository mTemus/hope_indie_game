using UnityEngine;

namespace Code.Utilities
{
    public static class Utilities
    {
        //TODO: fix main cam
        public static Vector2 GetMouseWorldPosition2D() =>
            Camera.main.ScreenToWorldPoint(Input.mousePosition);

        public static int IncrementIdx(int idx, int value, int maxValue)
        {
            if (idx + value >= maxValue) 
                return 0;
            if(idx + value < 0) 
                return maxValue;
            
            return idx + value;
        }
    }
}