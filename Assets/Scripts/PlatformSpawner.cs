using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    public GameObject platform;
    private List<GameObject> platforms = new();

    private float timer = 0f;
    private float spawnInterval = 0f;
    public float spawnIntervalMin = 1f;
    public float spawnIntervalMax = 3f;


    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            var p = Instantiate(platform);
            p.gameObject.SetActive(false);
            platforms.Add(p);
        }
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            spawnInterval = Random.Range(spawnIntervalMin, spawnIntervalMax);
            foreach (var platform in platforms)
            {
                if(!platform.gameObject.activeSelf)
                {
                    platform.gameObject.SetActive(true);
                    break;
                }
            }
        }
    }
}