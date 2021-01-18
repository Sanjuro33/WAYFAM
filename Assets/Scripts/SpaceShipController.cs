  using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShipController : MonoBehaviour
{
    [Header("Bases")]
    [SerializeField] GameObject fromBase;
    [SerializeField] GameObject toBase;

    [Header("Flag Variables")]
    [SerializeField] bool exiting;
    [SerializeField] bool moving;
    [SerializeField] bool entering;

    [Header("Commander Attributes")]
    //[SerializeField] Material baseMaterial;
    [SerializeField] float moveSpeed = .0f;
    [SerializeField] string allegianceTag = "Friend";
 
//Custom Methods
    
    //Tell the ship to leave the local base
    public void LeaveBase(Transform formation, Vector3 formationPosition, float moveSpeed)
    {
        UnityEngine.Debug.Log("The ship is independently moving");

        //orders the the ship to move to it's formation position
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(formationPosition.x, formationPosition.y, formationPosition.z), moveSpeed);
    }

    //Tells the ship to move into the center of the base
    public void EnterBase(Vector3 toBase, float moveSpeed)
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(toBase.x, toBase.y, toBase.z), moveSpeed);
    }

//Accessors and Mutators

    //Changes the allegiance tag of the ship
    public void SetAllegianceTag(string tag)
    {
        allegianceTag = tag;
    }

    //Returns the allegeiance tag of the ship
    public string GetAllegianceTag()
    {
        return allegianceTag;
    }

    public GameObject GetToBase()
    {
        return toBase;
    }

    public void SetToBase(GameObject toBase)
    {
        this.toBase = toBase;
    }

    //Changes the material of the ship
    //public void SetShipMaterial(Material material)
    //{
        //shipMaterial = material;
        //transform.GetChild(0).GetComponent<MeshRenderer>().material = shipMaterial;
    //}

    //Returns the ship material
    //public Material GetMaterial()
    //{
        //return shipMaterial;
    //}


}
