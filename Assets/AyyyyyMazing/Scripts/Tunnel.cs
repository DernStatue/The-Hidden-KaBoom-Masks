using UnityEngine;

public class TunnelPiece : MonoBehaviour
{
    [Header("Connections")]
    public Transform entrance;

    public Transform[] exits;

    [Header("Grid Shape")]
    public Vector3Int[] occupiedCells;

    [Header("Generation")]
    public bool allowBranching = true;

    [Header("Props")]
    public BoxCollider propVolume;

    public GameObject[] possibleProps;

    [Range(0f, 1f)]
    public float propSpawnChance = 0.35f;

    public int propAttempts = 6;

    public void SpawnProps(
        System.Random rng
    )
    {
        if (
            possibleProps == null ||
            possibleProps.Length == 0
        )
        {
            return;
        }

        if (propVolume == null)
            return;

        Bounds bounds =
            propVolume.bounds;

        for (
            int i = 0;
            i < propAttempts;
            i++
        )
        {
            if (
                rng.NextDouble() >
                propSpawnChance
            )
            {
                continue;
            }

            GameObject selectedProp =
                possibleProps[
                    rng.Next(
                        possibleProps.Length
                    )
                ];

            Vector3 randomPos =
                new Vector3(
                    Random.Range(
                        bounds.min.x,
                        bounds.max.x
                    ),

                    bounds.max.y + 2f,

                    Random.Range(
                        bounds.min.z,
                        bounds.max.z
                    )
                );

            if (
                Physics.Raycast(
                    randomPos,
                    Vector3.down,
                    out RaycastHit hit,
                    20f
                )
            )
            {
                if (
                    !hit.transform.IsChildOf(
                        transform
                    )
                )
                {
                    continue;
                }

                Quaternion rotation =
                    Quaternion.Euler(
                        0f,
                        rng.Next(0, 360),
                        0f
                    );

                Instantiate(
                    selectedProp,
                    hit.point,
                    rotation,
                    transform
                );
            }
        }
    }
}