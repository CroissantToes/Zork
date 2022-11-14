using TMPro;
using UnityEngine;
using Zork.Common;

public class UnityOutputService : MonoBehaviour, IOutputService
{
    [SerializeField] private TextMeshProUGUI TextLine;

    public void Write(object obj)
    {
        TextLine.text = obj.ToString();
    }

    public void Write(string message)
    {
        TextLine.text = message;
    }

    public void WriteLine(object obj)
    {
        TextLine.text = obj.ToString();
    }

    public void WriteLine(string message)
    {
        TextLine.text = message;
    }
}
