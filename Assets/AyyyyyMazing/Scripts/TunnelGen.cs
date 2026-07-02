using UnityEngine;

public class TunnelGenerator : MonoBehaviour
{
    [Header("Seed")]
    public int seed = 12345;

    [Header("Generation")]
    public int maxPieces = 80;

    public float noiseScale = 0.15f;

    public float turnThreshold = 0.65f;

    public float rampUpThreshold = 0.75f;

    public float rampDownThreshold = 0.25f;

    [Header("Pieces")]
    public TunnelPiece startPiece;

    public TunnelPiece[] straightPieces;

    public TunnelPiece[] leftTurnPieces;

    public TunnelPiece[] rightTurnPieces;

    public TunnelPiece[] rampUpPieces;

    public TunnelPiece[] rampDownPieces;

    public TunnelPiece[] roomPieces;

    public TunnelPiece deadEndPiece;

    public TunnelPiece bossRoom;

    [Header("Rooms")]
    [Range(0f, 1f)]
    public float roomChance = 0.08f;

    [Header("Enemies")]
    public GameObject enemyPrefab;

    [Range(0f, 1f)]
    public float enemySpawnChance = 0.35f;

    private System.Random rng;

    private Transform currentExit;

    private float noiseOffsetA;

    private float noiseOffsetB;

    void Start()
    {
        Generate();
    }

    void Generate()
    {
        rng =
            new System.Random(seed);

        noiseOffsetA =
            seed * 0.173f;

        noiseOffsetB =
            seed * 0.827f;

        TunnelPiece first =
            Instantiate(
                startPiece,
                Vector3.zero,
                Quaternion.identity
            );

        RegisterPiece(first);

        if (
            first.exits.Length == 0
        )
        {
            Debug.LogError(
                "Start piece has no exits!"
            );

            return;
        }

        currentExit =
            first.exits[0];

        for (
            int i = 0;
            i < maxPieces;
            i++
        )
        {
            TunnelPiece prefab =
                ChoosePiece(i);

            if (prefab == null)
            {
                continue;
            }

            TunnelPiece piece =
                Instantiate(prefab);

            AlignPiece(
                piece,
                currentExit
            );

            RegisterPiece(piece);

            if (
                piece.exits.Length > 0
            )
            {
                currentExit =
                    piece.exits[
                        rng.Next(
                            piece.exits.Length
                        )
                    ];
            }
        }

        if (
            bossRoom != null
        )
        {
            TunnelPiece boss =
                Instantiate(
                    bossRoom
                );

            AlignPiece(
                boss,
                currentExit
            );
        }
        else if (
            deadEndPiece != null
        )
        {
            TunnelPiece end =
                Instantiate(
                    deadEndPiece
                );

            AlignPiece(
                end,
                currentExit
            );
        }
    }

    TunnelPiece ChoosePiece(
        int index
    )
    {
        float verticalNoise =
            Mathf.PerlinNoise(
                noiseOffsetA +
                index * noiseScale,
                0f
            );

        float turnNoise =
            Mathf.PerlinNoise(
                noiseOffsetB +
                index * noiseScale,
                100f
            );

        if (
            rng.NextDouble() <
            roomChance &&
            roomPieces.Length > 0
        )
        {
            return roomPieces[
                rng.Next(
                    roomPieces.Length
                )
            ];
        }

        if (
            verticalNoise >
            rampUpThreshold
        )
        {
            if (
                rampUpPieces.Length > 0
            )
            {
                return rampUpPieces[
                    rng.Next(
                        rampUpPieces.Length
                    )
                ];
            }
        }

        if (
            verticalNoise <
            rampDownThreshold
        )
        {
            if (
                rampDownPieces.Length > 0
            )
            {
                return rampDownPieces[
                    rng.Next(
                        rampDownPieces.Length
                    )
                ];
            }
        }

        if (
            turnNoise >
            turnThreshold
        )
        {
            if (
                rightTurnPieces.Length > 0
            )
            {
                return rightTurnPieces[
                    rng.Next(
                        rightTurnPieces.Length
                    )
                ];
            }
        }

        if (
            turnNoise <
            1f - turnThreshold
        )
        {
            if (
                leftTurnPieces.Length > 0
            )
            {
                return leftTurnPieces[
                    rng.Next(
                        leftTurnPieces.Length
                    )
                ];
            }
        }

        if (
            straightPieces.Length > 0
        )
        {
            return straightPieces[
                rng.Next(
                    straightPieces.Length
                )
            ];
        }

        return null;
    }

    void RegisterPiece(
        TunnelPiece piece
    )
    {
        piece.SpawnProps(rng);

        SpawnEnemies(piece);
    }

    void SpawnEnemies(
        TunnelPiece piece
    )
    {
        if (
            enemyPrefab == null
        )
        {
            return;
        }

        if (
            piece.enemySpawnPoints == null
        )
        {
            return;
        }

        foreach (
            Transform spawnPoint
            in piece.enemySpawnPoints
        )
        {
            if (
                rng.NextDouble() >
                enemySpawnChance
            )
            {
                continue;
            }

            Instantiate(
                enemyPrefab,
                spawnPoint.position,
                spawnPoint.rotation
            );
        }
    }

    void AlignPiece(
        TunnelPiece piece,
        Transform targetExit
    )
    {
        Quaternion rotation =
            targetExit.rotation *
            Quaternion.Inverse(
                piece.entrance.rotation
            );

        piece.transform.rotation =
            rotation;

        Vector3 offset =
            targetExit.position -
            piece.entrance.position;

        piece.transform.position +=
            offset;
    }
}