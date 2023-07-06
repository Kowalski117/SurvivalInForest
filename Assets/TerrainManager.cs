using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    private GameObject Tree;
    Terrain CurrentTerrain;
    
    public void Correct()
    {
        CurrentTerrain = GetComponent<Terrain>();

        for (int i = 0; i < CurrentTerrain.terrainData.treeInstances.Length; i++)
        {
            Tree = CurrentTerrain.terrainData.treePrototypes[Random.Range(0,CurrentTerrain.terrainData.treePrototypes.Length)].prefab;

            Vector3 WorldTreePos = Vector3.Scale(CurrentTerrain.terrainData.treeInstances[i].position, CurrentTerrain.terrainData.size) + CurrentTerrain.transform.position;
            GameObject a = Instantiate(Tree, WorldTreePos, Quaternion.identity);
            a.name = a.name.Substring(0, a.name.LastIndexOf("(") - 1);
            a.transform.SetParent(transform);
        }

        CurrentTerrain.terrainData.treeInstances = new TreeInstance[0];
        Debug.Log("Trees had been spawned.");
    }

    public void Delete()
    {
        CurrentTerrain = GetComponent<Terrain>();
        for (int i = 0; i < CurrentTerrain.transform.childCount; i++) DestroyImmediate(CurrentTerrain.transform.GetChild(0).gameObject);
        Debug.Log("Trees had been deleted.");
    }
}
