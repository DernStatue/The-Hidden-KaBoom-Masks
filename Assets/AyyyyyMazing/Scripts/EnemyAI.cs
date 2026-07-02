using UnityEngine;

public class SimpleEnemyAI : MonoBehaviour
{
    public Transform player;

    public float moveSpeed = 3f;

    public float detectionRange = 15f;

    public float attackRange = 2f;

    public float rotationSpeed = 5f;

    public float damage = 10f;

    public float attackCooldown = 1f;

    private float nextAttackTime;

    void Start()
    {
        if (player == null)
        {
            GameObject playerObj =
                GameObject.FindGameObjectWithTag(
                    "Player"
                );

            if (playerObj != null)
            {
                player =
                    playerObj.transform;
            }
        }
    }

    void Update()
    {
        if (player == null)
            return;

        float distance =
            Vector3.Distance(
                transform.position,
                player.position
            );

        if (
            distance >
            detectionRange
        )
        {
            return;
        }

        Vector3 direction =
            (
                player.position -
                transform.position
            ).normalized;

        direction.y = 0f;

        Quaternion targetRotation =
            Quaternion.LookRotation(
                direction
            );

        transform.rotation =
            Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed *
                Time.deltaTime
            );

        if (
            distance >
            attackRange
        )
        {
            transform.position +=
                transform.forward *
                moveSpeed *
                Time.deltaTime;
        }
        else
        {
            Attack();
        }
    }

    void Attack()
    {
        if (
            Time.time <
            nextAttackTime
        )
        {
            return;
        }

        nextAttackTime =
            Time.time +
            attackCooldown;

        PlayerHealth health =
            player.GetComponent<
                PlayerHealth>();

        if (health != null)
        {
            health.TakeDamage(
                damage
            );
        }
    }
}