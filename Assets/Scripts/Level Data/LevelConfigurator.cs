using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelConfigurator : MonoBehaviour
{
    public static LevelConfigurator Instance;
    private void Awake()
    {
        Instance = this;
    }

    public Vector2Int LevelSize;
    public List<Block> Blocks = new List<Block>();
    public List<Grinder> Grinders = new List<Grinder>();

    public GameObject HolePrefab;

    public List<TextAsset> LevelDatas = new List<TextAsset>();

    public GameObject GridSystemPrefab;
    public GameObject BlockSpawnerPrefab;
    public GameObject GrinderSpawnerPrefab;

    public GridSystem CurrentGridSystem;
    public BlockSpawner CurrentBlockSpawner;
    public GrinderSpawner CurrentGrinderSpawner;

    GameObject CurrentHoleMesh;

    public int LevelIndex = 0;


    [ContextMenu("Create Empty Grid")]
    public void CreateEmptyGrid()
    {
        GridSystem.instance.CreateGrid(LevelSize.x, LevelSize.y);
    }


    [ContextMenu("Save")]
    public void Save()
    {
        LevelSpawner.LevelData.LevelSize = LevelSize;
        SaveBlocks();
        SaveGrinders();

        LevelSpawner.SaveLevelData(LevelIndex.ToString());
    }

    [ContextMenu("Load")]
    public void Load(int levelIndex)
    {
        LevelIndex = levelIndex;
        DestroyOldLevel();
        CreateSpawners();


        //LevelSpawner.LoadLevelData(LevelIndex.ToString());
        LevelSpawner.LoadLevelData(LevelDatas[LevelIndex]);
        LevelSize = LevelSpawner.LevelData.LevelSize;
        GridSystem.instance.CreateGrid(LevelSize.x, LevelSize.y);
        LoadBlocks();
        LoadGrinders();
        CreateHole();
    }

    void SaveBlocks()
    {
        List<BlockData> blockDatas = new List<BlockData>();
        foreach (var block in Blocks)
        {
            BlockData blockData = new BlockData();
            blockData.GridPosition = block.GetBlockPart(0).currentCell.GridPosition;
            blockData.MovementType = (int)block.BlockMovementType;
            blockData.BlockIndex = block.Index;
            blockData.BlockColor = (int)block.blockColor;
            blockData.IceCounter = block.IceCounter;
            blockDatas.Add(blockData);
        }
        LevelSpawner.LevelData.BlockDatas = blockDatas.ToArray();
    }

    void SaveGrinders()
    {
        List<GrinderData> grinderDatas = new List<GrinderData>();
        foreach (var grinder in Grinders)
        {
            GrinderData grinderData = new GrinderData();
            grinderData.GrinderColor = (int)grinder.Color;
            grinderData.GridPosition = grinder.GridPosition;
            grinderData.Direction = new Vector2Int(-(int)grinder.transform.forward.x, -(int)grinder.transform.forward.z);
            grinderData.Size = grinder.Size;
            grinderDatas.Add(grinderData);
        }
        LevelSpawner.LevelData.GrinderDatas = grinderDatas.ToArray();
    }

    void CreateSpawners()
    {
        CurrentGridSystem = Instantiate(GridSystemPrefab).GetComponent<GridSystem>();
        CurrentBlockSpawner = Instantiate(BlockSpawnerPrefab).GetComponent<BlockSpawner>();
        CurrentGrinderSpawner = Instantiate(GrinderSpawnerPrefab).GetComponent<GrinderSpawner>();
    }

    void DestroyOldLevel()
    {
        if (CurrentGridSystem != null)
        {
            Destroy(CurrentGridSystem.gameObject);
            Destroy(CurrentBlockSpawner.gameObject);
            Destroy(CurrentGrinderSpawner.gameObject);
            Destroy(CurrentHoleMesh);
        }
    }


    void LoadBlocks()
    {
        BlockData[] blockDatas = LevelSpawner.LevelData.BlockDatas;
        foreach (var blockData in blockDatas)
        {
            BlockSpawner.Instance.SpawnBlock(blockData);
        }
    }

    void LoadGrinders()
    {
        GrinderData[] grinderDatas = LevelSpawner.LevelData.GrinderDatas;
        foreach(var grinderData in grinderDatas)
        {
            GrinderSpawner.Instance.SpawnGrinder(grinderData);
        }
    }

    void CreateHole()
    {
        CurrentHoleMesh = Instantiate(HolePrefab, Vector3.zero + Vector3.up * 0.5f, Quaternion.identity, transform);
        CurrentHoleMesh.transform.localScale = new Vector3(LevelSize.x / 2f + 0.2f, 1f, LevelSize.y / 2f + 0.2f);
    }
}
