using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TextBoxManager : MonoBehaviour
{
    [SerializeField] GameObject textBox;
    [SerializeField] SceneController sceneController;
    [SerializeField] TextMeshProUGUI theText;

    [SerializeField] TextAsset textFile;
    [SerializeField] List<string> textLines;

    [SerializeField] int currentLine;
    [SerializeField] int endAtLine;

    [SerializeField] bool printingText;
    [SerializeField] bool trigger;
    [SerializeField] bool waitingForTrigger;

    // Start is called before the first frame update
    void Start()
    {
        trigger = false;
        SortTextFile();
        FindSceneController();
        //if (textFile != null)
        //{
        //textLines = new List<string>((textFile.text.Split('\n')));
        //}

        //if(endAtLine == 0)
        //{
        //endAtLine = textLines.Count - 1;
        //}
    }

   

    // Update is called once per frame
    void Update()
    {
        if (currentLine < textLines.Count)
        {
            theText.text = textLines[currentLine];

            //Check if the computer is waiting for a boolean response from this statement, such as a yes or a no(YN)
            if (textLines[currentLine].Substring(0, GameTags.boolResponseQualifier.Length).Equals(GameTags.boolResponseQualifier))
            {
                CheckIfTextIsBoolean();
            }
            else if (textLines[currentLine].Substring(0, GameTags.finalLineIdentifier.Length).Equals(GameTags.finalLineIdentifier))
            {
                CheckIfTextIsFinal();
            }
            else if (textLines[currentLine].Substring(0, GameTags.triggerTextIndentifier.Length).Equals(GameTags.triggerTextIndentifier))
            {
                CheckIfTextIsTrigger();
            }
            //Check if the line expects a condition to be met before it can continue

            //If the line can be clicked through, just allow the player to click through it
            else
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    currentLine += 1;
                    theText.text = textLines[currentLine];
                }
            }
        }

        else if(currentLine > endAtLine)
        {
            textBox.SetActive(false);
        }

    }

    private void CheckIfTextIsTrigger()
    {
        
         waitingForTrigger = true;
         if(trigger == true)
         {
            currentLine++;
            theText.text = textLines[currentLine];
         }
       
    }

    private void FindSceneController()
    {
        sceneController = GameObject.FindGameObjectWithTag("sceneController").GetComponent<SceneController>();
    }

    

    private void CheckIfTextIsFinal()
    {
        
            theText.text = textLines[currentLine].Substring(GameTags.finalLineIdentifier.Length);
            if(sceneController.CheckGameOver())
            {
                if(sceneController.CheckPlayerVictory())
                {
                    ClearLinesThatStartWith(GameTags.loseTextIdentifier);
                    ClearBeginningsfromLines(GameTags.winTextIdentifier);
                    currentLine++;
                    theText.text = textLines[currentLine];
            }
                else
                {
                    ClearLinesThatStartWith(GameTags.winTextIdentifier);
                    ClearBeginningsfromLines(GameTags.loseTextIdentifier);
                    currentLine++;
                    theText.text = textLines[currentLine];
                }
            }
        

    }

    private void CheckIfTextIsBoolean()
    {
        
            theText.text = textLines[currentLine].Substring(GameTags.boolResponseQualifier.Length);
            if (Input.GetKeyDown(KeyCode.Y))
            {

                ClearLinesThatStartWith(GameTags.noAnswer);
                ClearBeginningsfromLines(GameTags.yesAnswer);
                currentLine += 1;
                theText.text = textLines[currentLine];
            }
            if (Input.GetKeyDown(KeyCode.N))
            {

                ClearLinesThatStartWith(GameTags.yesAnswer);
                ClearBeginningsfromLines(GameTags.noAnswer);
                currentLine += 1;
                theText.text = textLines[currentLine];
            }
        
    }

    private void SortTextFile()
    {
        if (textFile != null)
        {
            textLines = new List<string>((textFile.text.Split('\n')));
        }

        if (endAtLine == 0)
        {
            endAtLine = textLines.Count - 1;
        }

        for (int x = 0; x < textLines.Count; x++)
        {
            if (string.IsNullOrWhiteSpace(textLines[x]))
            {
                UnityEngine.Debug.Log("I've tried to remove an empty line");
                textLines.RemoveAt(x);
                x--;
            }
            else if (textLines[x].Substring(0, GameTags.commentIndentifier.Length) == GameTags.commentIndentifier)
            {
                UnityEngine.Debug.Log("I've tried to remove an empty line");
                textLines.RemoveAt(x);
                x--;
            }
        }
        theText.text = textLines[currentLine];
    }

    private void ClearBeginningsfromLines(string start)
    {
        bool foundLines = false;
        for (int x = currentLine; x < textLines.Count; x++)
        {
            UnityEngine.Debug.Log(currentLine + "" + foundLines);
            if (textLines[x].Substring(0, start.Length) == start)
            {
                UnityEngine.Debug.Log("I've tried to remove the beginning from a line");
                textLines[x] = textLines[x].Substring(start.Length);
                //x--
                foundLines = true;
            }
            else 
            {
                if (foundLines)
                {
                    break;
                }
            }
        }
    }

    private void ClearLinesThatStartWith(string start)
    {
        bool foundLines = false;
        for (int x = currentLine; x < textLines.Count; x++)
        {
            UnityEngine.Debug.Log(currentLine + ""+ foundLines);
            if (textLines[x].Substring(0, start.Length) == start)
            {
                UnityEngine.Debug.Log("I've tried to remove an empty line");
                textLines.RemoveAt(x);
                x--;
                foundLines = true;
            }
            else 
            {
                if (foundLines)
                {
                    break;
                }
            }
        }
    }

    public bool IsWaitingForTrigger()
    {
        return waitingForTrigger;
    }

    public void SetTrigger()
    {
        trigger = true;
    }
}