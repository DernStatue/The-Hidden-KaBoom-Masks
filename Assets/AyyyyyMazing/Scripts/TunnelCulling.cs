using System.Collections;
using UnityEngine;

public class TunnelChunkCulling : MonoBehaviour
{
    public float disableDistance = 120f;

    private Transform player;

    private Renderer[] renderers;
    private Collider[] colliders;

    void Start()
    {
        GameObject playerObject =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        renderers =
            GetComponentsInChildren<Renderer>();

        colliders =
            GetComponentsInChildren<Collider>();

        StartCoroutine(CheckDistance());
    }

    IEnumerator CheckDistance()
    {
        while (true)
        {
            if (player != null)
            {
                float distance =
                    Vector3.Distance(
                        player.position,
                        transform.position
                    );

                bool visible =
                    distance < disableDistance;

                foreach (Renderer r in renderers)
                {
                    r.enabled = visible;
                }

                foreach (Collider c in colliders)
                {
                    c.enabled = visible;
                }
            }

            yield return new WaitForSeconds(0.25f);
        }
    }
}