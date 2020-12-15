using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickController : MonoBehaviour
{
    [SerializeField] GameObject fromBase;
    [SerializeField] GameObject toBase;
    [SerializeField] float basePercentage;
    // Start is called before the first frame update
    void Start()
    {
        basePercentage = 1;
    }

    // Update is called once per frame
    void Update()
    {
        CheckForStartingBaseRegister();
    }

    void CheckForStartingBaseRegister()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            StartCoroutine(setFromBase());
        }

        if (Input.GetButtonDown("Fire2"))
        {
            StartCoroutine(setToBase());
        }
    }

    private IEnumerator setFromBase()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        // Casts the ray and get the first game object hit
        Physics.Raycast(ray, out hit);
        if (hit.collider.name == "Base Outer")
        {
            Debug.Log("FromBase = " + hit.collider.transform.parent);
            fromBase = hit.collider.transform.parent.gameObject;
        }
        yield return new WaitForSeconds(.1f);
    }

    private IEnumerator setToBase()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        // Casts the ray and get the first game object hit
        Physics.Raycast(ray, out hit);
        if (hit.collider.name == "Base Outer")
        {
            Debug.Log("ToBase =  " + hit.collider.transform.parent);
            toBase = hit.collider.transform.parent.gameObject;
        }
        yield return new WaitForSeconds(.1f);
        if(toBase != null && fromBase != null)
        {
            SendShipTransfer();
        }
    }

    private void SendShipTransfer()
    {
        
        //if(fromBase.tag == toBase.tag)
        //{
            //fromBase.GetComponent<BaseController>().SendShips(basePercentage, GameTags.allegienceTagFriend, toBase);
            //UnityEngine.Debug.Log(fromBase.tag + " " + toBase.tag);
        //}
        //if (fromBase.tag != toBase.tag)
        //{
        fromBase.GetComponent<BaseController>().SendShips(basePercentage, fromBase.tag, toBase);
        UnityEngine.Debug.Log(fromBase.tag + " " + toBase.tag);
        //}
        toBase = null;
        fromBase = null;
    }


    public void SetBasePercentage(float percentage)
    {
        basePercentage = percentage;
    }
}
