using System.Collections.Generic;
using UnityEngine;

public class TunnelGenerator : MonoBehaviour
{
    [Header("Generation")]
    public int seed = 12345;

    public int maxPieces = 40;

    [Header("Tunnel Pieces")]
    public TunnelPiece[] normalPieces;

    public TunnelPiece[] rampUpPieces;

    public TunnelPiece[] rampDownPieces;

    [Header("Collision")]
    public LayerMask tunnelLayer;

    private System.Random rng;

    private List<Transform> openExits =
        new List<Transform>();

    void Start()
    {
        Generate();
    }

    void Generate()
    {
        rng = new System.Random(seed);

        TunnelPiece firstPiece =
            Instantiate(
                normalPieces[0],
                Vector3.zero,
                Quaternion.identity
            );

        firstPiece.SpawnProps(rng);

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
        }
    }

    bool TryPlacePiece(Transform targetExit)
    {
        List<TunnelPiece> candidates =
            new List<TunnelPiece>();

        candidates.AddRange(normalPieces);

        // Try ramps if seed says so
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

            // Rotate
            Quaternion rotationDifference =
                targetExit.rotation *
                Quaternion.Inverse(
                    newPiece.entrance.rotation
                );

            newPiece.transform.rotation =
                rotationDifference *
                newPiece.transform.rotation;

            // Move
            Vector3 positionDifference =
                targetExit.position -
                newPiece.entrance.position;

            newPiece.transform.position +=
                positionDifference;

            // Check overlap
            if (IsOverlapping(newPiece))
            {
                Destroy(newPiece.gameObject);
                continue;
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



    bool IsOverlapping(TunnelPiece piece)
    {
        if (piece.collisionBounds == null)
            return false;

        BoxCollider box = piece.collisionBounds;

        Vector3 center = box.bounds.center;

        Vector3 halfSize = box.bounds.extents * 0.9f;

        Collider[] hits =
            Physics.OverlapBox(
                center,
                halfSize,
                piece.transform.rotation,
                tunnelLayer
            );

        foreach (Collider hit in hits)
        {
            // Ignore self collisions
            if (hit.transform.root == piece.transform.root)
                continue;
            Debug.Log("Overlap detected with: " + hit.name);

            return true;
        }

        return false;
    }

}
