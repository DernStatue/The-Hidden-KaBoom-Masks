using UnityEngine;

public class TunnelPiece : MonoBehaviour
{
    [Header("Tunnel Connections")]
    public Transform entrance;

    public Transform[] exits;

    [Header("Occupied Cells")]
    public Vector3Int[] occupiedCells;

    [Header("Enemy Spawns")]
    public Transform[] enemySpawnPoints;

    [Header("Overlap Check")]
    public BoxCollider overlapCollider;

    [Header("Props")]
    public BoxCollider propVolume;

    [Range(0f, 1f)]
    public float propSpawnChance = 0.4f;

    public int propAttempts = 10;

    public GameObject[] possibleProps;

    [Header("Prop Placement")]
    public LayerMask floorLayerMask = ~0;

    public float raycastStartHeight = 2f;

    public float raycastDistance = 20f;

    [Range(0f, 90f)]
    public float maxFloorAngle = 45f;

    public void SpawnProps(System.Random rng)
    {
        if (possibleProps.Length == 0)
            return;

        if (propVolume == null)
            return;

        Vector3 halfSize =
            propVolume.size * 0.5f;

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

            Vector3 localPoint =
                propVolume.center +
                new Vector3(
                    (float)(
                        rng.NextDouble() *
                        2.0 - 1.0
                    ) * halfSize.x,

                    halfSize.y +
                    raycastStartHeight,

                    (float)(
                        rng.NextDouble() *
                        2.0 - 1.0
                    ) * halfSize.z
                );

            Vector3 worldStart =
                propVolume.transform
                .TransformPoint(
                    localPoint
                );

            Vector3 worldDown =
                -propVolume.transform.up;

            if (
                Physics.Raycast(
                    worldStart,
                    worldDown,
                    out RaycastHit hit,
                    raycastDistance,
                    floorLayerMask,
                    QueryTriggerInteraction
                    .Ignore
                )
            )
            {
                if (
                    !hit.transform
                    .IsChildOf(transform)
                )
                {
                    continue;
                }

                float angle =
                    Vector3.Angle(
                        hit.normal,
                        propVolume.transform.up
                    );

                if (
                    angle >
                    maxFloorAngle
                )
                {
                    continue;
                }

                Quaternion randomRotation =
                    Quaternion.Euler(
                        0f,
                        rng.Next(0, 360),
                        0f
                    );

                Instantiate(
                    selectedProp,
                    hit.point,
                    randomRotation,
                    transform
                );
            }
        }
    }
}