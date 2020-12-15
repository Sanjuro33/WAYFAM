using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
public class BaseController : MonoBehaviour
{
    [SerializeField] int shipCount = 0;
    [SerializeField] Collider innerCollider;
    [SerializeField] string ownerTag;
    [SerializeField] GameObject SpaceShipFormation;
    [SerializeField] Material baseMaterial;
    [SerializeField] Material genericMaterial;
    [SerializeField] GameObject SpaceShip;
    [SerializeField] GameObject spawnPoint;
    [SerializeField] bool emptyBase = false;
    [SerializeField] bool addingShip = false;
    [SerializeField] int maxShips = 20;
    [SerializeField] int shipsPerGen = 1;
    [SerializeField] float generationDelay = 2f;
    //[SerializeField] GameObject toBase;
    [SerializeField] float exitSpeed = .01f;
    [SerializeField] int numShips = 2;
    [SerializeField] TextMeshPro numShipsText;
    // Start is called before the first frame update
    void Start()
    {
        numShipsText = GetComponentInChildren<TextMeshPro>();
        transform.Find(GameTags.outerBaseColliderName).GetComponent<MeshRenderer>().material = baseMaterial;
        gameObject.tag = ownerTag;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateNumShipsText();
        checkToGenericBase();
        GenerateShips();
    }

    public void UpdateNumShipsText()
    {
        numShipsText.text = numShips.ToString();
    }

    public void OnShipCollision(Collider collision)
    {
        if (collision.gameObject.GetComponent<SpaceShipController>().GetAllegianceTag() == ownerTag)
        {
            numShips++;
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.GetComponent<SpaceShipController>().GetAllegianceTag() != ownerTag)
        {
            numShips--;
            Destroy(collision.gameObject);
            if (numShips <= 0)
            {
                numShips = Mathf.Abs(numShips);
                Flip(collision);
            }
        }
    }

    public void GenerateShips()
    {
        if ((numShips < maxShips) && !addingShip )
        {
            StartCoroutine(AddShip());
        }
    }

    public IEnumerator AddShip()
    {
        addingShip = true;
        yield return new WaitForSeconds(generationDelay);
        if ((baseMaterial != genericMaterial))
        {
            numShips += shipsPerGen;
        }
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
            spaceShip.GetComponent<SpaceShipController>().SetShipMaterial(baseMaterial);
            
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


    public void Flip(Collider collision)
    {
        if (numShips == 0)
        {
            setBaseToGeneric();
        }

        else
        {
            ownerTag = collision.gameObject.GetComponent<SpaceShipController>().GetAllegianceTag();
            gameObject.tag = ownerTag;
            baseMaterial = collision.gameObject.GetComponent<SpaceShipController>().GetMaterial();
            print(baseMaterial);
            AssignBaseMaterial(baseMaterial);
        }
    }

    public void AssignBaseMaterial(Material material)
    {
        transform.Find(GameTags.outerBaseColliderName).GetComponent<MeshRenderer>().material = material;
    }

    public void setBaseToGeneric()
    {
        ownerTag = GameTags.allegienceTagNone;
        gameObject.tag = ownerTag;
        baseMaterial = genericMaterial;
        AssignBaseMaterial(baseMaterial);
    }

    public void checkToGenericBase()
    {
        if(emptyBase)
        {
            
            setBaseToGeneric();
            emptyBase = false;
        }
    }
}
