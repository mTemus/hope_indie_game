using _Prototype.Code.v002.System.GameInput;
using UnityEngine;

namespace _Prototype.Code.v002.System
{
    /// <summary>
    /// Main manager of the game
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private InputManager _inputManager;

        private float _timeSpeed = 1f;

        private void Update()
        {
            _inputManager.ManualUpdate(_timeSpeed);
        }
    }
}
