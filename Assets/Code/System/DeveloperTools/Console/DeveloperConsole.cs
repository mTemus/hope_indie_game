using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Code.System.DeveloperTools.Console
{
    public class DeveloperConsole : MonoBehaviour
    {
        [Header("UI Parts")] 
        [SerializeField] private GameObject console;
        [SerializeField] private TMP_InputField commandInputField;
        [SerializeField] private TextMeshProUGUI commandPlaceholderField;

        [Header("Colors")] 
        [SerializeField] private Color normalTextColor;
        [SerializeField] private Color noCommandColor;
        
        [Header("Commands")] 
        [SerializeField] private ConsoleCommandData[] commands;

       public static DeveloperConsole I { get; private set; }

        private void Awake()
        {
            commandInputField.onSelect.AddListener(ResetPlaceholderText);
            console.SetActive(false);
            I = this;
        }

        private void ResetPlaceholderText(string s)
        {
            commandPlaceholderField.text = "Enter command: ";
            commandPlaceholderField.color = normalTextColor;
        }
        
        public void ToggleConsole()
        {
            if (IsConsoleActive()) {
                Time.timeScale = 1f;
                commandInputField.text = String.Empty;
                console.SetActive(false);
            }
            else {
                Time.timeScale = 0f;
                console.SetActive(true);
                commandInputField.Select();
            }
        }
        
        public void GetCommand()
        {
            string rawCommand = commandInputField.text;

            if (!rawCommand.StartsWith("/")) return;
            string[] command = rawCommand.Remove(0, 1).Split(' ');

            foreach (ConsoleCommandData commandData in commands) {
                if (commandData.Command != command[0]) continue;
                if (commandData.Process(command.Skip(1).ToArray())) {
                        
                    //TODO: add command to history list
                    break;
                }

                ReturnWrongCommand("No such command!");
                break;


            }

            commandInputField.text = string.Empty;
            commandInputField.Select();
        }

        public void ReturnWrongCommand(string answerText)
        {
            commandInputField.text = string.Empty;
            commandPlaceholderField.text = answerText;
            commandPlaceholderField.color = noCommandColor;
        }
        
        public bool IsConsoleActive() =>
            console.activeSelf;

    }
}
