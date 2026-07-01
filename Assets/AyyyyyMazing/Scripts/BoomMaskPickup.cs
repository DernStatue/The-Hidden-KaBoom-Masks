using UnityEngine;

public class BoomMaskPickup : MonoBehaviour
{
    public KeyCode interactKey =
        KeyCode.E;

    private bool playerNearby;

    void Update()
    {
        if (
            playerNearby &&
            Input.GetKeyDown(interactKey)
        )
        {
            PlayerCombat combat =
                FindObjectOfType<PlayerCombat>();

            if (combat != null)
            {
                combat.UnlockBoomMask();
            }

            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (
            other.CompareTag("Player")
        )
        {
            playerNearby = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (
            other.CompareTag("Player")
        )
        {
            playerNearby = false;
        }
    }
}