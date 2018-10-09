using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    Text timerText;
    Text gameOverText;

    GameObject player;
    bool isOver;
    float secondsCount;
    int minuteCount;
    int hourCount;


    void Start()
    {
        Time.timeScale = 1.0f;
        player = GameObject.Find("car");
        timerText = GameObject.Find("TimerText").GetComponent<Text>();
        gameOverText = GameObject.Find("GameOverText").GetComponent<Text>();
    }

    void Update()
    {
        Timer();
        CheckGameOver();
    }

    void Timer()
    {
        secondsCount += Time.deltaTime;
        timerText.text = hourCount + "0:" + minuteCount + "0:" + (int)secondsCount;
        if (secondsCount >= 60)
        {
            minuteCount++;
            secondsCount = 0;
        }
        else if (minuteCount >= 60)
        {
            hourCount++;
            minuteCount = 0;
        }
    }

    void CheckGameOver()
    {
        
    }

    IEnumerator SetGameOver()
    {
        isOver = false;
        yield return new WaitForSecondsRealtime(1.0f);
        Restart();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void RestartLevel()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }
}
