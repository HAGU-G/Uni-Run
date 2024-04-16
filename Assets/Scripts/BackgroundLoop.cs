using UnityEngine;

public class BackgroundLoop : MonoBehaviour
{
    private float width;
    public float speed = 10f;
    private void Awake()
    {
        width = GetComponent<BoxCollider2D>().size.x;
    }

    private void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        if (transform.position.x <= -width)
        {
            RePosition();
        }
    }

    private void RePosition()
    {
        transform.position += transform.right * width * 2;
    }
}
