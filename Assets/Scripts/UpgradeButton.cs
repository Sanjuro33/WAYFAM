using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeButton : MonoBehaviour
{
    [Header("BaseController")]
    [SerializeField] BaseController baseController;
//Main Methods

    // Start is called before the first frame update
    void Start()
    {
        FindBaseController();
    }

//Custom Methods

    //Finds the parent BaseController object of the upgrade button
    public void FindBaseController()
    {
        baseController = transform.parent.GetComponent<BaseController>();
    }
    //Upgrades the parent base of the upgrade button
    public void UpgradeBase()
    {
        UnityEngine.Debug.Log("The Upgrade Button had been clicked");
        baseController.UpgradeBase();
    }
}
