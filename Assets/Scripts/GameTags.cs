using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameTags 
{
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
    [SerializeField] public static string allegienceTagFriend = "friend";
    [SerializeField] public static string allegienceTagFoe = "foe";
 }
