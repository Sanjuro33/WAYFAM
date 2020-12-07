using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadStartScreen()
    {
        SceneManager.LoadScene(GameTags.startScreenIndex);
    }

    public void LoadLoadingScreen()
    {
        SceneManager.LoadScene(GameTags.loadingScreenIndex);
    }

    public void LoadLevel1()
    {
        SceneManager.LoadScene(GameTags.level1Index);
    }

    public void LoadOptionsScreen()
    {
        SceneManager.LoadScene(GameTags.optionsScreenIndex);
    }

    public void LoadControlScreen()
    {
        SceneManager.LoadScene(GameTags.controlsScreenIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }


}
