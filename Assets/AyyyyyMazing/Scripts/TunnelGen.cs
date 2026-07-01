using System.Collections.Generic;
using UnityEngine;

public class TunnelGenerator : MonoBehaviour
{
    [Header("Generation")]
    public int seed = 12345;

    public int maxPieces = 80;

    public float gridSize = 36f;

    [Header("Pieces")]
    public TunnelPiece[] straightPieces;

    public TunnelPiece[] turnPieces;

    public TunnelPiece[] rampUpPieces;

    public TunnelPiece[] rampDownPieces;

    public TunnelPiece[] roomPieces;

    public TunnelPiece deadEndPiece;

    public TunnelPiece bossRoom;

    private System.Random rng;

    private List<Transform> openExits =
        new List<Transform>();

    private HashSet<Vector3Int> occupiedCells =
        new HashSet<Vector3Int>();

    private Transform furthestExit;

    private float furthestDistance;

    void Start()
    {
        Generate();
    }

    void Generate()
    {
        rng = new System.Random(seed);

        TunnelPiece startPiece =
            Instantiate(
                straightPieces[0],
                Vector3.zero,
                Quaternion.identity
            );

        RegisterPiece(startPiece);

        foreach (
            Transform exit
            in startPiece.exits
        )
        {
            openExits.Add(exit);
        }

        int placedPieces = 1;

        while (
            openExits.Count > 0 &&
            placedPieces < maxPieces
        )
        {
            int index =
                rng.Next(openExits.Count);

            Transform targetExit =
                openExits[index];

            openExits.RemoveAt(index);

            bool success =
                TryPlacePiece(targetExit);

            if (success)
            {
                placedPieces++;
            }
            else
            {
                SpawnDeadEnd(
                    targetExit
                );
            }
        }

        if (furthestExit != null)
        {
            SpawnBossRoom(
                furthestExit
            );
        }
    }

    bool TryPlacePiece(
        Transform targetExit
    )
    {
        List<TunnelPiece> candidates =
            new List<TunnelPiece>();

        candidates.AddRange(
            straightPieces
        );

        candidates.AddRange(
            turnPieces
        );

        // occasional ramps
        if (rng.NextDouble() < 0.25)
        {
            candidates.AddRange(
                rampUpPieces
            );

            candidates.AddRange(
                rampDownPieces
            );
        }

        // occasional rooms
        if (rng.NextDouble() < 0.12)
        {
            candidates.AddRange(
                roomPieces
            );
        }

        for (
            int i = 0;
            i < 12;
            i++
        )
        {
            TunnelPiece prefab =
                candidates[
                    rng.Next(
                        candidates.Count
                    )
                ];

            TunnelPiece piece =
                Instantiate(prefab);

            AlignPieceToExit(
                piece,
                targetExit
            );

            if (
                !CanPlacePiece(piece)
            )
            {
                Destroy(
                    piece.gameObject
                );

                continue;
            }

            RegisterPiece(piece);

            foreach (
                Transform exit
                in piece.exits
            )
            {
                openExits.Add(exit);
            }

            return true;
        }

        return false;
    }

    bool CanPlacePiece(
        TunnelPiece piece
    )
    {
        foreach (
            Vector3Int localCell
            in piece.occupiedCells
        )
        {
            Vector3 worldPos =
                piece.transform.position +
                piece.transform.rotation *
                Vector3.Scale(
                    localCell,
                    Vector3.one *
                    gridSize
                );

            Vector3Int cell =
                WorldToGrid(
                    worldPos
                );

            if (
                occupiedCells.Contains(
                    cell
                )
            )
            {
                return false;
            }
        }

        return true;
    }

    void RegisterPiece(
        TunnelPiece piece
    )
    {
        foreach (
            Vector3Int localCell
            in piece.occupiedCells
        )
        {
            Vector3 worldPos =
                piece.transform.position +
                piece.transform.rotation *
                Vector3.Scale(
                    localCell,
                    Vector3.one *
                    gridSize
                );

            occupiedCells.Add(
                WorldToGrid(
                    worldPos
                )
            );
        }

        float distance =
            Vector3.Distance(
                Vector3.zero,
                piece.transform.position
            );

        if (
            distance >
            furthestDistance
        )
        {
            furthestDistance =
                distance;

            if (
                piece.exits.Length > 0
            )
            {
                furthestExit =
                    piece.exits[0];
            }
        }

        piece.SpawnProps(rng);
    }

    void SpawnDeadEnd(
        Transform exit
    )
    {
        TunnelPiece piece =
            Instantiate(
                deadEndPiece
            );

        AlignPieceToExit(
            piece,
            exit
        );
    }

    void SpawnBossRoom(
        Transform exit
    )
    {
        TunnelPiece piece =
            Instantiate(
                bossRoom
            );

        AlignPieceToExit(
            piece,
            exit
        );
    }

    void AlignPieceToExit(
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

    Vector3Int WorldToGrid(
        Vector3 position
    )
    {
        return new Vector3Int(
            Mathf.RoundToInt(
                position.x /
                gridSize
            ),

            Mathf.RoundToInt(
                position.y /
                gridSize
            ),

            Mathf.RoundToInt(
                position.z /
                gridSize
            )
        );
    }
}