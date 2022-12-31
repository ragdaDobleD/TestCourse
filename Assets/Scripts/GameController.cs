using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    //S�ngleton de la clase Game Controller
    public static GameController _sharedInstance;

    [HideInInspector]
    public List<TargetBehaviour> targets = new List<TargetBehaviour>();

    private float timeRemaining;
    public Text timeText;

    private int score;
    public Text scoreText;
    public Text highScoreText;

    private void Awake()
    {
        _sharedInstance = this;
        timeRemaining = 60;
        timeText.text = timeRemaining.ToString();
    }
    void Start()
    {
        iTween.ValueTo(
            gameObject,
            iTween.Hash(
                "from", timeRemaining, 
                "to", 0,
                "time", timeRemaining,
                "onupdatetarget", gameObject,
                "onupdate", "TimeUpdate",
                "oncomplete",  "GameOver"
                )
            );

        StartCoroutine("SpawnTargets");

        //Recupero la puntuaci�n m�xima de la preferencias del jugador
        int maxscore = PlayerPrefs.GetInt("highscore", 0);
        highScoreText.text = "High Score: " + maxscore.ToString();

        //Configuramos la puntuaci�n actual
        score = 0;
        scoreText.text = "Score: " + score.ToString();


        print(PlayerPrefs.GetString("Player Name", "Juan Gabriel"));
    }

    private void TimeUpdate(float newTime)
    {
        timeRemaining = newTime;

        if (timeRemaining > 10)
        {
            timeText.text = timeRemaining.ToString("#"); //Parte entera del n�mero
        }
        else
        {
            timeText.color = Color.red;
            timeText.text = timeRemaining.ToString("#.0"); //Un solo decimal
        }
    }

    private void GameOver()
    {
        StopCoroutine("SpawnTargets");

        timeText.color = Color.black;
        timeText.text = "GAME OVER";
    }

    private void SpawnTarget()
    {
        int idx = Random.Range(0, targets.Count); //Genera un nn�mero aleatorio entre 0 y targets.Count -1
        TargetBehaviour target = targets[idx];
        target.ShowTarget();
    }
    IEnumerator SpawnTargets()
    {
        yield return new WaitForSeconds(1.0f);

        while(true){
            int numberIfTargets = Random.Range(2, 5);

            for (int i = 0; i < numberIfTargets; i++)
            {
                SpawnTarget();
            }

            float timeToSleep = Random.Range(0.5f * numberIfTargets, 3.5f);

            yield return new WaitForSeconds(timeToSleep);
        }

    }

    public void AddScore()
    {
        score++;

        scoreText.text = "Score: " + score.ToString();

        if (score > PlayerPrefs.GetInt("highscore", 0))
        {
            PlayerPrefs.SetInt("highscore", score);

            highScoreText.text = "High Score: " + score.ToString();
        }
    }
}
