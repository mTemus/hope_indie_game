using UnityEngine;

namespace Code.Player.Tools
{
    public class PlayerToolsManager : MonoBehaviour
    {
        private readonly PlayerTool hammer = new PlayerToolHammer();
        private readonly PlayerTool hand = new PlayerToolFreeHand();
        private readonly PlayerTool goldPocket = new PlayerToolGoldPocket();
        private readonly PlayerTool villagersBook = new PlayerToolVillagersBook();
        private readonly PlayerTool buildingsBook = new PlayerToolBuildingsBook();

        private PlayerTool currentTool;

        private void Awake()
        {
            currentTool = hand;
        }

        public void UseCurrentTool()
        {
            currentTool.UseTool();
        }
        
        public void SelectTool(int toolIndex)
        {
            currentTool = toolIndex switch {
                0 => buildingsBook,
                1 => villagersBook,
                2 => goldPocket,
                3 => hammer,
                4 => hand,
                _ => currentTool
            };
            
            Debug.LogWarning(currentTool.ToString() + " selected.");
        }
    }
}
