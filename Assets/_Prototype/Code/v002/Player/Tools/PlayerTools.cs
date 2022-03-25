using System;
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
        /// <param name="toolType"> Enum of tool type that will be converted to tool object </param>
        public void SelectTool(PlayerTool toolType)
        {
            _currentTool = toolType switch {
                PlayerTool.Hand => _hand,
                PlayerTool.Hammer => _hammer,
                PlayerTool.VillagersBook => _villagersBook,
                PlayerTool.BuildingsBook => _buildingsBook,
                PlayerTool.CoinPocket => _goldPocket,
                _ => throw new ArgumentOutOfRangeException(nameof(toolType), toolType, null)
            };

            Debug.LogWarning(_currentTool + " selected.");
        }
    }
}
