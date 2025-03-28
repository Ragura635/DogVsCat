using UnityEngine;
using UnityEngine.Pool;

public class FoodObjectPoolManager : MonoBehaviour
{
    public IObjectPool<GameObject> objectPool { get; private set; }
    public static FoodObjectPoolManager instance;

    //풀링할 오브젝트 프리팹
    [SerializeField] private GameObject prefab;

    private const int defaultCapacity = 20;  //초기 풀 크기
    private const int maxSize = 50;         //풀 최대 크기


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        objectPool = new ObjectPool<GameObject>(
            createFunc,         //새로운 오브젝트 생성
            actionOnGet,        //objectPool.Get(obj) 하면 실행
            actionOnRelease,    //objectPool.Release(obj) 하면 실행
            actionOnDestroy,    //풀에서 삭제될 때 실행할 함수
            true, defaultCapacity, maxSize);

        //미리 오브젝트 생성해두기
        for (int i = 0; i < defaultCapacity; i++)
        {
            objectPool.Release(createFunc());
        }
    }

    //새로운 오브젝트 생성
    private GameObject createFunc()
    {
        GameObject obj = Instantiate(prefab);
        obj.SetActive(false); // 처음에는 비활성화 상태로 생성
        return obj;
    }

    //objectPool.Get(obj) 하면 실행
    private void actionOnGet(GameObject obj)
    {
        obj.SetActive(true);
    }

    //objectPool.Release(obj) 하면 실행
    private void actionOnRelease(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.SetParent(transform);
    }

    //풀에서 삭제될 때 실행할 함수
    private void actionOnDestroy(GameObject obj)
    {
        Destroy(obj);
    }

    public GameObject GetObject()
    {
        GameObject obj = objectPool.Get();
        return obj;
    }

    public GameObject GetObject(Vector2 vec)
    {
        GameObject obj = objectPool.Get();
        obj.transform.position = vec;
        obj.transform.rotation = Quaternion.identity;
        return obj;
    }

    public void ReleaseObject(GameObject obj)
    {
        if (objectPool != null)
        {
            objectPool.Release(obj);
        }
    }
}