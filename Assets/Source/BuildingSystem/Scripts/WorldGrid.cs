using UnityEngine;

public static class WorldGrid
{
    public static Vector3 GridPositionFromWorldPoint3D(Vector3 worldPos, float gridScale)
    {
        float x = Mathf.Round(worldPos.x / gridScale) * gridScale;
        float y = Mathf.Round(worldPos.y / gridScale) * gridScale;
        float z = Mathf.Round(worldPos.z / gridScale) * gridScale;

        return new Vector3(x, y, z);
    }
}
