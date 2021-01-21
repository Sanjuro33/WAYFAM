using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ClickController : MonoBehaviour
{
    [Header("Sending Ships")]
    [SerializeField] GameObject fromBase;
    [SerializeField] List<GameObject> fromBases;
    [SerializeField] GameObject toBase;
    [SerializeField] float basePercentage;
    [SerializeField] string playerTag;

    [Header("Ability Usage")]
    [SerializeField] bool usingAbility;
    [SerializeField] AbilityButtons abilityButtons;

    [Header("Player Interactivity")]
    [SerializeField] bool canInteract;
    

//Main Methods
    // Start is called before the first frame update
    void Start()
    {
        canInteract = true;
        basePercentage = 1;
        playerTag = GameTags.allegienceTagBlue;
        abilityButtons = GetComponent<AbilityButtons>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canInteract)
        {
            CheckForClick();
        }
    }

//Custom Methods

    //Checks for different player input from the mouse
    void CheckForClick()
    {
        
        //Check for collider
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit);

        //Left click
        if (Input.GetButtonDown("Fire1"))
        {
            if (hit.collider.name == GameTags.outerBaseColliderName && hit.collider.transform.parent.gameObject.tag == playerTag)
            {
                StartCoroutine(setFromBase(hit));
            }

            if (hit.collider.name == GameTags.upgradeIconName && hit.collider.transform.parent.gameObject.tag == playerTag)
            {
                ActivateUpgrade(hit);
            }
        }
        
        //Right Click
        if (Input.GetButtonDown("Fire2"))
        {
            if (hit.collider.name == GameTags.outerBaseColliderName)
            {
                StartCoroutine(setToBase(hit));
            }
        }
    }

    //Tells a base to upgrade when the player clicks it's upgrade button
    private void ActivateUpgrade(RaycastHit hit)
    {
        hit.collider.gameObject.GetComponent<UpgradeButton>().UpgradeBase();
    }

    //Gives a slight pause before selecting a from base to make sure that the player doesn't add a base multiple times
    private IEnumerator setFromBase(RaycastHit hit)
    {
        //UnityEngine.Debug.Log(playerTag);

        //Debug.Log("FromBase = " + hit.collider.transform.parent);
        //fromBase = hit.collider.transform.parent.gameObject;
        if (!fromBases.Contains(hit.collider.transform.parent.gameObject))
        {
            fromBases.Add(hit.collider.transform.parent.gameObject);
            hit.collider.transform.parent.gameObject.transform.Find(GameTags.numShipsTextName).GetComponent<TextMeshPro>().color = Color.blue;
        }
        else
        {
            fromBases.Remove(hit.collider.transform.parent.gameObject);
            hit.collider.transform.parent.gameObject.transform.Find(GameTags.numShipsTextName).GetComponent<TextMeshPro>().color = Color.white;
        }
            
        
         yield return new WaitForSeconds(.1f);
    }

    //Gives a slight pause before selecting a to base to make sure that the player doesn't add a base multiple times
    private IEnumerator setToBase(RaycastHit hit)
    {
        
      
       
        //Debug.Log("ToBase =  " + hit.collider.transform.parent);
       
        toBase = hit.collider.transform.parent.gameObject;
        
        yield return new WaitForSeconds(.1f);
        if(toBase != null && fromBases.Count != 0)
        {
            SendShipTransfer();
        }
    }

    //selects each base in the list of frombases and sends the selected attack percentage to the selected ToBase
    private void SendShipTransfer()
    {

        foreach (GameObject from in fromBases)
        {
            if (from != toBase)
            {
                from.GetComponent<BaseController>().SendShips(basePercentage, from.tag, toBase);
            }
            //UnityEngine.Debug.Log(from.tag + " " + toBase.tag);
            from.transform.Find(GameTags.numShipsTextName).GetComponent<TextMeshPro>().color = Color.white;
            
            
        }
        //clears the selected bases after an attack
        toBase = null;
        
        fromBases.Clear();
    }

    //Sets the percentage of ships in the fromBase to send to the toBase, called by the Player Input Controller
    public void SetBasePercentage(float percentage)
    {
        basePercentage = percentage;
    }

    public void SetCanInteract(bool canAct)
    {
        canInteract = canAct;
    }

    public List<GameObject> GetFromBases()
    {
        return fromBases;
    }
}
