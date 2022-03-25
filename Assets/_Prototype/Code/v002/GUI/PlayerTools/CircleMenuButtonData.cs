using _Prototype.Code.v002.Player.Tools;
using UnityEngine;

namespace _Prototype.Code.v002.GUI.PlayerTools
{
    /// <summary>
    /// Scriptable Object asset containing data for custom UI buttons of player tools circular menu
    /// </summary>
    [CreateAssetMenu(fileName = "Circle Button Data", menuName = "Game Data/GUI/Circle Button Data", order = 0)]
    public class CircleMenuButtonData : ScriptableObject
    {
        [SerializeField] private string toolName;
        [SerializeField] private PlayerTool toolType;
        [SerializeField] private string toolDescription;
        [SerializeField] private Sprite toolIcon;

        public string ToolName => toolName;
        public PlayerTool ToolType => toolType;
        public string ToolDescription => toolDescription;
        public Sprite ToolIcon => toolIcon;
    }
}
