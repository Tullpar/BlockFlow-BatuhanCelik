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
            return Mathf.Min(closestRayDistance, Mathf.Abs(xMoveDelta) * Mathf.Sign(xMoveDelta));
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
            return Mathf.Min(closestRayDistance,Mathf.Abs(zMoveDelta)) * Mathf.Sign(zMoveDelta);
        }
    }


    public bool CheckMovement(Vector3 movementDelta,out Vector3 newDelta)
    {
        //for (int i = 0; i < RayPositions.Count; i++)
        //{
        //    Ray ray = new Ray(RayPositions[i].position, RayPositions[i].forward);
        //    if (Physics.Raycast(ray, out RaycastHit hit, 0.2f, LayerMask.NameToLayer("Block")))
        //    {
        //        if(hit.collider.TryGetComponent(out Block otherBlock))
        //        {
        //            newDelta = movementDelta;
        //            //newDelta = hit.distance;
        //            return false;
        //        }
        //    }
        //}

        bool sideWays = CheckSideways(movementDelta,out float x);
        bool forwards = CheckForward(movementDelta,out float z);

        if (!sideWays && !forwards)
        {
            newDelta = Vector3.zero;
            return false;
        }
        else
        {
            newDelta = new Vector3(x, 0, z);
            return true;
        }


            Collider[] colliders = Physics.OverlapBox(transform.position + movementDelta, Vector3.one * 0.5f, transform.rotation);
        if (colliders.Length > 0)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].TryGetComponent(out Block otherBlock))
                {
                    Debug.Log(otherBlock.name);
                    if(otherBlock != Block)
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }

    bool CheckSideways(Vector3 movementDelta,out float x)
    {
        x = movementDelta.x;
        Collider[] colliders = Physics.OverlapBox(transform.position + Vector3.right * movementDelta.x, Vector3.one * 0.5f, transform.rotation);
        if (colliders.Length > 0)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].TryGetComponent(out Block otherBlock))
                {
                    if (otherBlock != Block)
                    {
                        x = colliders[i].transform.position.x - 0.51f * Mathf.Sign(-movementDelta.x) - Block.transform.position.x;
                        Debug.DrawRay(transform.position + Vector3.right * movementDelta.x, Vector3.up, Color.red);
                        return false;
                    }
                }
            }
        }
        return true;
    }

    bool CheckForward(Vector3 movementDelta,out float z)
    {
        z = movementDelta.z;
        Collider[] colliders = Physics.OverlapBox(transform.position + Vector3.forward * movementDelta.z, Vector3.one * 0.5f, transform.rotation);
        if (colliders.Length > 0)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].TryGetComponent(out Block otherBlock))
                {
                    if (otherBlock != Block)
                    {
                        z = colliders[i].transform.position.z - 0.51f * Mathf.Sign(-movementDelta.z) - Block.transform.position.z;
                        Debug.DrawRay(transform.position + Vector3.forward * movementDelta.z, Vector3.up, Color.red);
                        return false;
                    }
                }
            }
        }
        return true;
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
