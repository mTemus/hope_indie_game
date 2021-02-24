using UnityEngine;

namespace Code.Utilities
{
    public static class Utilities
    {
        //TODO: fix main cam
        public static Vector2 GetMouseWorldPosition2D() =>
            Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}