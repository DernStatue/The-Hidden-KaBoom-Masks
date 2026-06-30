using UnityEngine;

public class TunnelPiece : MonoBehaviour
{
    [Header("Tunnel Connections")]
    public Transform entrance;
    public Transform[] exits;

    [Header("Props")]
    public Transform[] propPoints;

    [Range(0f, 1f)]
    public float propSpawnChance = 0.4f;

    public GameObject[] possibleProps;

    [Header("Collision Check")]
    public BoxCollider collisionBounds;

    public void SpawnProps(System.Random rng)
    {
        if (possibleProps.Length == 0)
            return;

        foreach (Transform point in propPoints)
        {
            double randomValue = rng.NextDouble();

            if (randomValue <= propSpawnChance)
            {
                int propIndex =
                    rng.Next(possibleProps.Length);

                GameObject selectedProp =
                    possibleProps[propIndex];

                Instantiate(
                    selectedProp,
                    point.position,
                    point.rotation,
                    transform
                );
            }
        }
    }
}
