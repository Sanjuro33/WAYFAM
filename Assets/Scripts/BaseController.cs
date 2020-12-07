using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
public class BaseController : MonoBehaviour
{
    [SerializeField] int shipCount = 0;
    [SerializeField] Collider innerCollider;
    [SerializeField] float ownerTag;
    [SerializeField] GameObject SpaceShipFormation;
    [SerializeField] GameObject SpaceShip;
    [SerializeField] GameObject spawnPoint;
    //[SerializeField] GameObject toBase;
    [SerializeField] float exitSpeed = .01f;
    [SerializeField] int numShips = 2;
    [SerializeField] TextMeshPro numShipsText;
    // Start is called before the first frame update
    void Start()
    {
        numShipsText = GetComponentInChildren<TextMeshPro>();
        //SendShips(1, GameTags.allegienceTagFriend);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateNumShipsText();
    }

    public void UpdateNumShipsText()
    {
        numShipsText.text = numShips.ToString();
    }

    public void OnShipCollision(Collider collision)
    {
        if (collision.gameObject.GetComponent<SpaceShipController>().GetAllegianceTag() == GameTags.allegienceTagFriend)
        {
            numShips++;
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.GetComponent<SpaceShipController>().GetAllegianceTag() == GameTags.allegienceTagFoe)
        {
            numShips--;
            Destroy(collision.gameObject);
        }
    }


    public void SendShips(float percentOfTotal, string allegianceTag, GameObject toBase)
    {
        int i = 0;
        GameObject spaceShipFormation = Instantiate(SpaceShipFormation, spawnPoint.transform.position, transform.rotation);
        spaceShipFormation.transform.localPosition = spawnPoint.transform.position;
        spaceShipFormation.GetComponent<SpaceShipFormationController>().ReceiveValues(this.gameObject, toBase, exitSpeed);
        //spaceShipFormation.transform.LookAt(toBase.transform);
        for (int x = numShips; x > (numShips * (1- percentOfTotal)); x--)
        {
            GameObject spaceShip = Instantiate(SpaceShip, spaceShipFormation.transform.position, spaceShipFormation.transform.rotation, spaceShipFormation.transform);
            spaceShip.transform.localPosition = new Vector3(0, 0, 0);
            spaceShip.GetComponent<SpaceShipController>().SetAllegianceTag(allegianceTag);
            UnityEngine.Debug.Log("I'm setting this to be a friend");
            spaceShipFormation.GetComponent<SpaceShipFormationController>().AddShip(spaceShip);
            //spaceShip.transform.LookAt(toBase.transform);
            i++;
        }
        numShips -= i;
        //transform.LookAt(toBase.transform);
        spaceShipFormation.GetComponent<SpaceShipFormationController>().TriggerExit();
    }
}
