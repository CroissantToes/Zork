using TMPro;
using UnityEngine;
using Zork.Common;
using UnityEngine.UI;
using System.Collections.Generic;

public class UnityOutputService : MonoBehaviour, IOutputService
{
    [SerializeField] private TextMeshProUGUI TextLinePrefab;

    [SerializeField] private Image NewLinePrefab;

    [SerializeField] private Transform ContentTransform;

    [SerializeField] private int MaxLines = 20;

    public void Write(object obj)
    {
        ParseAndWriteLine(obj.ToString());
    }

    public void Write(string message)
    {
        ParseAndWriteLine(message);
    }

    public void WriteLine(object obj)
    {
        ParseAndWriteLine(obj.ToString());
    }

    public void WriteLine(string message)
    {
        ParseAndWriteLine(message);
    }

    private void ParseAndWriteLine(string message)
    {
        string[] lines = message.Split("\n");

        foreach(string line in lines) 
        {
            if(line == "")
            {
                var textLine = Instantiate(NewLinePrefab, ContentTransform);
                _entries.Enqueue(textLine.gameObject);
            }
            else
            {
                var textLine = Instantiate(TextLinePrefab, ContentTransform);
                textLine.text = line;
                _entries.Enqueue(textLine.gameObject);
            }
             
            if(_entries.Count > MaxLines)
            {
                Destroy(_entries.Dequeue());
            }
        }
    }

    private Queue<GameObject> _entries = new Queue<GameObject>();
}
