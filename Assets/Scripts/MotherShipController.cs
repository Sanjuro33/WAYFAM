using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotherShipController : MonoBehaviour
{
    
    [Header("Team Attributes")]
    [SerializeField] string allegianceTag = GameTags.allegienceTagBlue;
    [SerializeField] Material teamMaterial;
 

//Main Methods

    public  string GetAllegianceTag()
    {
        return allegianceTag;
    }

    public Material GetTeamMaterial()
    {
        return teamMaterial;
    }
}
