using UnityEngine;

namespace HopeMain.Code.System.PlayerTools
{
    public class PlayerToolsManager : MonoBehaviour
    {
        private readonly Tool hammer = new Hammer();
        private readonly Tool hand = new FreeHand();
        private readonly Tool goldPocket = new GoldPocket();
        private readonly Tool villagersBook = new VillagersBook();
        private readonly Tool buildingsBook = new BuildingsBook();

        private Tool currentTool;

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
            
            Debug.LogWarning(currentTool + " selected.");
        }
    }
}
