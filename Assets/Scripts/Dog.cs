using UnityEngine;

public class Dog : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating(nameof(makeFood), 0f, 0.2f);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float x = mousPos.x;
        if (x > 8.5f){ x = 8.5f; }
        else if (x < -8.5f){ x = -8.5f; }
        transform.position = new Vector2(x, transform.position.y);
    }

    void makeFood()
    {
        if (FoodObjectPoolManager.instance != null)
        {
            float x = transform.position.x;
            float y = transform.position.y + 2.0f;
            GameObject square = FoodObjectPoolManager.instance.GetObject(new Vector2(x,y));
        }
    }
}
