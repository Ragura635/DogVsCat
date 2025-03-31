using System.Drawing;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.Pool;

public class CatObjectPoolManager : MonoBehaviour
{
    public IObjectPool<GameObject>[] objectPools { get; private set; }
    public static CatObjectPoolManager instance;

    //풀링할 오브젝트 프리팹
    [SerializeField] private GameObject[] prefabs; // 3가지 프리팹 (normal, fat, pirate)

    private const int defaultCapacity = 20;  //초기 풀 크기
    private const int maxSize = 150;         //풀 최대 크기

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        objectPools = new ObjectPool<GameObject>[3];

        //타입별로 오브젝트 생성해두기
        for(int i=0; i<3; i++)
        {
            int typeIndex = i;
            objectPools[i] = new ObjectPool<GameObject>(
                () => createFunc(typeIndex),    //새로운 오브젝트 생성
            actionOnGet,                        //objectPool.Get(obj) 하면 실행
            actionOnRelease,                    //objectPool.Release(obj) 하면 실행
            actionOnDestroy,                    //풀에서 삭제될 때 실행할 함수
                true, defaultCapacity, maxSize);

            for (int j = 0; j < defaultCapacity; j++)
            {
                objectPools[i].Release(createFunc(typeIndex));
            }
        }
    }

    //새로운 오브젝트 생성
    private GameObject createFunc(int type)
    {
        GameObject obj = Instantiate(prefabs[type]);
        obj.SetActive(false);
        obj.GetComponent<Cat>().type = type; // Cat 타입 설정
        return obj;
    }

    //objectPool.Get(obj) 하면 실행
    private void actionOnGet(GameObject obj)
    {
        obj.SetActive(true);

        //cat 위치 및 방향 초기화
        float x = Random.Range(-9.0f, 9.0f);
        float y = 30.0f;
        obj.transform.position = new Vector2(x, y);
        obj.transform.rotation = Quaternion.identity;

        //cat 상태 초기화
        Cat cat = obj.GetComponent<Cat>();
        if (cat != null)
        {
            cat.ResetState();
        }
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

    public GameObject GetObject(int type)
    {
        if (type < 0 || type >= objectPools.Length) return null;
        return objectPools[type].Get();
    }

    public void ReleaseObject(GameObject obj)
    {
        Cat cat = obj.GetComponent<Cat>();
        if (cat != null && cat.type < objectPools.Length)
        {
            objectPools[cat.type].Release(obj);
        }
        else
        {
            Destroy(obj);
        }
    }
}