using UnityEngine;

namespace HopeMain.Code.GUI.UIElements
{
   public class UiSelectingPointer : MonoBehaviour
   {
      [SerializeField] private RectTransform pointer;
      [SerializeField] private float offset;
      
      public void SetPointerOnUiElement(Transform element)
      {
         Rect elementRect = element.GetComponent<RectTransform>().rect;
         Vector2 newPointerSize = new Vector2(elementRect.size.x + offset, elementRect.size.y + offset);
         pointer.sizeDelta = newPointerSize;
         pointer.transform.position = element.position;
      }

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
