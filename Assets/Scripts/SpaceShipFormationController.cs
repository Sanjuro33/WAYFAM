using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class SpaceShipFormationController : MonoBehaviour
{
    [SerializeField] GameObject fromBase;
    [SerializeField] GameObject toBase;
    [SerializeField] float moveSpeed = .001f;
    [SerializeField] bool exiting;
    [SerializeField] bool moving;
    [SerializeField] bool entering;
    [SerializeField] bool rotating;
    [SerializeField] bool troopsOrdered;
    [SerializeField] List<GameObject> ships = new List<GameObject>();
    [SerializeField] List<Vector3> formPositions = new List<Vector3>();
    [SerializeField] float RotationSpeed;
    [SerializeField] Quaternion _lookRotation;
    [SerializeField] Vector3 _direction;

    //Rotation Variables
    [SerializeField] Vector3 dir;
    [SerializeField] Quaternion lookRot;
    [SerializeField] float angle;
    //values that will be set in the Inspector

    // Start is called before the first frame update
    void Start()
    {
        GenerateFormation(5);
        //PrintFormPositions();
    }

    public void ReceiveValues(GameObject fromBase, GameObject toBase, float moveSpeed )
    {
        this.fromBase = fromBase;
        this.toBase = toBase;
        this.moveSpeed = moveSpeed;
    }

    public void AddShip(GameObject spaceShip)
    {
        ships.Add(spaceShip);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        if(exiting)
        {
            
            MoveOut();
        }

        if(rotating)
        {
            RotateToFaceBase();
        }
        
        if(moving)
        {
            GoToBase();
        }

        if(entering)
        {
            MoveIn();
        }
    }
    
    public void TriggerExit()
    {
        exiting = true;
    }

    void MoveOut()
    {
        GenerateFormation(ships.Count);
        //UnityEngine.Debug.Log(formPositions.Count);
        var allReady = true;
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(fromBase.transform.position.x, fromBase.transform.position.y + 1, fromBase.transform.position.z), moveSpeed);
        int i = 0;
        if(i > ships.Count) { i = 0; }
        
            foreach (GameObject ship in ships)
            {
                //Display the respective position of each spaceship to the console
                //UnityEngine.Debug.Log(new Vector3(transform.position.x + formPositions[i].x, transform.position.y, transform.position.z + formPositions[i].z) + " " + i);

                //Tells the ships to leave the base
                ship.GetComponent<SpaceShipController>().LeaveBase(transform, new Vector3(transform.localPosition.x + formPositions[i].x, transform.localPosition.y, transform.localPosition.z + formPositions[i].z), moveSpeed);
                
                if (ship.transform.position != new Vector3(transform.localPosition.x + formPositions[i].x, transform.localPosition.y, transform.localPosition.z + formPositions[i].z))
                {
                    //UnityEngine.Debug.Log("Apparently " + ship.transform.position + " is equal to " + new Vector3(fromBase.transform.position.x, fromBase.transform.position.y + 1, fromBase.transform.position.z));
                    allReady = false;
                }
                i ++;
            }
        //transform.LookAt(toBase.transform);
        if (allReady == true)
        {

        SetUpRotationVariables();
        rotating = true;
        exiting = false;

        }
       

    }

    

    void PrintFormPositions()
    {
        foreach ( Vector3 point in formPositions)
        {
            //UnityEngine.Debug.Log(point);
        }
    }

    
    void SetUpRotationVariables()
    {
        dir = toBase.transform.position - transform.position;
        dir.y = 0;
        lookRot = Quaternion.LookRotation(dir);
        lookRot = lookRot *= Quaternion.Euler(0, 270, 0);
        //lookRot.x = 0;
        //lookRot.z = 0;
        //lookRot.y -= 180;
        //lookRot = Quaternion.Inverse(lookRot);
    }



    void RotateToFaceBase()
    {
        UnityEngine.Debug.Log("I know I'm supposed to be rotating");
        

        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRot, 1f);
        //transform.rotation = lookRot;
        //UnityEngine.Debug.Log("transform.rotation = " + transform.rotation + " lookRot = " + lookRot);
        if (transform.rotation == lookRot)
        {
            moving = true;
            rotating = false;
        }
        
       
    }
    void GoToBase()
    {

        
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(toBase.transform.position.x, toBase.transform.position.y + 1, toBase.transform.position.z), moveSpeed);
        if (Vector3.Distance(transform.position, toBase.transform.position) < 3)
        {
            entering = true;
            moving = false;
            
        }
    }
    void MoveIn()
    {
        foreach (GameObject ship in ships)
        {
            if (ship != null)
            {
                ship.GetComponent<SpaceShipController>().EnterBase(toBase.transform.position, moveSpeed);
            }
            else
            {
                ships.Remove(ship);
            }
        }
        if(ships.Count == 0)
        {
            entering = false;
        }
        
    }

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
                        //UnityEngine.Debug.Log((-1 * r).ToString() + ", " + i.ToString());
                        //UnityEngine.Debug.Log(r);
                        //UnityEngine.Debug.Log(n);

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
                        //UnityEngine.Debug.Log((-1 * r).ToString() + ", " + (i - .5f).ToString());
                        //UnityEngine.Debug.Log(r);
                        //UnityEngine.Debug.Log(n);
                        //n++;
                    }
                }
                r++;
            
            

            
        }
        formPositions = formPositions.Distinct().ToList();

    }
}
