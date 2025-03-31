using System.Drawing;
using UnityEngine;

public class Cat : MonoBehaviour
{
    public GameObject hungryCat;
    public GameObject fullCat;
    public RectTransform front;
    public int type;

    float full, energy, speed;
    bool isFull;

    //풀에서 가져올때 초기화하는 함수
    public void ResetState()
    {
        energy = 0.0f;
        isFull = false;
        front.localScale = new Vector3(0, 1, 1);
        fullCat.SetActive(false);
        hungryCat.SetActive(true);

        switch (type)
        {
            case 0: // 기본 고양이
                full = 5.0f;
                speed = 0.05f;
                transform.localScale = new Vector2(1.0f, 1.0f);
                break;
            case 1: // 뚱뚱한 고양이
                full = 10.0f;
                speed = 0.02f;
                transform.localScale = new Vector2(1.0f, 1.0f);
                break;
            case 2: // 해적 고양이
                full = 5.0f;
                speed = 0.1f;
                transform.localScale = new Vector2(0.8f, 0.8f);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isFull)
        {
            //체력이 비어있으면 아래로 이동
            transform.position += Vector3.down * speed;
        }
        else
        {
            //체력이 꽉차면 양 옆으로 이동
            if(transform.position.x > 0)
            {
                transform.position += Vector3.right * speed;
            }
            else
            {
                transform.position += Vector3.left * speed;
            }
        }
        //이동 후 일정좌표 값에 이르면 오브젝트 반환
        if (transform.position.x > 18 || transform.position.x < -18)
        {
            CatObjectPoolManager.instance.ReleaseObject(gameObject);
        }

        //일정 좌표값까지 내려오면 게임종료
        if (transform.position.y < -16.0f)
        {
            GameManager.instance.GameOver();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Food 태그와 만나면
        if (collision.gameObject.CompareTag("Food"))
        {
            if(energy < full)
            {
                //에너지가 꽉차있지 않으면 에너지 증가, Food 오브젝트 풀로 반환
                energy += 1.0f;
                front.localScale = new Vector3(energy / full, 1, 1);
                if (FoodObjectPoolManager.instance != null)
                {
                    FoodObjectPoolManager.instance.ReleaseObject(collision.gameObject);
                }
                else
                {
                    Destroy(collision.gameObject);
                }
                //에너지가 증가되어 꽉찬다면 점수증가
                if (energy >= full)
                {
                    isFull = true;
                    hungryCat.SetActive(false);
                    fullCat.SetActive(true);
                    GameManager.instance.AddScore();
                }
            }
        }
    }
}