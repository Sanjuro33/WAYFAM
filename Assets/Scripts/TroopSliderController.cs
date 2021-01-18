using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TroopSliderController : MonoBehaviour
{
    [Header("Canvas Elements")]
    [SerializeField] Slider troopSlider;
    [SerializeField] TextMeshProUGUI playerTroopsText;
    [SerializeField] TextMeshProUGUI enemyTroopsText;


//Main Methods

    // Start is called before the first frame update
    void Start()
    {
        //Find the components
        FindComponents();
    }

//Custom Methods

    //Finds all the components in the object and it's children
    public void FindComponents()
    {
        troopSlider = gameObject.GetComponent<Slider>();
        playerTroopsText = transform.GetChild(0).transform.gameObject.GetComponent<TextMeshProUGUI>();
        enemyTroopsText = transform.GetChild(1).transform.gameObject.GetComponent<TextMeshProUGUI>();
    }

    //Updates the troopsSlider to reflect the troops in the game
    public void UpdateTroopSlider(int totalGameTroops, int playerTroops)
    {
        //Updates the slider values to reflect the state of the game
        troopSlider.maxValue = totalGameTroops;
        troopSlider.value = playerTroops;

        //Updates the slider text to reflect the state of the game
        playerTroopsText.text = playerTroops.ToString();
        enemyTroopsText.text = (totalGameTroops - playerTroops).ToString();
    }
}
