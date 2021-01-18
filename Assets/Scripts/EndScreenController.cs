using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class EndScreenController : MonoBehaviour
{
    [Header("GameAttributes")]
    [SerializeField] bool playerWon;
    [SerializeField] TextMeshProUGUI endText;

    [Header("Endscreen Values")]
    [SerializeField] string winTextValue = "YOU WIN!";
    [SerializeField] string loseTextValue = "YOU LOSE";
    // Start is called before the first frame update

//Main Methods

    void Start()
    {
        endText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        gameObject.SetActive(false);
    }

//Custom Methods

    //Allows the EndScreenController to get statistics and the result of the game
    public void ReceiveValues(bool playerWon)
    {
        UnityEngine.Debug.Log("I have received values");
        this.playerWon = playerWon;
        ActivateEndScreen();
    }

    //Activates the end screen
    void ActivateEndScreen()
    {
        //Player victory
        if (playerWon)
        {
            endText.text = winTextValue;
        }
        //Player defeat
        else
        {
            endText.text = loseTextValue;
        }
    }
}
