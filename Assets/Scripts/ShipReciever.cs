using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipReciever : MonoBehaviour
{
    //Triggers the OnShipCollision method in the parent GameObject
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == GameTags.spaceShipTag)
        {
            transform.parent.GetComponent<BaseController>().OnShipCollision(other);
        }
    }
}
