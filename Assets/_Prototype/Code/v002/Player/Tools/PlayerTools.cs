using _Prototype.Code.v002.System.GameInput;
using UnityEngine;
using Zenject;

namespace _Prototype.Code.v002.Player.Tools
{
    /// <summary>
    /// Class responsible for handling user tools
    /// </summary>
    public class PlayerTools : MonoBehaviour
    {
        private Tool _hammer;
        private Tool _hand;
        private Tool _goldPocket;
        private Tool _villagersBook;
        private Tool _buildingsBook;

        private Tool _currentTool;

        [Inject]
        private void Construct(InputManager inputManager)
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
        
        /// <summary>
        /// Use current selected tool and invoke its properties
        /// </summary>
        public void UseCurrentTool()
        {
            _currentTool.UseTool();
        }
        
        /// <summary>
        /// Set player tool that should be selected and be invoked on use
        /// </summary>
        /// <param name="toolIndex">Tool int id (should be handled by enum to have more control)</param>
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
