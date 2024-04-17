using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PlatformSpawner : MonoBehaviour
{
    public GameObject platform;
    private List<GameObject> platforms = new();
    public Platform pl;
    private IObjectPool<Platform> pp;

    private float timer = 0f;
    private float spawnInterval = 0f;
    public float spawnIntervalMin = 1f;
    public float spawnIntervalMax = 3f;


    void Start()
    {
        //for (int i = 0; i < 10; i++)
        //{
        //    var p = Instantiate(platform);
        //    p.gameObject.SetActive(false);
        //    platforms.Add(p);
        //}

        pp = new ObjectPool<Platform>(
            CreatePooledItem, // 생성(오브젝트 풀에 없는 경우)
            OnTakeFromPool, // 활성(오브젝트 풀에서 꺼냄)
            OnReturnToPool, // 비활성(오브젝트 풀에 넣음)
            OnDestroyPoolObject, // 삭제
            true, 10, 1000);
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            spawnInterval = Random.Range(spawnIntervalMin, spawnIntervalMax);
            pp.Get();

            //foreach (var platform in platforms)
            //{
            //    if(!platform.gameObject.activeSelf)
            //    {
            //        platform.gameObject.SetActive(true);
            //        break;
            //    }
            //}
        }
    }

    private Platform CreatePooledItem()
    {
        var p = Instantiate(pl);
        p.pp = this.pp;
        return p;
    }

    private void OnTakeFromPool(Platform platform)
    {
        platform.gameObject.SetActive(true);
    }

    private void OnReturnToPool(Platform platform)
    {
        platform.gameObject.SetActive(false);

    }

    private void OnDestroyPoolObject(Platform platform)
    {
        Destroy(platform);
    }
}