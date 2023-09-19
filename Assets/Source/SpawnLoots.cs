using UnityEngine;

public abstract class SpawnLoots : MonoBehaviour
{
    public static void Spawn(ItemPickUp itemPickUp, Vector3 position, Transform transform, bool isKinematic, float spawnPointUp, bool isSetParant)
    {
        ItemPickUp current;

        if (isSetParant)
        {
            current = Instantiate(itemPickUp, position, transform.rotation, transform);
        }
        else
        {
            current = Instantiate(itemPickUp, position, transform.rotation);
        }

        current.transform.position = new Vector3(current.transform.position.x, transform.position.y + spawnPointUp, current.transform.position.z);
        current.GetComponent<Rigidbody>().isKinematic = isKinematic;
        current.GenerateNewID();
    }
}