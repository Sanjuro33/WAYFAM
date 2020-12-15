using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform Player;
    [SerializeField] float camDistanceZ;
    // Start is called before the first frame update
    void Start()
    {
        camDistanceZ = 14;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(Player.position.x, transform.position.y, Player.position.z - camDistanceZ);
    }
}
