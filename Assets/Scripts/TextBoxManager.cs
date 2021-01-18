using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TextBoxManager : MonoBehaviour
{
    [SerializeField] GameObject textBox;

    [SerializeField] TextMeshProUGUI theText;

    [SerializeField] TextAsset textFile;
    [SerializeField] List<string> textLines;

    [SerializeField] int currentLine;
    [SerializeField] int endAtLine;

    [SerializeField] bool printingText;
    // Start is called before the first frame update
    void Start()
    {
        if (textFile != null)
        {
            textLines = new List<string>((textFile.text.Split('\n')));
        }

        if(endAtLine == 0)
        {
            endAtLine = textLines.Count - 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentLine < textLines.Count)
        {
            theText.text = textLines[currentLine];
            if (Input.GetKeyDown(KeyCode.Return))
            {
                currentLine += 1;
            }
        }

        if(currentLine > endAtLine)
        {
            textBox.SetActive(false);
        }

    }
}