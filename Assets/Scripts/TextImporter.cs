using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextImporter : MonoBehaviour
{
    [SerializeField] TextAsset textFile;
    [SerializeField] List<string> textLines;
    // Start is called before the first frame update
    void Start()
    {

        SortTextFile();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SortTextFile()
    {
        if (textFile != null)
        {
            textLines = new List<string>((textFile.text.Split('\n')));
        }
        foreach(string text in textLines)
        {
            if(text == "")
            {
                textLines.RemoveAt(textLines.IndexOf(text));
            }
        }
    }
}
