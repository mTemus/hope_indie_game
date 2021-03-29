using UnityEngine;

namespace Code.GUI.UIElements
{
    public abstract class UiVerticalGroup : MonoBehaviour
    {
        [SerializeField] private RectTransform[] groupElements;

        [SerializeField] private float spacing;
        [SerializeField] private float topPadding;
        
        private float currentY;
        
        public void UpdateElementsPosition()
        {
            currentY = -topPadding;
            
            foreach (RectTransform element in groupElements) {
                Vector2 elemAnchPos = element.anchoredPosition;
                currentY += -(element.sizeDelta.y / 2);
                element.anchoredPosition = new Vector2(elemAnchPos.x, currentY);
                currentY += -(element.sizeDelta.y / 2 + spacing);
            }
            
            currentY = 0;
        }
    }
}
