using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Text score;

    [HideInInspector]
    public static int firstPlayerScore = 0;
    [HideInInspector]
    public static int secondPlayerScore = 0;
    [HideInInspector]
    public static int numPlayers = 2;

    public UnityEvent changeScore;
    public List<PlayerController> players = new List<PlayerController>();

    private RG_Inputs inputs;
    private InputAction quitToTitle;
    private void Awake()
    {
        instance = this;
        inputs = new RG_Inputs();
    }
    void Start()
    {

        ChangeScore();
    }

    private void OnEnable()
    {
        quitToTitle = inputs.General.Quit;
        quitToTitle.Enable();
        quitToTitle.started += QuitToTitle;

    }
    public void ChangeScore()
    {
        if (score)
            score.text = firstPlayerScore.ToString() + "   " + secondPlayerScore.ToString();
    }

    public void TwoPlayers()
    {
        numPlayers = 2;
        ResetScore();
        SceneManager.LoadScene(1);
    }

    public void OnePlayers()
    {
        numPlayers = 1;
        ResetScore();
        SceneManager.LoadScene(1);
    }

    public void QuitToTitle(InputAction.CallbackContext context)
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
            SceneManager.LoadScene(0);
    }

    public void Exit()
    {

        Application.Quit();
    }

    private void ResetScore()
    {
        firstPlayerScore = 0;
        secondPlayerScore = 0;
        ChangeScore();
    }
}
