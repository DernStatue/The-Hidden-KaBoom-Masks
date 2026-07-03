using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 25f;

    public float lifeTime = 5f;

    bool hasHit;

    void Start()
    {
        Destroy(
            gameObject,
            lifeTime
        );
    }

    void OnTriggerEnter(
        Collider other
    )
    {
        TryDamage(
            other.transform
        );
    }

    void OnCollisionEnter(
        Collision collision
    )
    {
        TryDamage(
            collision.transform
        );
    }

    void TryDamage(
        Transform hit
    )
    {
        if (hasHit)
        {
            return;
        }

        hasHit = true;

        EnemyHealth enemy =
            hit.GetComponentInParent<
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