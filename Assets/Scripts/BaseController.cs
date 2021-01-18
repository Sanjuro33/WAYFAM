using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
public class BaseController : MonoBehaviour
{
    [Header("Ship Amounts")]
    [SerializeField] int shipCount = 0;
    [SerializeField] int shipsPerGen = 1;
    [SerializeField] int maxShips = 20;
    [SerializeField] GameObject SpaceShipFormation;
    [SerializeField] int numShips = 2;

    [Header("SpaceShipController")]
    [SerializeField] GameObject SpaceShip;
    [SerializeField] GameObject spawnPoint;

    [Header("Components")]
    [SerializeField] Collider innerCollider;
    [SerializeField] UpgradeButton upgradeButton;
    [SerializeField] TextMeshPro numShipsText;

    [Header("Materials")]
    [SerializeField] Material baseMaterial;
    [SerializeField] Material upgradeTowerMaterial;
    [SerializeField] Material genericMaterial;


    [Header("Flag Varaibles")]
    [SerializeField] bool emptyBase = false;
    [SerializeField] bool addingShip = false;
    [SerializeField] bool canGenerateShips = true;
    
    [Header("Base Properties")]
    [SerializeField] string ownerTag;
    [SerializeField] float generationDelay = 2f;
    [SerializeField] float exitSpeed = .01f;
    [SerializeField] CommanderController commanderController;


    [Header("Upgrade Variables")]
    [SerializeField] int numberOfVisibleTowers;
    [SerializeField] int upgradeLevel = 0;
    [SerializeField] List<int> upgradeTowerCounts = new List<int> { 0, 1, 2, 3 };
    [SerializeField] List<int> upgradeCosts = new List<int> { 0, 15, 20, 30 };
    [SerializeField] List<int> capacityUpgrades = new List<int> { 20, 25, 35, 45 };
    [SerializeField] List<float> generationSpeedUpgrades = new List<float> { 2f, 1.6f, 1.4f, 1.3f };
    [SerializeField] bool upgradable;
    [SerializeField] List<Transform> baseSections = new List<Transform>();
   
    // Start is called before the first frame update
    void Start()
    {
        FindComponents();
        checkToGenericBase();
        FindCommanderController();
        FindMaterials();
        AssignMaterials();



        gameObject.tag = ownerTag;
        UpgradeBase();

        
        AssignShipType();
        

    }

    // Update is called once per frame
    void Update()
    {
        UpdateNumShipsText();
        checkToGenericBase();
        GenerateShips();
        UpdateNumShipsText();
        CheckIfUpgradePossible();
        checkToGenericBase();
        //AssignMaterials();
    }

    private void FindComponents()
    {
        upgradeButton = GetComponentInChildren<UpgradeButton>();
        numShipsText = GetComponentInChildren<TextMeshPro>();
        transform.Find(GameTags.outerBaseColliderName).GetComponent<MeshRenderer>().material = baseMaterial;
        baseSections = new List<Transform> (transform.Find(GameTags.outerBaseColliderName).GetComponentsInChildren<Transform>());
        UnityEngine.Debug.Log(baseSections);
    }

    private void RevealTowers(int towersToReveal)
    {
        for(int x = 1; x < baseSections.Count; x++)
        {
            if(x <= towersToReveal)
            {
                baseSections[x].gameObject.SetActive(true);
            }
            else
            {
                baseSections[x].gameObject.SetActive(false);
            }
        }
    }

    private void FindCommanderController()
    {
        UnityEngine.Debug.Log("I know to look for a commander controller");
        foreach(CommanderController controller in FindObjectsOfType<CommanderController>())
        {
            UnityEngine.Debug.Log(controller);
            if(controller.GetCommanderTag() == ownerTag)
            {
                
               commanderController = controller;
               baseMaterial = commanderController.GetMainBaseMaterial();
               upgradeTowerMaterial = commanderController.GetUpgradeTowerMaterial();
            }
        }
    }


    //Checks if the base has enough ships to upgrade. If so, the button becomes avaliable.
    private void CheckIfUpgradePossible()
    {
        //Makes the upgrade button visible and interactable if the player has enough ships to upgrade their base
        if(numShips > upgradeCosts[upgradeLevel])
        {
            upgradeButton.gameObject.SetActive(true);
        }

        //hides the upgrade button if the player doesn't have enough ships to upgrade their base
        else
        {
            upgradeButton.gameObject.SetActive(false);
        }
    }

    //Upgrades the base, activated by the UpgradeButton script
    public void UpgradeBase()
    {
        //Gets rid of the ships "sacrificed" to upgrade the base
        numShips -= upgradeCosts[upgradeLevel];

        //increases the max ship capacity of the base
        maxShips = capacityUpgrades[upgradeLevel];

        //Decreases the amount of time required to generate a ship in the base
        generationDelay = generationSpeedUpgrades[upgradeLevel];

        //Display the amount of towers assoicated with the upgrade
        RevealTowers(upgradeTowerCounts[upgradeLevel]);
        UnityEngine.Debug.Log("I should be revealing " + upgradeTowerCounts[upgradeLevel] + "towers.");
        //Increases the upgradeLevel by 1
        upgradeLevel++;

        UnityEngine.Debug.Log("My current Upgraded level is: " + upgradeLevel);
    }

    public void DegradeBase()
    {
        if (upgradeLevel > 1)
        {
            upgradeLevel -= 2;

            maxShips = capacityUpgrades[upgradeLevel];

            generationDelay = generationSpeedUpgrades[upgradeLevel];

            RevealTowers(upgradeTowerCounts[upgradeLevel]);

            upgradeLevel++;
            UnityEngine.Debug.Log("My current degraded level is: " + upgradeLevel);
        }
    }

    public void FindUpgradeTowers()
    {

    }
    

   
    //Adjusts the number of ships in the base after a ship collides with the base
    public void OnShipCollision(Collider collision)
    {
        UnityEngine.Debug.Log("A ship has entered" + gameObject);
        //If the ship is the same color as the base, add the ship to the base's count
        if (collision.gameObject.GetComponent<SpaceShipController>().GetAllegianceTag() == ownerTag)
        {
            numShips++;
            Destroy(collision.gameObject);
        }

        //If the ship is owned by a different color
        if (collision.gameObject.GetComponent<SpaceShipController>().GetAllegianceTag() != ownerTag)
        {
            //removes a ship from the total number of ships in the base
            numShips--;
            Destroy(collision.gameObject);

            //In the event that another player takes over the base
            if (numShips <= 0)
            {
                //Makes sure that the base never goes into negative ships
                numShips = Mathf.Abs(numShips);
                //calls the Flip Method
                Flip(collision);
            }
        }
    }

    //Constantly checks if the base should be generating a ship
    public void GenerateShips()
    {
        if ((numShips < maxShips) && !addingShip && canGenerateShips)
        {
            //Starts the coroutine to add the ship
            StartCoroutine(AddShip());
        }
    }

    //Adds a ship to the base 
    public IEnumerator AddShip()
    {
        //triggers the adding ship flag to stop the GenerateShips Method from continuously calling the coroutine
        addingShip = true;
        
        //makes sure it's not generic
        if ((baseMaterial != genericMaterial))
        {
            numShips += shipsPerGen;
        }
        
        //pauses and sets the variable back so the couroutine can be called once more
        yield return new WaitForSeconds(generationDelay);
        addingShip = false;
        
    }


    public void SendShips(float percentOfTotal, string allegianceTag, GameObject toBase)
    {
        //create an integer to make sure that there are a certain amount of ships subtracted at the end, representative of the ships generated
        int i = 0;

        //Instantiate a spaceShipFormation to control all of the ships in the base
        GameObject spaceShipFormation = Instantiate(SpaceShipFormation, spawnPoint.transform.position, transform.rotation);

        //Assign the spaceShipFormation to be centered on the base and to have values associated with the base 
        spaceShipFormation.transform.localPosition = spawnPoint.transform.position;
        spaceShipFormation.GetComponent<SpaceShipFormationController>().ReceiveValues(this.gameObject, toBase, exitSpeed);
        
        //execute a for loop to generate a certain number of ships inside of the base
        for (int x = numShips; x > (numShips * (1- percentOfTotal)); x--)
        {
            //Instantiate the ship
            GameObject spaceShip = Instantiate(SpaceShip, spaceShipFormation.transform.position, spaceShipFormation.transform.rotation, spaceShipFormation.transform);
            //set the ship to a center position
            spaceShip.transform.localPosition = new Vector3(0, 0, 0);

            //Assign it it's color and allegiance tag
            

            spaceShip.GetComponent<SpaceShipController>().SetAllegianceTag(allegianceTag);
            //spaceShip.GetComponent<SpaceShipController>().SetShipMaterial(baseMaterial);
            
            //Add the ship to the spaceship formation
            spaceShipFormation.GetComponent<SpaceShipFormationController>().AddShip(spaceShip);
            //spaceShip.transform.LookAt(toBase.transform);
            i++;
        }
        //subtract the total ships generated from the number of ships in the base
        numShips -= i;
        
        //Tell the spaceship controller to exit
        spaceShipFormation.GetComponent<SpaceShipFormationController>().TriggerExit();
    }


    //Flips a base in the event that it is taken over
    public void Flip(Collider collision)
    {
        //Set the base to generic if there are exactly zero ships in the base
        if (numShips == 0)
        {
            setBaseToGeneric();
            


        }

        //Set the base to belong to the player who took it over
        else
        {
            //Sets all of the base's variables to those of the player that took it over
            ownerTag = collision.gameObject.GetComponent<SpaceShipController>().GetAllegianceTag();
            gameObject.tag = ownerTag;
            FindCommanderController();
            FindMaterials();

            AssignMaterials();
            AssignShipType();
            //baseMaterial = collision.gameObject.GetComponent<SpaceShipController>().GetMaterial();
            //print(baseMaterial); 
    
            DegradeBase();
        }
    }

    private void FindMaterials()
    {
        if (gameObject.tag != GameTags.allegienceTagNone)
        {
            baseMaterial = commanderController.GetMainBaseMaterial();
            upgradeTowerMaterial = commanderController.GetUpgradeTowerMaterial();
        }
        else
        {
            baseMaterial = genericMaterial;
            upgradeTowerMaterial = genericMaterial;
        }
        
    }
    private void AssignMaterials()
    {
        


            UnityEngine.Debug.Log(baseMaterial);
            baseSections[0].GetComponent<MeshRenderer>().material = baseMaterial;
            baseSections[1].GetComponent<MeshRenderer>().material = upgradeTowerMaterial;
            baseSections[2].GetComponent<MeshRenderer>().material = upgradeTowerMaterial;
        
    }

    private void AssignShipType()
    {
        if(gameObject.tag != GameTags.allegienceTagNone)
        SpaceShip = commanderController.GetShipType();
    }

    //Causes the material of the outer base model to be the color of the player that owns it
    public void AssignBaseMaterial(Material material)
    {
        transform.Find(GameTags.outerBaseColliderName).GetComponent<MeshRenderer>().material = material;
    }

    
    //Checks if the base has been predefined as an empty base
    public void checkToGenericBase()
    {
        //checks for emptyBase variable
        if(emptyBase)
        {
            //Call method to make it a generic base
            setBaseToGeneric();
            //Set it so the method does not infinitely loop
            emptyBase = false;
        }
    }

    //Sets the base to a generic base
    public void setBaseToGeneric()
    {
        //Sets all of the variables to that of a generic base
        ownerTag = GameTags.allegienceTagNone;
        gameObject.tag = ownerTag;
        FindMaterials();
        UnityEngine.Debug.Log("The base material of this object is " + baseMaterial);
        AssignMaterials();
    }

//Accessors and Mutators

    //Returns the number of ships inside of the base
    public int GetNumShips()
    {
        return numShips;
    }

    //Returns the max ships that the base can hold
    public int GetMaxShips()
    {
        return maxShips;
    }

    //Sets whether or not the base can generate ships
    public void SetGeneratingShips(bool isGenerating)
    {
        canGenerateShips = isGenerating;
    }

    //returns the current cost to upgrade the base
    public int GetUpgradeCost()
    {
        return upgradeCosts[upgradeLevel];
    }

    //Sets the value of the text on the base's numShips display
    public void UpdateNumShipsText()
    {
        numShipsText.text = numShips.ToString();
    }
}
