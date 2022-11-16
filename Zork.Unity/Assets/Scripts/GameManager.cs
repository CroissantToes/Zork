using Newtonsoft.Json;
using TMPro;
using TMPro.EditorUtilities;
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
            UnityEditor.EditorApplication.isPlaying = false;
        }
    }

    private void SetLocationText(object sender, Room location)
    {
        LocationText.text = location.Name;
    }

    private Game _game;
}