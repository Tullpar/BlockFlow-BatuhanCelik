using UnityEngine;

public class BlockMovement : MonoBehaviour
{
    public Block CurrentBlock;
    Vector3 offset;
    public Plane MovementPlane;


    private void Start()
    {
        MovementPlane = new Plane(Vector3.up, transform.position);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Block"))
                {
                    CurrentBlock = hit.collider.GetComponent<Block>();

                }
                if (CurrentBlock != null)
                {
                    Ray planeRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (MovementPlane.Raycast(ray, out float point))
                    {
                        Vector3 position = ray.GetPoint(point);
                        offset = CurrentBlock.transform.position - position;
                        offset.y = 0;
                    }
                    CurrentBlock.OnBlockPicked();
                }
            }
        }

        if (Input.GetMouseButton(0))
        {
            if (CurrentBlock != null)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (MovementPlane.Raycast(ray, out float point))
                {
                    Vector3 position = ray.GetPoint(point);
                    position += offset;

                    Vector3 movementDelta = position - CurrentBlock.transform.position;

                    movementDelta = ClampMovementVector(movementDelta);

                    movementDelta = CurrentBlock.CalculateMovementVector(movementDelta);

                    CurrentBlock.transform.position += movementDelta;
                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (CurrentBlock != null)
            {
                GridCell closestCell = GridSystem.instance.GetClosestGridCell(CurrentBlock.transform.position);
                CurrentBlock.transform.position = closestCell.transform.position;

                CurrentBlock.OnBlockReleased();


                CurrentBlock = null;
            }
        }
    }

    Vector3 ClampMovementVector(Vector3 movementVector)
    {
        if(movementVector.magnitude > 0.4f)
        {
            movementVector = movementVector.normalized * 0.4f; 
        }
        return movementVector;
    }


}
