using Unity.Properties;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    //public GameObject normalCat;
    public GameObject retryBtn;
    public Text levelTxt;
    public RectTransform levelFront;

    int level = 0;
    int score = 0;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        if (instance == null)
        {
            instance = this;
        }
        Time.timeScale = 1.0f;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        retryBtn.SetActive(false);
        InvokeRepeating(nameof(MakeCat), 0f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void MakeCat()
    {
        if (CatObjectPoolManager.instance != null)
        {
            float x = Random.Range(-9.0f, 9.0f);
            float y = 30.0f;

            Vector2 catPos = new Vector2(x, y);
            GameObject cat = CatObjectPoolManager.instance.GetObject(catPos);
        }
    }

    public void GameOver()
    {
        retryBtn.SetActive(true);
        Time.timeScale = 0.0f;
    }

    public void AddScore()
    {
        score++;
        level = score / 5;
        levelTxt.text = level.ToString();
        levelFront.localScale = new Vector3((score % 5.0f)/5.0f, 1f, 1f);
    }
}
