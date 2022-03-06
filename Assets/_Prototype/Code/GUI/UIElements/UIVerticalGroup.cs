using UnityEngine;

namespace _Prototype.Code.GUI.UIElements
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class UIVerticalGroup : MonoBehaviour
    {
        [Header("Group elements")]
        [SerializeField] private RectTransform[] groupElements;

        [Header("Group properties")]
        [SerializeField] private float spacing;
        [SerializeField] private float topPadding;
        
        private float _currentY;
        
        /// <summary>
        /// 
        /// </summary>
        public void UpdateElementsPosition()
        {
            _currentY = -topPadding;
            
            foreach (RectTransform element in groupElements) {
                Vector2 elemAnchPos = element.anchoredPosition;
                _currentY += -(element.sizeDelta.y / 2);
                element.anchoredPosition = new Vector2(elemAnchPos.x, _currentY);
                _currentY += -(element.sizeDelta.y / 2 + spacing);
            }
            
            _currentY = 0;
        }
    }
}
