using System.Collections.Generic;
using UnityEngine;

public class TunnelGenerator : MonoBehaviour
{
    [Header("Generation")]
    public int seed = 12345;

    public int maxPieces = 60;

    [Header("Pieces")]
    public TunnelPiece startPiece;

    public TunnelPiece[] normalPieces;

    public TunnelPiece deadEndPiece;

    public TunnelPiece bossRoom;

    System.Random rng;

    List<Transform> openExits =
        new List<Transform>();

    Transform furthestExit;

    float furthestDistance;

    void Start()
    {
        Generate();
    }

    void Generate()
    {
        rng =
            new System.Random(
                seed
            );

        TunnelPiece start =
            Instantiate(
                startPiece,
                Vector3.zero,
                Quaternion.identity
            );

        foreach (
            Transform exit
            in start.exits
        )
        {
            openExits.Add(
                exit
            );
        }

        int placedPieces = 0;

        while (
            openExits.Count > 0 &&
            placedPieces < maxPieces
        )
        {
            Transform exit =
                GetBestExit();

            openExits.Remove(
                exit
            );

            TunnelPiece prefab =
                normalPieces[
                    rng.Next(
                        normalPieces.Length
                    )
                ];

            TunnelPiece piece =
                Instantiate(
                    prefab
                );

            AlignPiece(
                piece,
                exit
            );

            foreach (
                Transform newExit
                in piece.exits
            )
            {
                if (
                    newExit != null
                )
                {
                    openExits.Add(
                        newExit
                    );
                }
            }

            float dist =
                Vector3.Distance(
                    Vector3.zero,
                    piece.transform.position
                );

            if (
                dist >
                furthestDistance
            )
            {
                furthestDistance =
                    dist;

                if (
                    piece.exits.Length > 0
                )
                {
                    furthestExit =
                        piece.exits[0];
                }
            }

            placedPieces++;
        }

        foreach (
            Transform exit
            in openExits
        )
        {
            if (
                deadEndPiece != null
            )
            {
                TunnelPiece deadEnd =
                    Instantiate(
                        deadEndPiece
                    );

                AlignPiece(
                    deadEnd,
                    exit
                );
            }
        }

        if (
            bossRoom != null &&
            furthestExit != null
        )
        {
            TunnelPiece boss =
                Instantiate(
                    bossRoom
                );

            AlignPiece(
                boss,
                furthestExit
            );
        }
    }

    Transform GetBestExit()
    {
        Transform best = null;

        float bestScore =
            float.MinValue;

        foreach (
            Transform exit
            in openExits
        )
        {
            Vector3 dir =
                (
                    exit.position -
                    Vector3.zero
                ).normalized;

            float outwardness =
                Vector3.Dot(
                    exit.forward,
                    dir
                );

            float distance =
                exit.position
                    .magnitude;

            float score =
                outwardness * 10f +
                distance;

            if (
                score >
                bestScore
            )
            {
                bestScore =
                    score;

                best = exit;
            }
        }

        return best;
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