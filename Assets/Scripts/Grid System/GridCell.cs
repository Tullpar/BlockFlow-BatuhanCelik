using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    public Block CurrentBlock;
    public bool IsOccupied { get { return HasBlock(); } }

    public Vector2Int GridPosition;

    public void SetGridPosition(Vector2Int position)
    {
        GridPosition = position;
    }

    public void SetBlock(Block block)
    {
        CurrentBlock = block;
    }

    public void ReleaseBlock()
    {
        CurrentBlock = null;
    }

    bool HasBlock()
    {
        return CurrentBlock != null;
    }
}
