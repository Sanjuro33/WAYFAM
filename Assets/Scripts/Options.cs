using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Options : MonoBehaviour
{
    //Options Variables
    [Header("Option Variables")]
    [SerializeField] float volume = 4f;
    [SerializeField] bool fullscreen = false;

    //Canvas Components
    [Header("Canvas Components")]
    [SerializeField] Slider volumeSlider;
    [SerializeField] Toggle fullscreenToggle;

//Main Methods
    // Start is called before the first frame update
    void Start()
    {
        
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    
    // Update is called once per frame
    void Update()
    {
        
        UpdateValues();

    }

//Custom Methods

    //When the scene is loaded Sets the variables on the options screens to the values stored in the object
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().buildIndex == GameTags.optionsScreenIndex)
        {
            //Find the components of the options menu
            volumeSlider = GameObject.FindWithTag(GameTags.volumeSliderTag).GetComponent<Slider>();
            fullscreenToggle = GameObject.FindWithTag(GameTags.fullscreenToggleTag).GetComponent<Toggle>();

            //Set the components to their values
            volumeSlider.value = volume;
            fullscreenToggle.isOn = fullscreen;



        }
    }

    //Update the variables in the controller to reflect the sliders on the screen
    void UpdateValues()
    {
        if (SceneManager.GetActiveScene().buildIndex == GameTags.optionsScreenIndex)
        {
            volume = volumeSlider.value;
            fullscreen = fullscreenToggle.isOn;
        }
    }
}
