using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

//Main Methods
    // Start is called before the first frame update
    void Start()
    {
        MakeSingleton();
    }

//Utility Methods
    
    //Makes this object sustain between screen loads
    private void MakeSingleton()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("gameController");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }
}
