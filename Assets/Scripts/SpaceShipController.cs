using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShipController : MonoBehaviour
{
    [SerializeField] GameObject fromBase;
    [SerializeField] GameObject toBase;
    [SerializeField] float moveSpeed = .0f;
    [SerializeField] bool exiting;
    [SerializeField] bool moving;
    [SerializeField] bool entering;
    [SerializeField] string allegianceTag = "Friend";
    // Start is called before the first frame update
    void Start()
    {
        //SetAllegianceTag(GameTags.allegienceTagFoe);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Tell the ship to leave the local base
    public void LeaveBase(Transform formation, Vector3 formationPosition, float moveSpeed)
    {
        UnityEngine.Debug.Log("The ship is independently moving");

        //orders the t
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(formationPosition.x, formationPosition.y, formationPosition.z), moveSpeed);
    }

    

    public void EnterBase(Vector3 toBase, float moveSpeed)
    {  
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(toBase.x, toBase.y, toBase.z), moveSpeed);
    }

    public void SetAllegianceTag(string tag)
    {
        allegianceTag = tag;
    }

    public string GetAllegianceTag()
    {
        return allegianceTag;
    }


}
