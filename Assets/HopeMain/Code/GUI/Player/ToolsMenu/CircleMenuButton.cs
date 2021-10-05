using UnityEngine;
using UnityEngine.UI;

namespace HopeMain.Code.GUI.Player.ToolsMenu
{
    public class CircleMenuButton : MonoBehaviour
    {
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Image iconImage;
        [SerializeField] private RectTransform iconRectTransform;

        public RectTransform RectTransform => rectTransform;

        public Image BackgroundImage => backgroundImage;

        public Image IconImage => iconImage;

        public RectTransform IconRectTransform => iconRectTransform;
    }
}
