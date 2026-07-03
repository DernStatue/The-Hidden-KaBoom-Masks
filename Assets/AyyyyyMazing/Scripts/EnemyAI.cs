using UnityEngine;

public class SimpleEnemyAI : MonoBehaviour
{
    [Header("References")]
    public Transform player;

    public Animator animator;

    [Header("Movement")]
    public float moveSpeed = 3f;

    public float sprintSpeed = 5f;

    public float detectionRange = 18f;

    public float loseRange = 25f;

    public float attackRange = 2f;

    public float rotationSpeed = 6f;

    [Header("Combat")]
    public float damage = 10f;

    public float attackCooldown = 1f;

    [Header("Effects")]
    public AudioSource audioSource;

    public AudioClip idleSound;

    public AudioClip chaseSound;

    public AudioClip attackSound;

    public AudioClip hurtSound;

    [Header("Behaviour")]
    public bool useWander = true;

    public float wanderRadius = 6f;

    public float wanderInterval = 3f;

    Vector3 wanderTarget;

    float wanderTimer;

    float nextAttackTime;

    bool chasing;

    bool attacking;

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

        PickWanderPoint();
    }

    void Update()
    {
        if (
            PauseMenu.IsPaused
        )
        {
            return;
        }

        if (player == null)
        {
            return;
        }

        float distance =
            Vector3.Distance(
                transform.position,
                player.position
            );

        if (
            distance <= detectionRange
        )
        {
            chasing = true;
        }
        else if (
            distance >= loseRange
        )
        {
            chasing = false;
        }

        if (chasing)
        {
            HandleChase(
                distance
            );
        }
        else
        {
            HandleWander();
        }
    }

    void HandleChase(
        float distance
    )
    {
        Vector3 direction =
            (
                player.position -
                transform.position
            ).normalized;

        direction.y = 0f;

        if (
            direction != Vector3.zero
        )
        {
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
        }

        if (
            distance > attackRange
        )
        {
            attacking = false;

            transform.position +=
                transform.forward *
                sprintSpeed *
                Time.deltaTime;

            if (animator != null)
            {
                animator.SetBool(
                    "Walking",
                    true
                );

                animator.SetBool(
                    "Attacking",
                    false
                );
            }
        }
        else
        {
            Attack();
        }
    }

    void HandleWander()
    {
        if (!useWander)
        {
            return;
        }

        wanderTimer -=
            Time.deltaTime;

        Vector3 direction =
            (
                wanderTarget -
                transform.position
            ).normalized;

        direction.y = 0f;

        if (
            direction != Vector3.zero
        )
        {
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
        }

        transform.position +=
            transform.forward *
            moveSpeed *
            Time.deltaTime;

        if (
            Vector3.Distance(
                transform.position,
                wanderTarget
            ) < 1f ||
            wanderTimer <= 0f
        )
        {
            PickWanderPoint();
        }

        if (animator != null)
        {
            animator.SetBool(
                "Walking",
                true
            );

            animator.SetBool(
                "Attacking",
                false
            );
        }
    }

    void PickWanderPoint()
    {
        wanderTimer =
            wanderInterval;

        Vector2 random =
            Random.insideUnitCircle *
            wanderRadius;

        wanderTarget =
            transform.position +
            new Vector3(
                random.x,
                0f,
                random.y
            );
    }

    void Attack()
    {
        if (attacking)
        {
            return;
        }

        if (
            Time.time <
            nextAttackTime
        )
        {
            return;
        }

        attacking = true;

        nextAttackTime =
            Time.time +
            attackCooldown;

        if (animator != null)
        {
            animator.SetBool(
                "Walking",
                false
            );

            animator.SetTrigger(
                "Attack"
            );

            animator.SetBool(
                "Attacking",
                true
            );
        }

        if (
            attackSound != null &&
            audioSource != null
        )
        {
            audioSource.PlayOneShot(
                attackSound
            );
        }

        PlayerHealth health =
            player.GetComponent<
                PlayerHealth>();

        if (health != null)
        {
            health.TakeDamage(
                damage
            );
        }

        Invoke(
            nameof(
                ResetAttack
            ),
            0.5f
        );
    }

    void ResetAttack()
    {
        attacking = false;
    }

    public void PlayHurtSound()
    {
        if (
            hurtSound != null &&
            audioSource != null
        )
        {
            audioSource.PlayOneShot(
                hurtSound
            );
        }
    }
}