using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CommandsHandler
{
    public event EventHandler<bool> ModeChanged;

    private bool isSingleMode;

    public bool IsModeChanged
    {

        get => isSingleMode;
        set
        {
            isSingleMode = value;
            ModeChanged?.Invoke(null, isSingleMode);

        }
    }


    public void HandleState(GameCommands gameCommand)
    {
        switch (gameCommand)
        {
            case GameCommands.MainMenu:
            
                break;
            case GameCommands.GamePlay:


                break;

            case GameCommands.GameWin:

           

                break;

            case GameCommands.MultiPlayerMode:

                ModeChanged?.Invoke(null, false);
                break;

            case GameCommands.SinglePlayerMode:

                ModeChanged?.Invoke(null, true);
                break;

          

            case GameCommands.Restart:
                Restart();
                break;
            case GameCommands.QuitGame:
                EndGame();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(gameCommand));
        }
    }

    private static void Restart()
    {
        ResourceHolder.Reset();
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    private void EndGame()
    {

        Application.Quit();
    }
}