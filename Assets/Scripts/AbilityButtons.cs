using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityButtons : MonoBehaviour
{
    //Variables for the indexes of the buttons and their respective ability values
    [Header("Indexes")]

    [SerializeField] int buttonIndex;
    [SerializeField] int abilityIndex;

    //Variables for generating ships, used in the SendShips method
    [Header("Ships")]

    [SerializeField] GameObject SpaceShipController;
    [SerializeField] GameObject SpaceShip;
    [SerializeField] float exitSpeed = .01f;
    [SerializeField] List<GameObject> dispatchShips = new List<GameObject> { };
    [SerializeField] float shipScale;
    //Variables used to reference the current player
    [Header("Player Variables")]

    [SerializeField] MotherShipController player;
    [SerializeField] float delay = 3f;
    //Variables used to get input from the buttons on the bottom of the screen
    [Header("Buttons")]

    [SerializeField] List<GameObject> abilityButtons;
    [SerializeField] bool abilitySelected;
// TODO: Set variables up in GameTags to change these numbers to easily resettable variables
    [SerializeField] List<int> abilityValues = new List<int> { 1, 2, 3, 4, 5 };
    [SerializeField] ClickController clickController;
 
    
    

//Main Methods
    // Start is called before the first frame update
    void Start()
    {
        clickController = gameObject.GetComponent<ClickController>();
        abilityButtons = FindObjectOfType<UIController>().GetButtons();
        player = FindObjectOfType<MotherShipController>();
        // TODO: Set variables up in GameTags to change these numbers to easily resettable variables
        abilityValues = new List<int> { GameTags.checkShipSendValue, 2, 3, 4, 5 };
        shipScale = 0.5f;

    }

    // Update is called once per frame
    void Update()
    {
        CheckForButtonClick();
        MoveShips();
    }


//Custom Methods

    /// <summary>
    /// In order to assign a value to a button, the value of the ability on "Ability Values" must have the same index as the button on the "Ability Buttons" list.
    /// Then, a method needs to be created that has that value as a requirement for the method to execute. The button should reset the value and refresh
    /// based on an assigned delay of time. Different Ability values can be found and should be stored in the GameTags script
    /// </summary>

//Button Methods

    //Checks to see if the button has been clicked
    void CheckForButtonClick()
    {
        //Check the position of the mouse on the screen
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit);
        //Return the collider that the ray hits
        UnityEngine.Debug.Log(hit.collider);


        //If the left mouse button is pressed, check to see if a button has been clicked
        if (Input.GetButtonDown("Fire1"))
        {

            CheckButtonClick(hit);

            
        }

        //Also call CheckShipSend because it needs to be checked for a right mouse button click
        CheckShipSend();
    }

    //Makes sure that only one of the buttons is highlighted on the display
    void IsolateButton()
    {
        //Go through each of the ability buttons
        foreach (GameObject button in abilityButtons)
        {
            //If the button is not the currently selected button
            if (buttonIndex != abilityButtons.IndexOf(button))
            {
                //Set the color of the unselected button to grey
                button.GetComponent<Image>().color = Color.white;
            }
            else
            {
                //Set the color of the selected button to white
                
                button.GetComponent<Image>().color = new Color32(56, 56, 56, 255);
            }
        }
    }

   

    public IEnumerator RechargeAbility(int buttonIndex, float delayTime)
    {
        abilityButtons[buttonIndex].GetComponent<Image>().color = Color.white;
        abilityButtons[buttonIndex].SetActive(false);
       
        abilityIndex = 0;
        yield return new WaitForSeconds(delayTime);
        abilityButtons[buttonIndex].SetActive(true);
    }

    //Checks if the player has clicked on a button and then changes the selected ability
    void CheckButtonClick(RaycastHit hit)
    {
        //makes sure the collider that the mouse is over is in the list of buttons
        if (abilityButtons.Contains(hit.collider.gameObject))
        {
            if(hit.collider.gameObject == abilityButtons[buttonIndex])
            {
                abilityIndex = 0;
                IsolateButton();
            }

            //Changes the index of the selected button to the one clicked
            buttonIndex = abilityButtons.IndexOf(hit.collider.gameObject);
            //Graphically isolates the button
            IsolateButton();
            //Changes the selected ability index to abilityValues[buttonIndex]
            SetAbilityIndex(buttonIndex);
        }
    }

//Send Ships Ability Methods
    //Checks to see if the player has both selected SendShips as their ability and has right clicked on a base they wish to send it to
    void CheckShipSend()
    {
        //Makes sure that the ability index for SendShips is selected
        //TODO: Set variables up in GameTags to change these numbers to easily resettable variables
        if (abilityIndex == GameTags.checkShipSendValue && Input.GetButtonDown("Fire2"))
        {
            //Checks for the base t
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit);
            GameObject toBase = hit.collider.gameObject;

            //Makes sure that it's sending ships to a base and not a different GameObject
            if (toBase.transform.parent.GetComponent<BaseController>() != null)
            {
                SendShips(toBase);
                StartCoroutine(RechargeAbility(abilityValues.IndexOf(abilityIndex), delay));
            }

        }
    }
    void SendShips(GameObject toBase)
    {
        //Sends a message of the base that it is 
        //UnityEngine.Debug.Log(toBase);


        //Generate SpaceShipFormation controller
        //GameObject spaceShipFormation = Instantiate(SpaceShipController, player.transform.position, player.transform.rotation);

        //Set the values of the spaceShipFormation controller
        //spaceShipFormation.transform.localPosition = player.transform.position;
        //spaceShipFormation.GetComponent<SpaceShipFormationController>().ReceiveValues(player.gameObject, toBase, exitSpeed);

        int numShips = Random.Range(1, 3);
        //Generate all of the ships for the ship formation controller
        for (int x = numShips; x > 0; x--)
        {
            //Instantiate the ship
            GameObject spaceShip = Instantiate(SpaceShip, player.transform.position, player.transform.rotation);
            //set the ship to a center position
            
            //Set up variables to assingn to spaceship controller
            string allegianceTag = player.GetAllegianceTag();
            Material baseMaterial = player.GetTeamMaterial();

            //Assign it it's color and allegiance tag
            spaceShip.GetComponent<SpaceShipController>().SetAllegianceTag(allegianceTag);
            //aaaaaaaaaaaaaaspaceShip.GetComponent<SpaceShipController>().SetShipMaterial(baseMaterial);
            spaceShip.transform.LookAt(toBase.transform);
            spaceShip.transform.rotation *= Quaternion.Euler(0, -90, 0);
            spaceShip.transform.localScale = new Vector3(shipScale, shipScale, shipScale);
            spaceShip.GetComponent<SpaceShipController>().SetToBase(toBase);
            dispatchShips.Add(spaceShip);
            //Add the ship to the spaceship formation
            //paceShipFormation.GetComponent<SpaceShipFormationController>().AddShip(spaceShip);
            //spaceShip.transform.LookAt(toBase.transform);

        }
        //Sends the spaceships away from the mothership and to the clicked base
        //GetComponent<SpaceShipFormationController>().TriggerExit();
    }

    void MoveShips()
    {
        if(dispatchShips.Count != 0)
        {
            foreach(GameObject ship in dispatchShips)
            {
                if (ship != null)
                {
                    ship.GetComponent<SpaceShipController>().EnterBase(ship.GetComponent<SpaceShipController>().GetToBase().transform.position, exitSpeed);
                }

                else
                {
                    dispatchShips.Remove(ship);
                }
            
            }
        }
    }

//Accessors and Mutators

    //Sets the value of abilityIndex to the value in ability Values at the index of the button pressed
    void SetAbilityIndex(int index)
    {
        abilityIndex = abilityValues[index];
    }
}
