using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTriggerController : MonoBehaviour
{
    [SerializeField] ClickController clickController;
    [SerializeField] AbilityButtons abilityButtons;
    [SerializeField] SceneController sceneController;
    [SerializeField] TextBoxManager textBoxManager;
    [SerializeField] StarshipMovement starshipMovement;

    [SerializeField] int triggerIndex;

    // Start is called before the first frame update
    void Start()
    {
        triggerIndex = 0;
        FindControllers();
        SetStartingConditions();
    }

    // Update is called once per frame
    void Update()
    {
        if(textBoxManager.IsWaitingForTrigger())
        {
            if(triggerIndex == 0)
            {
                FreezeClickController(false);
                textBoxManager.SetTrigger();
                triggerIndex++;
            }
            if (triggerIndex == 1);
            {
                if (clickController.GetFromBases()[0] != null)
                {
                    UnityEngine.Debug.Log("I've detected a fromBase");
                    textBoxManager.SetTrigger();
                    triggerIndex++;
                }

            }
            if(triggerIndex == 2)
            {



            }
        }
    }


    private void FindControllers()
    {
        clickController = GameObject.FindWithTag("playerInputController").GetComponent<ClickController>();
        abilityButtons = GameObject.FindWithTag("playerInputController").GetComponent<AbilityButtons>();
        sceneController = GameObject.FindWithTag("sceneController").GetComponent<SceneController>();
        textBoxManager = GameObject.FindWithTag("playerHUD").GetComponent<TextBoxManager>();
        starshipMovement = GameObject.FindWithTag("Player").GetComponent<StarshipMovement>();
    }

    private void SetStartingConditions()
    {
        FreezeClickController(true);
        FreezeAbilityButtons(true);
        FreezeMovement(true);
    }

    private void FreezeClickController(bool frozen)
    {
        clickController.SetCanInteract(!frozen);
    }

    private void FreezeAbilityButtons(bool frozen)
    {
        abilityButtons.SetCanInteract(!frozen);
    }

    private void FreezeMovement(bool frozen)
    {
        starshipMovement.SetCanMove(!frozen);
    }
}
