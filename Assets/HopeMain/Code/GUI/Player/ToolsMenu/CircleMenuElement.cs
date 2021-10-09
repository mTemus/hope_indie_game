using System;
using UnityEngine;
using UnityEngine.UI;

namespace HopeMain.Code.GUI.Player.ToolsMenu
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class CircleMenuElement : MonoBehaviour
    {
        [SerializeField] private string toolName;
        [SerializeField] private string toolDescription;
        [SerializeField] private Image buttonBackground;
        [SerializeField] private Sprite toolIcon;

        public string ToolName
        {
            get => toolName;
            set => toolName = value;
        }

        public string ToolDescription
        {
            get => toolDescription;
            set => toolDescription = value;
        }

        public Image ButtonBackground
        {
            get => buttonBackground;
            set => buttonBackground = value;
        }

        public Sprite ToolIcon => toolIcon;
    }
}
