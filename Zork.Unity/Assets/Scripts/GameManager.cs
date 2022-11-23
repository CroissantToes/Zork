using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using Zork.Common;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UnityInputService InputService;
    [SerializeField] private UnityOutputService OutputService;
    [SerializeField] private TextMeshProUGUI LocationText;
    [SerializeField] private TextMeshProUGUI ScoreText;
    [SerializeField] private TextMeshProUGUI MovesText;

    private void Awake()
    {
        TextAsset gameJson = Resources.Load<TextAsset>("GameJson");
        _game = JsonConvert.DeserializeObject<Game>(gameJson.text);
        _game.Run(InputService, OutputService);
        _game.Player.locationChanged += SetLocationText;
        _game.Player.scoreChanged += SetScoreText;
        _game.Player.movesChanged += SetMovesText;
    }

    private void Start()
    {
        InputService.SetFocus();
        LocationText.text = _game.Player.CurrentRoom.Name;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            InputService.ProcessInput();
            InputService.SetFocus();
        }

        if(_game.IsRunning == false)
        {
            //UnityEditor.EditorApplication.isPlaying = false;
            Application.Quit();
        }
    }

    private void SetLocationText(object sender, Room location)
    {
        LocationText.text = location.Name;
    }

    private void SetScoreText(object sender, int score)
    {
        ScoreText.text = $"Score: {score}";
    }

    private void SetMovesText(object sender, int moves)
    {
        MovesText.text = $"Moves: {moves}";
    }

    private Game _game;
}