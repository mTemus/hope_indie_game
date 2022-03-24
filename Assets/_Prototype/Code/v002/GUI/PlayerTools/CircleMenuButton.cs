using UnityEngine;
using UnityEngine.UI;

namespace _Prototype.Code.v002.GUI.PlayerTools
{
    /// <summary>
    /// Script that imitates UI button of any shape
    /// </summary>
    public class CircleMenuButton : MonoBehaviour
    {
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private RectTransform iconRectTransform;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Image iconImage;

        public Image BackgroundImage => backgroundImage;
        public Image IconImage => iconImage;
        public RectTransform IconRectTransform => iconRectTransform;
        public RectTransform RectTransform => rectTransform;
    }
}
