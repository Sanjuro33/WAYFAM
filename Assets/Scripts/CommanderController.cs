using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommanderController : MonoBehaviour
{
    [Header("Commander Attributes")]
    [SerializeField] string commanderTag;
    [SerializeField] int maxTroops;
    [SerializeField] bool isEnemy;
    [SerializeField] EnemyAI enemyAI;
    [SerializeField] GameObject ShipType;

    [Header("Troop Counts")]
    [SerializeField] int totalTroops;
    [SerializeField] List<BaseController> ownedBases;
    [SerializeField] List<SpaceShipController> deployedShips;

    [Header("World Info")]
    [SerializeField] List<BaseController> neutralBases;
    [SerializeField] MotherShipController commanderMothership;

    [Header("Textures")]
    [SerializeField] Material mainBaseMaterial;
    [SerializeField] Material upgradeTowerMaterial;
    [SerializeField] Material spaceshipMaterial;
   
//Main Methods
    // Start is called before the first frame update
    void Start()
    {
        FindMothership();
        FindAI();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateValues();
        ControlTroopGeneration();
    }

//Custom Methods

    //Finds the AI for the controller if it's marked as an enemy
    private void FindAI()
    {
        //Checks if it is an enemy
        if (isEnemy = true)
        {
            //Searches for enemy AIs
            List<EnemyAI> AIs = new List<EnemyAI>(FindObjectsOfType<EnemyAI>());
            foreach (EnemyAI AI in AIs)
            {
                //chooses the one that matches it's color
                if (AI.GetAllegianceTag() == commanderTag)
                {
                    enemyAI = AI;
                    AI.SetCommanderController(this);
                }
            }
        }
    }

    //Finds the mothership that is associated with this commander's color
    private void FindMothership()
    {
        //Finds all motherships
        List<MotherShipController> motherships = new List<MotherShipController>( FindObjectsOfType<MotherShipController>() );
        foreach (MotherShipController mothership in motherships)
        {
            //chooses the one with it's color
            if(mothership.GetAllegianceTag() == commanderTag)
            {
                commanderMothership = mothership;
            }
        }
    }

    //searches all of the bases in the map to fill variables
    //TODO: add the ability to search for all enemy bases
    private void FindBases()
    {
        //Sets up placeholder variables
        int maxTroopsInBases = 0;
        List<BaseController> bases = new List<BaseController>(FindObjectsOfType<BaseController>());
        List<BaseController> tagBases = new List<BaseController>();
        List<BaseController> noneBases = new List<BaseController>();

        //searches all bases on the map
        foreach(BaseController controller in bases)
        {
            //collects the bases that share the commander's color tag
            if(controller.gameObject.tag == commanderTag)
            {
                tagBases.Add(controller);
                maxTroopsInBases += controller.GetMaxShips();
            }
            //collects all of the neutral bases
            if(controller.gameObject.tag == GameTags.allegienceTagNone)
            {
                noneBases.Add(controller);
            }
        }
        //Assigns the placeholder variables to be the global variables
        maxTroops = maxTroopsInBases;
        ownedBases = tagBases;
        neutralBases = noneBases;
    }

    //Searches for all of the ships and selects those that belong to the commander
    private void FindShips()
    {
        //Set Up Placeholder variables
        List<SpaceShipController> ships = new List<SpaceShipController>(FindObjectsOfType<SpaceShipController>());
        List<SpaceShipController> tagShips = new List<SpaceShipController>();

        //search all ships on the map
        foreach (SpaceShipController controller in ships)
        {
            //selects those that share it's allegiance tag
            if (controller.GetAllegianceTag() == commanderTag)
            {
                tagShips.Add(controller);
            }
        }
        //Assign the placeholder variable to the global variable
        deployedShips = tagShips;

    }

    //Call other methods to update the values in the system
    private void UpdateValues()
    {
        FindBases();
        FindShips();

        //Get the total number of ships owned by the player
        //TODO: break this off into it's own method
        int ships = 0;
        foreach (BaseController controller in ownedBases)
        {
            ships += controller.GetNumShips();
        }

        totalTroops = ships + deployedShips.Count;

    }

    //Make sure the player stops generating troops if they reach capacity
    private void ControlTroopGeneration()
    {
        //If the player has the max troops tell their bases to stop generating
        if (totalTroops >= maxTroops)
        {
            foreach (BaseController controller in ownedBases)
            {
                controller.SetGeneratingShips(false);
            }



        }
        //If the player is not at max troops, tell their bases to keep generating
        else
        {
            foreach(BaseController controller in ownedBases)
            {
                controller.SetGeneratingShips(true);
            }
        }
    }

//Accessors and Mutators

    public string GetCommanderTag()
    {
        return commanderTag;
    }
    //Return the total amount of troops owned by the commander
    public int GetTotalTroops()
    {
        return totalTroops;
    }

    //Return the list of bases owned by the commander
    public List<BaseController> GetOwnedBases()
    {
        return ownedBases;
    }

    public Material GetMainBaseMaterial()
    {
        return mainBaseMaterial;
    }

    public Material GetUpgradeTowerMaterial()
    {
        return upgradeTowerMaterial;
    }

    public Material GetSpaceshipMaterial()
    {
        return spaceshipMaterial;
    }

    public GameObject GetShipType()
    {
        return ShipType;
    }
}
