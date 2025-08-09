using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPart : MonoBehaviour
{
    public Block Block;
    public GridCell currentCell;


    public Transform TopRightPoint;
    public Transform TopLeftPoint;
    public Transform MiddleRightPoint;
    public Transform MiddleLeftPoint;
    public Transform MiddleTopPoint;
    public Transform MiddleBottomPoint;
    public Transform BottomRightPoint;
    public Transform BottomLeftPoint;

    
    void OnEnable()
    {
        Block = GetComponentInParent<Block>();
        Block.BlockReleasedEvent += OnBlockReleased;
        Block.BlockPickedEvent += OnBlockPicked;
    }

    void OnDisable()
    {
        Block.BlockPickedEvent -= OnBlockPicked;
        Block.BlockReleasedEvent -= OnBlockReleased;
    }

    void OnBlockPicked()
    {
        if(currentCell != null)
        {
            currentCell.ReleaseBlock();
            currentCell = null;
        }
    }

    void OnBlockReleased()
    {
        GridCell cell = GridSystem.instance.GetClosestGridCell(transform.position);
        if (cell != null)
        {
            currentCell = cell;
            cell.SetBlock(Block);
        }
    }

    public float CalculateHorizontalMovement(float xMoveDelta)
    {
        List<Vector3> RayPoints = new List<Vector3>();
        if (xMoveDelta < 0)
        {
            RayPoints.Add(TopLeftPoint.position);
            RayPoints.Add(MiddleLeftPoint.position);
            RayPoints.Add(BottomLeftPoint.position);
        }
        else if (xMoveDelta > 0)
        {
            RayPoints.Add(TopRightPoint.position);
            RayPoints.Add(MiddleRightPoint.position);
            RayPoints.Add(BottomRightPoint.position);
        }

        float closestRayDistance = 99f;

        for(int i = 0; i < RayPoints.Count; i++)
        {
            Ray ray = new Ray(RayPoints[i], Vector3.right * Mathf.Sign(xMoveDelta));
            if (Physics.Raycast(ray, out RaycastHit hit, 10f))
            {
                if (hit.collider.GetComponent<Block>() != Block)
                {
                    if (hit.distance < closestRayDistance)
                    {
                        closestRayDistance = hit.distance;
                    }
                }
            }
        }
        if(closestRayDistance < 0.1f)
        {
            return 0;
        }
        else if(closestRayDistance == 99f)
        {
            return xMoveDelta;
        }
        else
        {
            float correctedRayDistance = closestRayDistance - 0.1f;
            if (correctedRayDistance < Mathf.Abs(xMoveDelta))
            {
                return correctedRayDistance * Mathf.Sign(xMoveDelta);
            }
            else
            {
                return xMoveDelta;
            }
        }
    }

    public float CalculateVerticalMovement(float zMoveDelta)
    {
        List<Vector3> RayPoints = new List<Vector3>();

        if (zMoveDelta < 0)
        {
            RayPoints.Add(BottomRightPoint.position);
            RayPoints.Add(BottomLeftPoint.position);
            RayPoints.Add(MiddleBottomPoint.position);
        }
        else if (zMoveDelta > 0)
        {
            RayPoints.Add(TopRightPoint.position);
            RayPoints.Add(TopLeftPoint.position);
            RayPoints.Add(MiddleTopPoint.position);
        }

        float closestRayDistance = 99f;

        for(int i = 0; i < RayPoints.Count; i++)
        {
            Ray ray = new Ray(RayPoints[i],Vector3.forward * Mathf.Sign(zMoveDelta));
            if (Physics.Raycast(ray, out RaycastHit hit, 10f))
            {
                if (hit.collider.GetComponent<Block>() != Block)
                {
                    if (hit.distance < closestRayDistance)
                    {
                        closestRayDistance = hit.distance;
                    }
                }
            }
        }

        if (closestRayDistance < 0.1f)
        {
            return 0;
        }
        else if (closestRayDistance == 99f)
        {
            return zMoveDelta;
        }
        else
        {
            float correctedRayDistance = closestRayDistance - 0.1f;
            if (correctedRayDistance < Mathf.Abs(zMoveDelta))
            {
                return correctedRayDistance * Mathf.Sign(zMoveDelta);
            }
            else
            {
                return zMoveDelta;
            }
        }
    }

    public bool DetectColission(Vector2 movementVector)
    {
        List<Vector3> RayPoints = new List<Vector3>();
        float xMoveDelta = movementVector.x;
        float zMoveDelta = movementVector.y;
        if (xMoveDelta < 0)
        {
            RayPoints.Add(TopLeftPoint.position);
            RayPoints.Add(MiddleLeftPoint.position);
            RayPoints.Add(BottomLeftPoint.position);
        }
        else if (xMoveDelta > 0)
        {
            RayPoints.Add(TopRightPoint.position);
            RayPoints.Add(MiddleRightPoint.position);
            RayPoints.Add(BottomRightPoint.position);
        }

        if (zMoveDelta < 0)
        {
            RayPoints.Add(BottomRightPoint.position);
            RayPoints.Add(BottomLeftPoint.position);
            RayPoints.Add(MiddleBottomPoint.position);
        }
        else if (zMoveDelta > 0)
        {
            RayPoints.Add(TopRightPoint.position);
            RayPoints.Add(TopLeftPoint.position);
            RayPoints.Add(MiddleTopPoint.position);
        }

        for (int i = 0; i < RayPoints.Count; i++)
        {
            Ray ray = new Ray(RayPoints[i], new Vector3(movementVector.x,0,movementVector.y));
            if (Physics.Raycast(ray, out RaycastHit hit, movementVector.magnitude))
            {
                if (hit.collider.GetComponent<Block>() != Block)
                {
                    return true;
                }
            }
        }
        return false;
    }
    public void CheckIsTouchingGrinder()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position, Vector3.one * 1f, transform.rotation,LayerMask.GetMask("Grinder"));
        if (colliders.Length > 0)
        {
            foreach (Collider collider in colliders)
            {
                if (collider.TryGetComponent(out Grinder grinder))
                {
                    Block.CheckGrinder(grinder);
                }
            }
        }

    }
}
