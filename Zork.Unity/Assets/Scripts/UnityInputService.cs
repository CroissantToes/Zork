using System;
using TMPro;
using UnityEngine;
using Zork.Common;

public class UnityInputService : MonoBehaviour, IInputService
{
    [SerializeField] private TextMeshProUGUI InputField;

    public event EventHandler<string> InputReceived;

    public void ProcessInput()
    {
    }
}
