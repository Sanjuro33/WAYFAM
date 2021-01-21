using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarshipMovement : MonoBehaviour
{
    [Header("Speeds")]
    [SerializeField] float rotationSpeed = 500f;
    [SerializeField] float movementSpeed = 50f;
    [SerializeField] float overallSpeed = 4f;

    [Header("Positions")]
    [SerializeField] float rotationPosition;

    [Header("Coroutines")]
    [SerializeField] Coroutine turningCoroutine;
    
    [Header("Interactivity")]
    [SerializeField] bool canMove;
//Main Methods

    // Start is called before the first frame update
    void Start()
    {
        canMove = true;
        rotationPosition = transform.eulerAngles.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            MoveForwardAndBackwards();
            Rotate();
        }
    }

//Custom Methods

    //Rotates the ship
    public IEnumerator RotateShip()
    {
        while (true)
        {
            
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 50f, 0), overallSpeed * Time.deltaTime);
        }
    }

    //Moves the ship forward and backwards
    public void MoveForwardAndBackwards()
    {
        //Moves the ship forward when W is blessed
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += transform.forward * Time.deltaTime * movementSpeed;
        }

        //Moves the ship backward when S is pressed
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += -transform.forward * Time.deltaTime * movementSpeed;
        }
    }
    //Rotates the ship 
    public void Rotate()
    {
        //Rotates left when the A key is pressed
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(-Vector3.up * rotationSpeed * Time.deltaTime);
        }

        //Rotates right when the D key is pressed
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        }
    }

    public void SetCanMove(bool moving)
    {
        canMove = moving;
    }
}

    

