using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Relative Player Position Variables")]
    [SerializeField] Transform Player;
    [SerializeField] float camDistanceZ;

//Main Methods
    // Start is called before the first frame update
    void Start()
    {
        //Sets the camDistance to be 14 units away from the player
        //TODO: move the 14 to a variable inside of GameTags
        FindPlayer();
        camDistanceZ = 14;
    }

    // Update is called once per frame
    void Update()
    {
        //Moves the camera to match the left and right positions of the spaceship but remains further back to not crowd in.
        transform.position = new Vector3(Player.position.x, transform.position.y, Player.position.z - camDistanceZ);
    }


    void FindPlayer()
    {
        foreach (MotherShipController controller in FindObjectsOfType<MotherShipController>())
        {
            if(controller.gameObject.tag == "Player")
            {
                Player = controller.gameObject.transform;
            }
        }
    }
}
