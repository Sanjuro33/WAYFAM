using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    [SerializeField] CommanderController playerController;
    [SerializeField] List<CommanderController> aiControllers;
    [SerializeField] int totalEnemyTroops;
    [SerializeField] int totalPlayerTroops;
    [SerializeField] int totalTroops;
    [SerializeField] bool gameOver = false;
    [SerializeField] bool playerVictory;
    [SerializeField] TroopSliderController troopSliderController;
    [SerializeField] GameObject canvasControllerObject;
 
   
   
        
    // Start is called before the first frame update
    void Start()
    {
        troopSliderController = FindObjectOfType<TroopSliderController>();
        canvasControllerObject = FindObjectOfType<CanvasController>().gameObject;
        canvasControllerObject.transform.GetChild(GameTags.endScreenCanvasIndex).gameObject.SetActive(false);
        
        gameOver = true;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTotalTroops();
        UpdateSlider();
        CheckToEndGame();
        StartCoroutine(WaitToCheckForGameOver());
    }

    private void FindControllers()
    {

    }

    private void FreezeUnfreezePlayerControls()
    {

    }

    void UpdateTotalTroops()
    {
        totalEnemyTroops = 0;
        foreach (CommanderController controller in aiControllers)
        {
            totalEnemyTroops += controller.GetTotalTroops();
        }
        totalPlayerTroops = playerController.GetTotalTroops();

        totalTroops = totalEnemyTroops + totalPlayerTroops;
    }

    void UpdateSlider()
    {
        troopSliderController.UpdateTroopSlider(totalTroops, totalPlayerTroops);
    }

    private IEnumerator WaitToCheckForGameOver()
    {
        yield return new WaitForSeconds(1f);
        gameOver = false;
    }

    //returns true for player victory and false for player defeat
    private void CheckToEndGame()
    {
        if (!gameOver)
        {
            UnityEngine.Debug.Log("I know the game is over");
            if (totalPlayerTroops == 0)
            {
                UnityEngine.Debug.Log("The Enemy Won");
                playerVictory = false;
                StartCoroutine(EndGame(playerVictory));
                gameOver = true;
            }
            if (totalEnemyTroops == 0)
            {
                UnityEngine.Debug.Log("The Player Won");
                playerVictory = true;
                StartCoroutine(EndGame(playerVictory));
                gameOver = true;
            }
        }
        
    }

    private IEnumerator EndGame(bool playerVictory)
    {
        UnityEngine.Debug.Log("I know to set the canvas to active");
        canvasControllerObject.transform.GetChild(GameTags.endScreenCanvasIndex).gameObject.SetActive(true);
        if (playerVictory)
        {
            
            yield return new WaitForSeconds(1f);
            canvasControllerObject.transform.GetChild(GameTags.endScreenCanvasIndex).GetComponent<EndScreenController>().ReceiveValues(true);
        }
        else
        {
            
            yield return new WaitForSeconds(1f);
            canvasControllerObject.transform.GetChild(GameTags.endScreenCanvasIndex).GetComponent<EndScreenController>().ReceiveValues(false);
        }
    }


    public bool CheckGameOver()
    {
        return gameOver;
    }

    public bool CheckPlayerVictory()
    {
        return playerVictory;
    }
}
