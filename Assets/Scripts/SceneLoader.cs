using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    //Loads the Start Screen
    public void LoadStartScreen()
    {
        SceneManager.LoadScene(GameTags.startScreenIndex);
    }

    //Loads the Loading Screen
    public void LoadLoadingScreen()
    {
        SceneManager.LoadScene(GameTags.loadingScreenIndex);
    }

    //Loads the first level
    public void LoadLevel1()
    {
        SceneManager.LoadScene(GameTags.level1Index);
    }

    //Loads the options screen
    public void LoadOptionsScreen()
    {
        SceneManager.LoadScene(GameTags.optionsScreenIndex);
    }

    //Loads the controls screen
    public void LoadControlScreen()
    {
        SceneManager.LoadScene(GameTags.controlsScreenIndex);
    }

    //Quits the game
    public void QuitGame()
    {
        Application.Quit();
    }


}
