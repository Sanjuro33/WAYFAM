using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIController : MonoBehaviour
{
    [Header("Player UI Elements")]
    [SerializeField] Button percentButton100;
    [SerializeField] Button percentButton75;
    [SerializeField] Button percentButton50;
    [SerializeField] Button percentButton25;
    [SerializeField] Slider troopSlider;
    [SerializeField] List<GameObject> abilityButtons;
   
//Custom Methods
    //Returns a list of all of the ability buttons
    public List<GameObject> GetButtons()
    {
        return abilityButtons;
    }

   
}
