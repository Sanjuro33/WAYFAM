using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Options : MonoBehaviour
{
    //Options Variables
    [SerializeField] float volume = 4f;
    [SerializeField] bool fullscreen = false;

    //Canvas Components
    [SerializeField] Slider volumeSlider;
    [SerializeField] Toggle fullscreenToggle;

    // Start is called before the first frame update
    void Start()
    {
        
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().buildIndex == GameTags.optionsScreenIndex)
        {

            volumeSlider = GameObject.FindWithTag(GameTags.volumeSliderTag).GetComponent<Slider>();
            fullscreenToggle = GameObject.FindWithTag(GameTags.fullscreenToggleTag).GetComponent<Toggle>();

            volumeSlider.value = volume;
            fullscreenToggle.isOn = fullscreen;



        }
    }
    // Update is called once per frame
    void Update()
    {
        
        UpdateValues();

    }

    

    void SetUpSliderVariables()
    {
        
        
    }
    void UpdateValues()
    {
        if (SceneManager.GetActiveScene().buildIndex == GameTags.optionsScreenIndex)
        {
            volume = volumeSlider.value;
            fullscreen = fullscreenToggle.isOn;
        }
    }
}
