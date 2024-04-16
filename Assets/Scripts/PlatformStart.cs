using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformStart : MonoBehaviour
{
    private float width;
    public float speed = 10f;

    private void Awake()
    {
        width = GetComponent<BoxCollider2D>().size.x;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        if (transform.position.x <= -width)
        {
            Destroy(gameObject);
        }
    }
}
