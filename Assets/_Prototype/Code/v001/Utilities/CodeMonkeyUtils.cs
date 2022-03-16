using TMPro;
using UnityEngine;

namespace _Prototype.Code.v001.Utilities
{
    public static class CodeMonkeyUtils
    {
        public const int SORTING_ORDER_DEFAULT = 5000;
        
        public static TextMeshPro ShowWorldText(string text, Transform parent = null, Vector3 localPosition = default, int fontSize = 40, Color? color = null, int sortingOrder = SORTING_ORDER_DEFAULT) {
            if (color == null) color = Color.white;
            return CreateWorldText(parent, text, localPosition, fontSize, (Color)color, sortingOrder);
        }
        
        // Create Text in the World
        private static TextMeshPro CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, int sortingOrder) {
            GameObject gameObject = new GameObject(text);
            Transform transform = gameObject.transform;
            transform.SetParent(parent, false);
            transform.localPosition = localPosition;

            TextMeshPro tmp = gameObject.AddComponent<TextMeshPro>();
            tmp.fontSize = fontSize;
            tmp.color = color;
            tmp.text = text;
            tmp.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
            
            return tmp;
        }











    }
}
