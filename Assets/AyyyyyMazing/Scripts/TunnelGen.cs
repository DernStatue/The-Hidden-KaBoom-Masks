using System.Collections.Generic;
using UnityEngine;

public class TunnelGenerator : MonoBehaviour
{
    [Header("Generation")]
    public int seed = 12345;

    public int maxPieces = 40;

    public float gridSize = 10f;

    [Header("Tunnel Pieces")]
    public TunnelPiece[] normalPieces;

    public TunnelPiece[] rampUpPieces;

    public TunnelPiece[] rampDownPieces;

    [Header("Special Pieces")]
    public TunnelPiece endPiece;

    [Header("Boss Room")]
    public TunnelPiece bossRoom;



    private System.Random rng;

    private List<Transform> openExits =
        new List<Transform>();

    private HashSet<Vector3Int> occupiedCells =
        new HashSet<Vector3Int>();

    private Transform furthestExit;

    private float furthestDistance = 0f;


    void Start()
    {
        Generate();
    }

    void Generate()
    {
        rng = new System.Random(seed);

        // Spawn first piece
        TunnelPiece firstPiece =
            Instantiate(
                normalPieces[0],
                Vector3.zero,
                Quaternion.identity
            );

        firstPiece.SpawnProps(rng);

        // Mark first cell occupied
        Vector3Int firstCell =
            WorldToGrid(firstPiece.transform.position);

        occupiedCells.Add(firstCell);

        // Add exits
        foreach (Transform exit in firstPiece.exits)
        {
            openExits.Add(exit);
        }

        int placedPieces = 1;

        while (openExits.Count > 0 &&
               placedPieces < maxPieces)
        {
            int exitIndex =
                rng.Next(openExits.Count);

            Transform targetExit =
                openExits[exitIndex];

            openExits.RemoveAt(exitIndex);

            bool placedSuccessfully =
            TryPlacePiece(targetExit);

            if (placedSuccessfully)
            {
                placedPieces++;
            }
            else
            {
                SpawnEndPiece(targetExit);
            }

        }

        if (furthestExit != null)
        {
            SpawnBossRoom(furthestExit);
        }

    }

    void SpawnBossRoom(Transform targetExit)
    {
        TunnelPiece newPiece =
            Instantiate(bossRoom);

        Quaternion rotationDifference =
            targetExit.rotation *
            Quaternion.Inverse(
                newPiece.entrance.rotation
            );

        newPiece.transform.rotation =
            rotationDifference *
            newPiece.transform.rotation;

        Vector3 positionDifference =
            targetExit.position -
            newPiece.entrance.position;

        newPiece.transform.position +=
            positionDifference;

        newPiece.SpawnProps(rng);
    }

    bool TryPlacePiece(Transform targetExit)
    {
        List<TunnelPiece> candidates =
            new List<TunnelPiece>();

        candidates.AddRange(normalPieces);

        // Even seeds go upward
        if (seed % 2 == 0)
        {
            candidates.AddRange(rampUpPieces);
        }
        else
        {
            candidates.AddRange(rampDownPieces);
        }

        // Try several attempts
        for (int i = 0; i < 10; i++)
        {
            TunnelPiece selectedPrefab =
                candidates[rng.Next(candidates.Count)];

            TunnelPiece newPiece =
                Instantiate(selectedPrefab);

            // Rotate piece
            Quaternion rotationDifference =
                targetExit.rotation *
                Quaternion.Inverse(
                    newPiece.entrance.rotation
                );

            newPiece.transform.rotation =
                rotationDifference *
                newPiece.transform.rotation;

            // Move piece into place
            Vector3 positionDifference =
                targetExit.position -
                newPiece.entrance.position;

            newPiece.transform.position +=
                positionDifference;

            // Grid overlap check
            Vector3Int gridPos =
                WorldToGrid(
                    newPiece.transform.position
                );

            if (occupiedCells.Contains(gridPos))
            {
                Destroy(newPiece.gameObject);
                continue;
            }

            // Mark occupied
            occupiedCells.Add(gridPos);

            float distance =
            Vector3.Distance(
            Vector3.zero,
            newPiece.transform.position
            );

            if (distance > furthestDistance)
            {
                furthestDistance = distance;

                // Pick one exit from this piece
                if (newPiece.exits.Length > 0)
                {
                    furthestExit = newPiece.exits[0];
                }
            }


            // Spawn props
            newPiece.SpawnProps(rng);

            // Add exits
            foreach (Transform exit in newPiece.exits)
            {
                openExits.Add(exit);
            }

            return true;
        }

        return false;
    }

    void SpawnEndPiece(Transform targetExit)
    {
        TunnelPiece newPiece =
            Instantiate(endPiece);

        Quaternion rotationDifference =
            targetExit.rotation *
            Quaternion.Inverse(
                newPiece.entrance.rotation
            );

        newPiece.transform.rotation =
            rotationDifference *
            newPiece.transform.rotation;

        Vector3 positionDifference =
            targetExit.position -
            newPiece.entrance.position;

        newPiece.transform.position +=
            positionDifference;

        newPiece.SpawnProps(rng);
    }

    Vector3Int WorldToGrid(Vector3 position)
    {
        return new Vector3Int(
            Mathf.RoundToInt(position.x / gridSize),
            Mathf.RoundToInt(position.y / gridSize),
            Mathf.RoundToInt(position.z / gridSize)
        );
    }
}
