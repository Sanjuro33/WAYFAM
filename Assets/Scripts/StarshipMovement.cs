using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarshipMovement : MonoBehaviour
{

    [SerializeField] float rotationSpeed = 500f;
    [SerializeField] float movementSpeed = 50f;
    [SerializeField] float rotationPosition;
    [SerializeField] Coroutine turningCoroutine;
    [SerializeField] float overallSpeed = 4f;
    // Start is called before the first frame update
    void Start()
    {
        rotationPosition = transform.eulerAngles.y;
    }

    // Update is called once per frame
    void Update()
    {
        MoveForwardAndBackwards();
        Rotate();
    }

    public IEnumerator RotateShip()
    {
        while (true)
        {
            
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 50f, 0), overallSpeed * Time.deltaTime);
        }
    }

    public void MoveForwardAndBackwards()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += transform.forward * Time.deltaTime * movementSpeed;
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.position += -transform.forward * Time.deltaTime * movementSpeed;
        }
    }
    public void Rotate()
    {

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(-Vector3.up * rotationSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        }
    }

}

    

