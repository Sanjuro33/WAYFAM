using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class SpaceShipFormationController : MonoBehaviour
{
    [Header("Flag Variables")]
    [SerializeField] bool exiting;
    [SerializeField] bool moving;
    [SerializeField] bool entering;
    [SerializeField] bool rotating;
    [SerializeField] bool troopsOrdered;

    [Header("Base")]
    [SerializeField] GameObject fromBase;
    [SerializeField] GameObject toBase;

    [Header("Movement Variables")]
    [SerializeField] float moveSpeed = .001f;
    [SerializeField] Vector3 _direction;
    [SerializeField] float aboveBaseDistance = 2;

    [Header("Ship Variables")]
    [SerializeField] List<GameObject> ships = new List<GameObject>();
    [SerializeField] List<Vector3> formPositions = new List<Vector3>();
    
    [Header("Rotation variables")]
    [SerializeField] Vector3 dir;
    [SerializeField] Quaternion lookRot;
    [SerializeField] float angle;
    [SerializeField] float RotationSpeed;
    [SerializeField] Quaternion _lookRotation;
    //values that will be set in the Inspector

//Main Methods

    //Start is called before the first frame update
    void Start()
    {
        GenerateFormation(5);
       
    }

    //Called every physics frame
    void FixedUpdate()
    {
        MovementLoop();
    }

//Custom Methods

    //Makes the controller move as flag variables are activated
    public void MovementLoop()
    {
        if (exiting)
        {

            MoveOut();
        }

        if (rotating)
        {
            RotateToFaceBase();
        }

        if (moving)
        {
            GoToBase();
        }

        if (entering)
        {
            MoveIn();
        }
    }

    //Reveives values from the base that spawns it
    public void ReceiveValues(GameObject fromBase, GameObject toBase, float moveSpeed )
    {
        this.fromBase = fromBase;
        this.toBase = toBase;
        this.moveSpeed = moveSpeed;
    }

    //Adds a ship to the formation
    public void AddShip(GameObject spaceShip)
    {
        ships.Add(spaceShip);
    }

    //Sets exiting to true
    public void TriggerExit()
    {
        exiting = true;
    }

    //Spawns the ships and moves them into position
    void MoveOut()
    {
        //Creates a list of the positions for the ships to form into
        GenerateFormation(ships.Count);

        //Sets the ships to be ready until the pass
        var allReady = true;

        //Moves the spaceship controller to the desired position
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(fromBase.transform.position.x, fromBase.transform.position.y + aboveBaseDistance, fromBase.transform.position.z), moveSpeed);
        int i = 0;

        //TODO: Find out why this line exists
        if(i > ships.Count) { i = 0; }
        
        //Goes through all of the ships and makes sure that they have left the base and gotten into position
        foreach (GameObject ship in ships)
            {
                

                //Tells the ships to leave the base
                ship.GetComponent<SpaceShipController>().LeaveBase(transform, new Vector3(transform.localPosition.x + formPositions[i].x, transform.localPosition.y, transform.localPosition.z + formPositions[i].z), moveSpeed);
                
                //Only allows for allReady to be true if all of the ships are in position
                if (ship.transform.position != new Vector3(transform.localPosition.x + formPositions[i].x, transform.localPosition.y, transform.localPosition.z + formPositions[i].z))
                {
                    
                    allReady = false;
                }
                i ++;
            }
        
        //Continues to the next stage of movement if all the ships are in position
        if (allReady == true)
        {
        //Sets up variables for rotation so it's only done once
        SetUpRotationVariables();

        //Triggers the flag for the next stage
        rotating = true;

        //Turns off the flag for the current stage
        exiting = false;

        }
       

    }

    //Sets up the variables for the rotation stage
    void SetUpRotationVariables()
    {
        dir = toBase.transform.position - transform.position;
        dir.y = 0;
        lookRot = Quaternion.LookRotation(dir);
        lookRot = lookRot *= Quaternion.Euler(0, 270, 0);
        
    }


    //Rotates the ships to face the base that they're heading for
    void RotateToFaceBase()
    {
        //Makes the ships rotate towards the base
        transform.rotation = Quaternion.RotateTowards(transform.localRotation, lookRot, 1f);
       
        //When the rotation is approximately accurate:
        if (Quaternion.Angle(transform.rotation, lookRot) < .05f)
        {
            //Triggers the flag for the next stage
            moving = true;

            //Turns off the flag for the current stage
            rotating = false;
        }
         
    }

    //Moves the FormationController towads the toBase
    void GoToBase()
    {

        //Moves the ship to the location of the toBase
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(toBase.transform.position.x, toBase.transform.position.y + 1, toBase.transform.position.z), moveSpeed);

        //When the formation is within an acceptable radius of the toBase
        if (Vector3.Distance(transform.position, toBase.transform.position) < 3)
        {
            //Triggers the flag for the next stage
            entering = true;

            //Turns off the flag for the current stage
            moving = false;
            
        }
    }

    //Moves the ships in the formation into the toBase
    void MoveIn()
    {
        //for all of the ships in the controller
        for(int x = 0; x < ships.Count; x++)
        {
            //If the ship exists, have it enter the base
            if (ships[x] != null)
            {
                ships[x].GetComponent<SpaceShipController>().EnterBase(toBase.transform.position, moveSpeed);
            }

            //IF the ship doesn't exist, remove it from the list
            else
            {
                ships.Remove(ships[x]);
            }
        }
        //If there are no more ships in the controller
        if(ships.Count == 0)
        {
            //End the movement loop and destroy the FormationController
            entering = false;
            Destroy(gameObject);
        }
        
    }

    //Generates a series of positions to put ships in for the animation
    void GenerateFormation(int numShips)
    {
        //Set up a number of ships that have already been generated
        int n = 0;
        int r = 0;
        //Set up a for loop to continue generating ships while the total number of ships generated by the method is the same as the number of ships requested
        while (n < numShips)
        { 

                if (r % 2 != 0)
                {
                    //The max placement distance to each side is half the total number of ships already generated
                       
                    float maxDist = r / 2;

                    
                    for (float i = maxDist; i >= -maxDist; i--)
                    {
                        n++;
                        if (n > numShips) { break; }
                        formPositions.Add(new Vector3((-1 * r), 0, i ));
                        

                    }
                }
                else if (r % 2 == 0)
                {
                       
                    float maxDist = r / 2;
                    for (float i = maxDist; i > -maxDist; i--)
                    {
                        n++;
                        if (n > numShips) { break; }
                        formPositions.Add(new Vector3((-1 * r), 0, (i - .5f)));

                    }
                }
                r++;
            
            

            
        }
        formPositions = formPositions.Distinct().ToList();

    }

//Utility Methods

    //Rounding without midpoint mistakes
    public static float MyRound(float value)
    {
        if (value % 0.5f == 0)
            return Mathf.Ceil(value);
        else
            return Mathf.Round(value);
    }
}
