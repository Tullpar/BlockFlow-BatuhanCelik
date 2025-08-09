using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPartRay : MonoBehaviour
{
    public enum RayDirection { Up, Down, Right, Left };
    public RayDirection Direction;
    public void CastRay()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        if (Direction == RayDirection.Up)
        {

        }

    }
}
