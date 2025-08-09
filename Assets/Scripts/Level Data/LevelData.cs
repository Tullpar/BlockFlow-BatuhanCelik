using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    public float LevelTimer = 100;
    public Vector2Int LevelSize;
    public BlockData[] BlockDatas;
    public GrinderData[] GrinderDatas;
}

[System.Serializable]
public class BlockData
{
    public int BlockIndex;
    public Vector2Int GridPosition;
    public int BlockColor;
    public int MovementType;
    public int IceCounter = 0;
    //public Block.BlockColor BlockColor;
    //public Block.MovementType MovementType;
}

[System.Serializable]
public class GrinderData
{
    public int Size;
    public Vector2Int GridPosition;
    public Vector2Int Direction;
    public int GrinderColor;

    //public Block.BlockColor GrinderColor;
}