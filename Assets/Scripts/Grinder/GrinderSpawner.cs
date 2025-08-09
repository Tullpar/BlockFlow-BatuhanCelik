using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrinderSpawner : MonoBehaviour
{
    #region Singleton
    public static GrinderSpawner Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion


    public List<GameObject> GrinderPrefabs = new List<GameObject>();

    public void SpawnGrinder(GrinderData grinderData)
    {
        Vector3 position = GridSystem.instance.GetEdgePosition(grinderData.GridPosition, grinderData.Direction, 0.3f, out Quaternion rotation);
        GameObject prefab = GrinderPrefabs[grinderData.Size - 1];

        GameObject spawnedGO = Instantiate(prefab, position, rotation,transform);

        Grinder spawnedGrinder = spawnedGO.GetComponent<Grinder>();

        Grinder.Direction direction = grinderData.Direction.x != 0 ? Grinder.Direction.Vertical : Grinder.Direction.Horizontal;


        spawnedGrinder.Color = (Block.BlockColor)grinderData.GrinderColor;
        spawnedGrinder.GrinderDirection = direction;
        spawnedGrinder.GridPosition = grinderData.GridPosition;


    }
}
