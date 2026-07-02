using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float health = 100f;

    public void TakeDamage(
        float damage
    )
    {
        health -= damage;

        Debug.Log(
            "Player Health: " +
            health
        );

        if (health <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log(
            "Player Died"
        );
    }
}