using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using Zork.Common;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UnityInputService Input;
    [SerializeField] private UnityOutputService Output;
    [SerializeField] private TextMeshProUGUI LocationText;
    [SerializeField] private TextMeshProUGUI ScoreText;
    [SerializeField] private TextMeshProUGUI MovesText;
    [SerializeField] private TextMeshProUGUI InputField;

    private void Awake()
    {
        TextAsset gameJson = Resources.Load<TextAsset>("GameJson");
        _game = JsonConvert.DeserializeObject<Game>(gameJson.text);

        _game.Run(Input, Output);
    }

    private Game _game;
}