using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    #region Singleton
    public static BlockSpawner Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion


    public List<GameObject> BlockPrefabs = new List<GameObject>();

    public List<Block> SpawnedBlocks = new List<Block>();

    private void OnEnable()
    {
        Grinder.BlockDestroyedEvent += Grinder_BlockDestroyedEvent;
    }
    private void OnDisable()
    {
        Grinder.BlockDestroyedEvent -= Grinder_BlockDestroyedEvent;
    }

    private void Grinder_BlockDestroyedEvent()
    {
        SpawnedBlocks.RemoveAll(x => x == null);
        SpawnedBlocks.RemoveAll(x => x.IsDestroyed);
        if(SpawnedBlocks.Count == 0)
        {
            GameManager.Instance.Win();
        }
    }




    public void SpawnBlock(BlockData blockData)
    {
        Vector3 position = GridSystem.instance.GetGridCell(blockData.GridPosition).transform.position;
        GameObject prefab = BlockPrefabs[blockData.BlockIndex];

        GameObject spawnedBlockGO = Instantiate(prefab, position, prefab.transform.rotation, transform);
        Block spawnedBlock = spawnedBlockGO.GetComponent<Block>();
        spawnedBlock.blockColor = (Block.BlockColor)blockData.BlockColor;
        spawnedBlock.BlockMovementType = (Block.MovementType)blockData.MovementType;
        spawnedBlock.IceCounter = blockData.IceCounter;
        SpawnedBlocks.Add(spawnedBlock);

    }


    public void SpawnBlock(Vector2Int gridPosition,int blockIndex)
    {
        Vector3 position = GridSystem.instance.GetGridCell(gridPosition).transform.position;
        GameObject prefab = BlockPrefabs[blockIndex];


        GameObject spawnedBlockGO = Instantiate(prefab, position, Quaternion.identity, transform);
        Block spawnedBlock = spawnedBlockGO.GetComponent<Block>();
        spawnedBlock.OnBlockReleased();
    }
}
