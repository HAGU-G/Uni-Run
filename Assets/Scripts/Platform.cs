using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;


//1. ��ֹ� On/Off
//2. �÷��� ������ ���� �߰�
//3. �̵�
//4. ������Ʈ Ǯ��

public class Platform : MonoBehaviour
{
    public float speed = 10f;

    public List<GameObject> obstacles;
    public float onOffPercentage = 0.3f;

    private GameManager gameManager;
    private bool stepped = false;
    private Vector2 startPos = new(14.74f, -3.65f);

    public IObjectPool<Platform> pp;

    private void OnEnable()
    {
        foreach (var obstacle in obstacles)
        {
            obstacle.SetActive(Random.value < onOffPercentage);
        }
        stepped = false;
        transform.position = startPos + Vector2.up * Random.Range(0f, 3f);
    }

    private void Start()
    {
        gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
    }

    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);
        if (transform.position.x < -20)
        {
            gameObject.SetActive(false);
            pp.Release(this);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!stepped
            && !gameManager.IsGameOver
            && collision.collider.CompareTag("Player")
            && collision.contacts[0].normal == Vector2.down)
        {
            gameManager.Score += 1;
            stepped = true;
        }
    }
}
