using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Miss issippi 
public class EnemyAI : MonoBehaviour
{
    [Header("Starting Attributes")]
    [SerializeField] string allegianceTag;
    [SerializeField] float maxSendingMultiplier = 1.5f;
    [SerializeField] float attackDelay = 10.5f;
    [SerializeField] bool attacking = true;

    [Header("Commander Controller Variables")]
    [SerializeField] CommanderController commanderController;
    [SerializeField] CommanderController playerCommanderController;
    // Start is called before the first frame update

//Main Methods

    void Start()
    {
        FindPlayerCommanderController();
    }

    // Update is called once per frame
    void Update()
    {
        CheckToUpgradeBases();
        AttackOtherBase();
    }

//Custom Methods
    
    //Finds the commander controller that is attributed to the player
    private void FindPlayerCommanderController()
    {
        //Makes a list of all commander controllers
        foreach(CommanderController controller in new List<CommanderController>(FindObjectsOfType<CommanderController>()))
        {
            //Selects the Commander Controller owned by the player
            if(controller.gameObject.tag == "Player")
            {
                playerCommanderController = controller;
            }
        }
    }

    //Check if any of the bases owned by the commander controller are upgradable. If so, upgrade them
    private void CheckToUpgradeBases()
    {
        //Makes as List of all bases owned by the commander controller
        foreach(BaseController controller in commanderController.GetOwnedBases())
        {
            //Upgrades those with enough ships
            if(controller.GetNumShips() > controller.GetUpgradeCost())
            {
                controller.UpgradeBase();
            }
        }
    }

    //Returns a list of lists of bases that can be attacked by each enemy base respectively
    private List<List<BaseController>> CheckToAttackBases()
    {
        //Set up variable to return
        List<List<BaseController>> attackableBasesForEach = new List<List<BaseController>>();
        foreach (BaseController enemyController in commanderController.GetOwnedBases())
        {

            //set up a list for base controllers of the 3 most attackable bases
            List<BaseController> attackableBases = new List<BaseController>(3);
            
            //set up a list for the distances of the three most attackable bases
            List<float> attackableBaseDistances = new List<float>(3);

            UnityEngine.Debug.Log(attackableBases.Count + " " + attackableBaseDistances.Count);

            //pre-set all of the distances of the bases to infinity so any distance is shorter
            attackableBaseDistances.Add(Mathf.Infinity);
            attackableBaseDistances.Add(Mathf.Infinity);
            attackableBaseDistances.Add(Mathf.Infinity);

            //pre-set all of the bases to be null so the individual indexes can be referenced
            attackableBases.Add(null);
            attackableBases.Add(null);
            attackableBases.Add(null);

            //Look through every base on the map
            foreach (BaseController controller in FindObjectsOfType<BaseController>())
            {

                //make sure the AI is not trying to attack one of it's own bases
                if (!commanderController.GetOwnedBases().Contains(controller))
                {
                    //calculate the distance between the bases
                    float distBetweenBases = Vector3.Distance(enemyController.transform.position, controller.transform.position);

                    //Create a for loop to check through all of the values on the attackableBaseDistances list to see if it's shorter than the other values
                    for (int x = 0; x < 3; x++)
                    {
                        //create a float to hold the distances when they are shifted up and down the list
                        float distHolder = 0;
                        BaseController baseHolder = null;
                        //Check for the eventuality that there is a base with a distance less than the value of attackableBaseDistances that is currently being attacked
                        if (distBetweenBases < attackableBaseDistances[x] && CheckForNumberAdvantage(enemyController, controller))
                        {
                            //Sets a placeholder variable to the value of both lists at x as a placeholder
                            distHolder = attackableBaseDistances[x];
                            baseHolder = attackableBases[x];
                            
                            //Sets the value of both lists at x to be those of the current base
                            attackableBaseDistances[x] = distBetweenBases;
                            attackableBases[x] = controller;
                            x++;

                            //Moves the rest of the values down the list in order to keep it consistant
                            for (int i = x; i < 2; i++)
                            {
                                distHolder = attackableBaseDistances[i];
                                UnityEngine.Debug.Log(distHolder);
                                baseHolder = attackableBases[i];
                                UnityEngine.Debug.Log(baseHolder);
                                attackableBaseDistances[i] = distHolder;
                                attackableBases[i] = baseHolder;
                            }
                            //in the event something falls at the end of the list and escapes the i loop, just add it on the end
                            if (x == 2)
                            {
                                attackableBaseDistances[x] = distHolder;
                                attackableBases[x] = baseHolder;
                            }                            
                            break;

                        }


                    }
                }
            }
            //Removes any null values from teh list of attackable bases that served as placeholders in the beginning
            for (int i = 0; i < attackableBases.Count; i++)
            {
                if (attackableBases[i] == null)
                {
                    attackableBases.RemoveAt(i); // O(n)
                }
               
            } 
            //Adds the list of attackableBases to attackableBasesForEach ta the same index as the base in owned Bases
            attackableBasesForEach.Add(attackableBases);
        }
        return attackableBasesForEach;
    }
    
    //Allows the AI to attack another base
    private void AttackOtherBase()
    {
        //Sets up placeholder variables
        List<List<BaseController>> attackableBasesForEach = CheckToAttackBases();
        List<BaseController> ownedBases = commanderController.GetOwnedBases();

        //if the base can attack
        if (attacking)
            {
            
                //Find the base to attack from
                BaseController controller = FindBaseToAttack(ownedBases);

                //Find the base to attack
                List<BaseController> attackableBases = attackableBasesForEach[ownedBases.IndexOf(controller)];
                int baseToAttack = Random.Range(0, attackableBases.Count);
                BaseController baseTo = attackableBases[baseToAttack];

                //Find the number of ships to send
                int numShips = NumShipsToSend(baseTo, controller);
                float percentToSend = ( (float) numShips / (float) controller.GetNumShips());

                //Send the attack
                StartCoroutine(DelayBaseAttack(baseTo.gameObject, controller.gameObject, percentToSend));
            }
        
    }

    //Selects a base from within the attackable bases
    private BaseController FindBaseToAttack(List<BaseController> ownedBases)
    {
        BaseController controller = null;
        while (controller == null)
        {
            int baseNum = Random.Range(0, ownedBases.Count);
            UnityEngine.Debug.Log("baseNum = " + baseNum);
            
            if (ownedBases[baseNum] != null)
            {
                controller = ownedBases[baseNum];
            }
        }
        return controller;
    }

    //Makes sure that the attacking base has at least more than half of the ships in the base being attacked
    private bool CheckForNumberAdvantage(BaseController fromBase, BaseController toBase)
    {
        if ((fromBase.GetNumShips()*.5) > toBase.GetNumShips())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private IEnumerator DelayBaseAttack(GameObject toBase, GameObject fromBase, float percentOfTotal)
    {
        attacking = false;
        //UnityEngine.Debug.Log("I am attacking " + toBase + " from " + fromBase + " with " + percentOfTotal + " percent of my ships");
        sendAttack(toBase, fromBase, percentOfTotal);
        yield return new WaitForSeconds(attackDelay);
        attacking = true;
    }

    //Selects a random number of ships to send, ranging from exactly enough to the fromBase's full capacity
    private int NumShipsToSend(BaseController toBase, BaseController fromBase)
    {
        int numToSend = Random.Range(toBase.GetNumShips(), fromBase.GetNumShips());
        return numToSend;
    }

    //Allows the sendAttack Method to be easier to call
    private void sendAttack(GameObject toBase, GameObject fromBase, float percentOfTotal)
    {
        fromBase.GetComponent<BaseController>().SendShips(percentOfTotal, allegianceTag, toBase);
    }
       
//Accessors and Mutators

    //returns the allegiance tag of the EnemyAI
    public string GetAllegianceTag()
    {
        return allegianceTag;
    }

    //Sets the CommanderCOntroller that matches the AI
    public void SetCommanderController(CommanderController controller)
    {
        commanderController = controller;
    }


//Utility Methods

    //Eliminates midpoint rounding errors
    public static float MyRound(float value)
    {
        if (value % 0.5f == 0)
            return Mathf.Ceil(value);
        else
            return Mathf.Round(value);
    }
}
