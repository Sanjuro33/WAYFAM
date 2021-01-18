using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameTags 
{
    //All of the static variables needed to manage tags and indexes that exist in multiple scripts and are therefore subject to change
     
    //Scene Indexes
    [SerializeField] public static int startScreenIndex = 0;
    [SerializeField] public static int loadingScreenIndex = 1;
    [SerializeField] public static int level1Index = 2;
    [SerializeField] public static int optionsScreenIndex = 3;
    [SerializeField] public static int controlsScreenIndex = 4;
    

    //Options Tags
    [SerializeField] public static string volumeSliderTag = "volumeSlider";
    [SerializeField] public static string fullscreenToggleTag = "fullscreenToggle";

    //Pawn Tags
    [SerializeField] public static string spaceShipTag = "spaceShip";
    [SerializeField] public static string allegienceTagBlue = "blue";
    [SerializeField] public static string allegienceTagRed = "red";
    [SerializeField] public static string allegienceTagNone = "none";


    //Children
    [SerializeField] public static string outerBaseColliderName = "Base Outer";
    [SerializeField] public static string numShipsTextName = "NumShips Text";
    [SerializeField] public static string upgradeIconName = "Upgrade Icon";

    //Canvas Children
    [SerializeField] public static int playerHUDCanvasIndex = 0;
    [SerializeField] public static int endScreenCanvasIndex = 1;

    //Ability Values
    [SerializeField] public static int checkShipSendValue = 1;

}
