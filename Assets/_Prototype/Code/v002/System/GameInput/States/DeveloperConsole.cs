using UnityEngine;

namespace _Prototype.Code.v002.System.GameInput.States
{
    /// <summary>
    /// Input state responsible for handling developer input for developer console
    /// </summary>
    public class DeveloperConsole : IInputState
    {
        public void OnStateSet()
        {
            
        }

        public void HandleState(InputManager inputManager)
        {
            if (Input.GetKeyDown(inputManager.Console)) 
                v001.DeveloperTools.Console.DeveloperConsole.I.ToggleConsole();
            
            if (!v001.DeveloperTools.Console.DeveloperConsole.I.IsConsoleActive()) return;

            if (Input.GetKeyDown(inputManager.Accept)) 
                v001.DeveloperTools.Console.DeveloperConsole.I.GetCommand();
        }

        public void OnStateChange()
        {
            
        }
    }
}
