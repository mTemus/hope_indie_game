using UnityEngine;

namespace Code.Player.Tools
{
    public class PlayerToolsManager : MonoBehaviour
    {
        [SerializeField] private Tool[] tools;

        private int toolIdx = 0;

        public void SelectTool(int increase)
        {
            toolIdx += increase;

            if (toolIdx > tools.Length - 1) 
                toolIdx = 0;

            if (toolIdx < 0) 
                toolIdx = tools.Length - 1;
        
            Debug.Log(tools[toolIdx].name + " selected.");
        }

        public void UseCurrentTool()
        {
            tools[toolIdx].UseTool();
        
            Debug.Log(tools[toolIdx].name + " used.");
        }
    }
}