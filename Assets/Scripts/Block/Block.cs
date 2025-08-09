using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    IceBlockDisplay IceBlockDisplay;
    BlockColorSelector BlockColorSelector;

    public bool IsDestroyed = false;

    public int Index;

    public event Action BlockReleasedEvent;
    public event Action BlockPickedEvent;

    public Vector2Int BlockSize = new Vector2Int(1,1);

    public int IceCounter = 0;

    public bool IsIceBlock { get { return IceCounter > 0; } }

    public enum MovementType { All, Vertical, Horizontal,Locked }

    public MovementType BlockMovementType;

    public enum BlockColor {Red,Yellow,Purple,Blue,Green,Orange,Cyan,Pink}

    public BlockColor blockColor;

    BlockPart[] BlockParts;


    private void OnEnable()
    {
        Grinder.BlockDestroyedEvent += OnBlockDestroyed;
    }
    private void OnDisable()
    {
        Grinder.BlockDestroyedEvent -= OnBlockDestroyed;
    }

    private void Start()
    {

        SetBlockParts();

        BlockColorSelector = GetComponent<BlockColorSelector>();
        IceBlockDisplay = GetComponentInChildren<IceBlockDisplay>();

        BlockColorSelector.SetColor(blockColor);
        BlockColorSelector.SetArrows(BlockMovementType);
        if (IsIceBlock)
        {
            IceBlockDisplay.SetCounter(IceCounter);
            BlockColorSelector.SetIce();
        }

        OnBlockReleased();
    }



    public void OnBlockPicked()
    {
        BlockPickedEvent?.Invoke();
    }

    public void OnBlockReleased()
    {
        BlockReleasedEvent?.Invoke();

        if(BlockParts == null)
        {
            SetBlockParts();
        }

        foreach(BlockPart part in BlockParts)
        {
            part.CheckIsTouchingGrinder();
        }
    }

    void SetBlockParts()
    {
        BlockParts = GetComponentsInChildren<BlockPart>();
        foreach (BlockPart part in BlockParts)
        {
            part.Block = this;
        }
    }


    public Vector3 CalculateMovementVector(Vector3 movementVector)
    {
        float xMovement = movementVector.x;
        float zMovement = movementVector.z;
        foreach (BlockPart part in BlockParts) 
        {
            xMovement = Mathf.Min(Mathf.Abs(xMovement), Mathf.Abs(part.CalculateHorizontalMovement(xMovement))) * Mathf.Sign(xMovement);
            zMovement = Mathf.Min(Mathf.Abs(zMovement),Mathf.Abs(part.CalculateVerticalMovement(zMovement))) * Mathf.Sign(zMovement);
        }

        switch (BlockMovementType)
        {
            case MovementType.Vertical:
                xMovement = 0;
                break;
            case MovementType.Horizontal:
                zMovement = 0;
                break;
            case MovementType.Locked:
                xMovement = 0;
                zMovement = 0;
                break;
        }


        return new Vector3 (xMovement,0, zMovement);
    }


    public bool CheckMovement(Vector3 movementDelta,out Vector3 newDelta)
    {
        newDelta = movementDelta;
        foreach (BlockPart part in BlockParts)
        {
            if (!part.CheckMovement(movementDelta,out Vector3 partDelta))
            {
                newDelta = SetNewDelta(movementDelta,partDelta);
                return false;
            }
        }
        return true;
    }

    Vector3 SetNewDelta(Vector3 delta,Vector3 partDelta)
    {
        if(Mathf.Abs(partDelta.x) < Mathf.Abs(delta.x))
        {
            delta.x = partDelta.x;
        }

        if(Mathf.Abs(partDelta.z) < Mathf.Abs(delta.z))
        {
            delta.z = partDelta.z;
        }
        return delta;
    }


    public void CheckGrinder(Grinder grinder)
    {
        grinder.IsBlockMatching(this);
    }

    public BlockPart[] GetBlockParts()
    {
        return BlockParts;
    }

    public BlockPart GetBlockPart(int index)
    {
        return BlockParts[index];
    }


    void OnBlockDestroyed()
    {
        if(IsIceBlock)
        {
            IceCounter--;
            IceBlockDisplay.SetCounter(IceCounter);
            if(!IsIceBlock)
            {
                BlockColorSelector colorSelector = GetComponent<BlockColorSelector>();
                colorSelector.SetColor(blockColor);
                BlockMovementType = MovementType.All;
                IceBlockDisplay.DisableCounter();
            }
        }
    }

    public void DestroyBlock(Grinder grinder)
    {
        if(IsDestroyed)
        {
            return;
        }
        BlockColorSelector.SetArrows(MovementType.All);

        IsDestroyed = true;
        StartCoroutine(DestroyCoroutine(grinder));
    }

    IEnumerator DestroyCoroutine(Grinder grinder)
    {
        float t = 0;
        float duration = 0.5f;
        Vector3 startPosition = transform.position;

        

        Vector3 endPosition = transform.position - grinder.transform.forward * 3f;


        while(t <= 1)
        {
            t+= Time.deltaTime/duration;
            yield return new WaitForEndOfFrame();
            transform.position = Vector3.Lerp(startPosition, endPosition, t);
        }

        Destroy(gameObject);

    }
}
