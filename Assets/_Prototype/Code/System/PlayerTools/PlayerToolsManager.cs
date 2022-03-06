using UnityEngine;

namespace _Prototype.Code.System.PlayerTools
{
    public class PlayerToolsManager : MonoBehaviour
    {
        private readonly Tool _hammer = new Hammer();
        private readonly Tool _hand = new FreeHand();
        private readonly Tool _goldPocket = new GoldPocket();
        private readonly Tool _villagersBook = new VillagersBook();
        private readonly Tool _buildingsBook = new BuildingsBook();

        private Tool _currentTool;

        private void Awake()
        {
            _currentTool = _hand;
        }

        public void UseCurrentTool()
        {
            _currentTool.UseTool();
        }
        
        public void SelectTool(int toolIndex)
        {
            _currentTool = toolIndex switch {
                0 => _buildingsBook,
                1 => _villagersBook,
                2 => _goldPocket,
                3 => _hammer,
                4 => _hand,
                _ => _currentTool
            };
            
            Debug.LogWarning(_currentTool + " selected.");
        }
    }
}
