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

    void MakeCat()
    {
        if (CatObjectPoolManager.instance != null)
        {
            //기본 고양이 생성
            CatObjectPoolManager.instance.GetObject(0);

            if (level == 1 && (Random.Range(0, 10) < 2))
            {
                //lv.1 20%확률로 기본 고양이 추가 생성
                CatObjectPoolManager.instance.GetObject(0);
            }
            else if (level == 2 && (Random.Range(0, 10) < 5))
            {
                //lv.2 50%확률로 기본 고양이 추가 생성
                CatObjectPoolManager.instance.GetObject(0);
            }
            else if (level == 3)
            {
                //lv.3 추가로 뚱뚱한 고양이 생성
                CatObjectPoolManager.instance.GetObject(1);
            }

            if (level == 4)
            {
                //lv.4 추가로 해적 고양이 생성
                CatObjectPoolManager.instance.GetObject(2);
            }
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
        //레벨경험치(?)는 스코어를 5로 나눈 나머지값을 이용
        levelFront.localScale = new Vector3((score % 5.0f)/5.0f, 1f, 1f);
    }
}
