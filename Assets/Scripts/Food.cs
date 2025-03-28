using UnityEngine;

public class Food : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.up*0.5f;

        if (this.transform.position.y > 27)
        {
            FoodObjectPoolManager.instance.ReleaseObject(gameObject);
        }
    }
}
