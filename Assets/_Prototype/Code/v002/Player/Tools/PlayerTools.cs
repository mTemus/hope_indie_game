using _Prototype.Code.v002.System.GameInput;
using UnityEngine;
using Zenject;

namespace _Prototype.Code.v002.Player.Tools
{
    public class PlayerTools : MonoBehaviour
    {
        private Tool _hammer;
        private Tool _hand;
        private Tool _goldPocket;
        private Tool _villagersBook;
        private Tool _buildingsBook;

        private Tool _currentTool;

        [Inject]
        public void Construct(InputManager inputManager)
        {
            _hammer = new Hammer();
            _hand = new FreeHand();
            _goldPocket = new GoldPocket();
            _villagersBook = new VillagersBook();
            _buildingsBook = new BuildingsBook();

            _currentTool = _hand;
        
            _hammer.onInputChange += inputManager.SetState;
            _villagersBook.onInputChange += inputManager.SetState;
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
