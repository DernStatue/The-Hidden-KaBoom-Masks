using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 80f;

    public float damage = 25f;

    public float lifetime = 5f;

    void Start()
    {
        Destroy(
            gameObject,
            lifetime
        );
    }

    void Update()
    {
        transform.position +=
            transform.forward *
            speed *
            Time.deltaTime;
    }

    void OnTriggerEnter(
        Collider other
    )
    {
        EnemyHealth enemy =
            other.GetComponent<
                EnemyHealth>();

        if (enemy != null)
        {
            enemy.TakeDamage(
                damage
            );
        }

        Destroy(gameObject);
    }
}