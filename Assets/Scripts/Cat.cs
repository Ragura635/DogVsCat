using UnityEngine;

public class Cat : MonoBehaviour
{
    public GameObject hungryCat;
    public GameObject fullCat;

    public RectTransform front;

    float full;
    float energy;
    bool isFull;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        energy = 0.0f;
        full = 5.0f;
        isFull = false;
        front.localScale = new Vector3(energy / full, 1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (energy < full)
        {
            transform.position += Vector3.down * 0.05f;
        }
        else
        {
            if(transform.position.x > 0)
            {
                transform.position += Vector3.right * 0.05f;
            }
            else
            {
                transform.position += Vector3.left * 0.05f;
            }
        }

        if(transform.position.y < -16.0f)
        {
            GameManager.instance.GameOver();
        }

        if(transform.position.x > 18 || transform.position.x < -18)
        {
            CatObjectPoolManager.instance.ReleaseObject(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Food"))
        {
            if(energy < full)
            {
                energy += 1.0f;
                front.localScale = new Vector3(energy / full, 1, 1);
                Destroy(collision.gameObject);

                if(energy >= full)
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
