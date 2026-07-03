using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100f;

    public GameObject deathEffect;

    float currentHealth;

    bool dead;

    void Start()
    {
        currentHealth =
            maxHealth;
    }

    public void TakeDamage(
        float damage
    )
    {
        if (dead)
        {
            return;
        }

        currentHealth -= damage;

        Debug.Log(
            gameObject.name +
            " took damage: " +
            damage
        );

        if (
            currentHealth <= 0
        )
        {
            Die();
        }
    }

    void Die()
    {
        dead = true;

        if (
            deathEffect != null
        )
        {
            Instantiate(
                deathEffect,
                transform.position,
                Quaternion.identity
            );
        }

        Destroy(gameObject);
    }
}