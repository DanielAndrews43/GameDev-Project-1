using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class UIManager : MonoBehaviour {

    //Singleton UIManager.
    public static UIManager uiManager;

    GameObject LoadingBackground;
    Image LoadingLives;
    GameObject Score;
    List<Image> ScoreDigits = new List<Image>();
    GameObject Coins;
    List<Image> CoinsDigits = new List<Image>();
    GameObject Timer;
    List<Image> TimerDigits = new List<Image>();
    List<Image> LoadingLivesDigit = new List<Image>();
	Text livesLabel;
    string pathToSprites = "NES - Super Mario Bros - Font(Transparent)";
    public Object assetTest;
    Sprite[] numberSprites;
    float loadTime = 1.5f;
    int playerLives = 3;
    int playerScore = 0;
    int playerTime = 330;
    int playerCoins = 0;

    // Use this for initialization
    void Start()
    {

        if (uiManager == null)
        {
            uiManager = this;
        }
        else
        {
            Destroy(gameObject);
        }
		livesLabel = GameObject.Find("LivesLabel").GetComponent<Text>();
        LoadingBackground = GameObject.Find("Loading_Background");
        LoadingLives = GameObject.Find("Loading_Lives").GetComponent<Image>();
        LoadingLivesDigit.Add(LoadingLives);
        numberSprites = Resources.LoadAll(pathToSprites).OfType<Sprite>().Take(10).ToArray();
        Score = GameObject.Find("Score");
        foreach (Transform child in Score.transform)
        {
            ScoreDigits.Add(child.GetComponent<Image>());
        }
        Coins = GameObject.Find("Coins");
        foreach (Transform child in Coins.transform)
        {
            CoinsDigits.Add(child.GetComponent<Image>());
        }
        Timer = GameObject.Find("Timer");
        foreach (Transform child in Timer.transform)
        {
            TimerDigits.Add(child.GetComponent<Image>());
        }
        SetDigits(ScoreDigits, playerScore);
        SetDigits(CoinsDigits, playerCoins);
        SetDigits(TimerDigits, playerTime);
        SetDigits(LoadingLivesDigit, playerLives);
        LoadingBackground.SetActive(false);
    }

    /* Use coroutine for spending time on Loading screen. */
    IEnumerator ShowLoadingScreen() {
        LoadingBackground.SetActive(true);
        yield return new WaitForSeconds(loadTime);
        LoadingBackground.SetActive(false);
        SceneManager.LoadScene("Main Scene");
        StartCoroutine("KeepTime");
        yield break;
    }

    // Update is called once per frame
    void Update () {}

    void SetDigits(List<Image> digits, int result)
    {
        int placeCounter = 0;
        while (result / (int) Mathf.Pow(10, placeCounter) > 0) {
            placeCounter += 1;
        }
        placeCounter -= 1;
        IEnumerator<Image> digitsEnum = digits.GetEnumerator();
        digitsEnum.MoveNext();
        while (placeCounter >= 0) {
            int digit = result / (int) (Mathf.Pow(10, placeCounter));
            result = result % (int)(Mathf.Pow(10, placeCounter));
            if (digit >= 10) {
                int maxNum = 0;
                while (placeCounter >= 0) {
                    maxNum += (9 * (int) Mathf.Pow(10, placeCounter));
                    placeCounter -= 1;
                }
                SetDigits(digits, maxNum);
                return;
            }
            digitsEnum.Current.sprite = numberSprites[digit];
            digitsEnum.MoveNext();
            placeCounter -= 1;
        }
    }

    public void LoadScene() {
        playerTime = 330;
        SetDigits(TimerDigits, playerTime);
        StopCoroutine("KeepTime");
        StartCoroutine("ShowLoadingScreen");
    }

    public void UpdateScore(int scoreToAdd) {
        playerScore += scoreToAdd;
        SetDigits(ScoreDigits, playerScore);
    }

    IEnumerator KeepTime() {
        while (playerTime > 0)
        {
            yield return new WaitForSeconds(1);
            playerTime -= 1;
            SetDigits(TimerDigits, playerTime);
        }
        TakeLife();
        LoadScene();
        yield break;
    }

    public void TakeLife() {
        playerLives -= 1;
		livesLabel.text = "Lives: " + playerLives; 
        SetDigits(LoadingLivesDigit, playerLives);
        if (playerLives == 0) {
            print("load menu");
            //Do game over scene and back to Menu Scene?
            SceneManager.LoadScene("Menu Scene");
            playerLives = 3;
        }
        else {
            print("reloading!");
            LoadScene();
        }
    }

    public void AddCoin() {
        playerCoins += 1;
        SetDigits(CoinsDigits, playerCoins);
    }
}
