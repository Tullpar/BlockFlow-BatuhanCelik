using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grinder : MonoBehaviour
{
    public static event Action BlockDestroyedEvent;


    public Block.BlockColor Color;

    public int Size = 1;

    public Vector2Int GridPosition;
    
    public Direction GrinderDirection;
    public enum Direction { Horizontal, Vertical };

    public Transform[] RayPositions;

    private void Start()
    {
        GetComponent<BlockColorSelector>().SetColor(Color);
    }

    public void IsBlockMatching(Block block)
    {
        if(block.IsDestroyed)
        {
            return;
        }
        if(!IsColorsMatching(block))
        {
            return;
        }
        BlockPart[] blockParts = block.GetBlockParts();

        bool isPartsMatching = true;

        foreach (BlockPart part in blockParts) 
        {
            if(!CheckBlockPart(part))
            {
                isPartsMatching = false;
            }
        }

        if (isPartsMatching)
        {
            block.DestroyBlock(this);
            BlockDestroyedEvent?.Invoke();
            //Destroy(block.gameObject);
        }

    }

    bool IsColorsMatching(Block block)
    {
        return Color == block.blockColor;
    }

    bool CheckBlockPart(BlockPart part)
    {
        bool isPartMatching = false;
        Vector2Int Direction = new Vector2Int((int)transform.forward.x, (int)transform.forward.z);
        if (Direction == Vector2Int.right)
        {
            bool isOnSameRow = part.currentCell.GridPosition.y <= GridPosition.y &&
                part.currentCell.GridPosition.y > GridPosition.y - Size;
            isPartMatching = isOnSameRow;
        }
        else if (Direction == Vector2Int.left)
        {
            bool isOnSameRow = part.currentCell.GridPosition.y >= GridPosition.y &&
                part.currentCell.GridPosition.y < GridPosition.y + Size;
            isPartMatching = isOnSameRow;
        }
        else if(Direction == Vector2Int.up)
        {
            bool isOnSameColumn = part.currentCell.GridPosition.x >= GridPosition.x &&
                part.currentCell.GridPosition.x < GridPosition.x + Size;
            isPartMatching = isOnSameColumn;
        }
        else if(Direction == Vector2Int.down)
        {
            bool isOnSameColumn = part.currentCell.GridPosition.x <= GridPosition.x &&
                part.currentCell.GridPosition.x > GridPosition.x - Size;
            isPartMatching = isOnSameColumn;

        }

        //if (GrinderDirection == Direction.Vertical)
        //{
        //    bool isOnSameRow = part.currentCell.GridPosition.y >= GridPosition.y &&
        //        part.currentCell.GridPosition.y < GridPosition.y + Size;
        //    isPartMatching = isOnSameRow;
        //}
        //else
        //{
        //    bool isOnSameColumn = part.currentCell.GridPosition.x >= GridPosition.x &&
        //        part.currentCell.GridPosition.x < GridPosition.x + Size;
        //    isPartMatching = isOnSameColumn;
        //}

        if(isPartMatching)
        {
            return !CheckIsPartObstructed(part);
        }
        else
        {
            return false;
        }
    }

    bool CheckIsPartObstructed(BlockPart part)
    {
        if (GrinderDirection == Direction.Vertical)
        {
            int rayIndex = part.currentCell.GridPosition.y - GridPosition.y;
            if(rayIndex > 0 && rayIndex < RayPositions.Length)
            {
                Ray ray = new Ray(RayPositions[rayIndex].position, RayPositions[rayIndex].forward);
                if (Physics.Raycast(ray, out RaycastHit hit,10f,LayerMask.GetMask("Block")))
                {
                    if (hit.collider.TryGetComponent(out Block block))
                    {
                        if (block != part.Block)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        else
        {
            int rayIndex = part.currentCell.GridPosition.x - GridPosition.x;
            if(rayIndex > 0 && rayIndex < RayPositions.Length)
            {
                Ray ray = new Ray(RayPositions[rayIndex].position, RayPositions[rayIndex].forward);
                if(Physics.Raycast(ray,out RaycastHit hit,10f, LayerMask.GetMask("Block")))
                {
                    if(hit.collider.TryGetComponent(out Block block))
                    {
                        if (block != part.Block)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}
