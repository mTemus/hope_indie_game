using UnityEngine;

namespace _Prototype.Code.v001.GUI.UIElements
{
   /// <summary>
   /// 
   /// </summary>
   public class UISelectingPointer : MonoBehaviour
   {
      [SerializeField] private RectTransform pointer;
      [SerializeField] private float offset;
      
      /// <summary>
      /// 
      /// </summary>
      /// <param name="element"></param>
      public void SetPointerOnUiElement(Transform element)
      {
         Rect elementRect = element.GetComponent<RectTransform>().rect;
         Vector2 newPointerSize = new Vector2(elementRect.size.x + offset, elementRect.size.y + offset);
         pointer.sizeDelta = newPointerSize;
         pointer.transform.position = element.position;
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="element"></param>
      public void SetPointerOnUiElementWithParent(Transform element)
      {
         Transform pointerTransform = pointer.transform;
         Rect elementRect = element.GetComponent<RectTransform>().rect;
         Vector2 newPointerSize = new Vector2(elementRect.size.x + offset, elementRect.size.y + offset);
         pointer.sizeDelta = newPointerSize;
         pointerTransform.SetParent(element.transform);
         pointerTransform.localPosition = Vector3.zero;
      }
   }
}
