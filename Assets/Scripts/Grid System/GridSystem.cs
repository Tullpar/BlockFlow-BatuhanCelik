using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem : MonoBehaviour
{
    public static GridSystem instance;


    public int RowCount = 5;
    public int ColumnCount = 5;

    public float GridCellSize = 1.1f;

    public GameObject GridPrefab;
    public GameObject WallPrefab;
    public GameObject CornerPrefab;


    public GridCell[][] GridCells;


    private void Awake()
    {
        instance = this;
    }


    public GridCell GetGridCell(Vector2Int gridPosition)
    {
        return GridCells[gridPosition.x][gridPosition.y];
    }

    public GridCell GetClosestGridCell(Vector3 position)
    {
        int closestXIndex = 0;
        int closestZIndex = 0;

        float closestXDistance = 99999;
        for(int i = 0; i < GridCells.Length;i++)
        {
            float xDistance = Mathf.Abs(GridCells[i][0].transform.position.x - position.x);
            if(xDistance < closestXDistance)
            {
                closestXIndex = i;
                closestXDistance = xDistance;
            }
        }

        float closestZDistance = 99999;
        for(int i = 0; i < GridCells[0].Length;i++)
        {
            float zDistance = Mathf.Abs(GridCells[0][i].transform.position.z - position.z);
            if(zDistance < closestZDistance)
            {
                closestZIndex = i;
                closestZDistance = zDistance;
            }
        }
        closestXIndex = Mathf.Clamp(closestXIndex,0,GridCells.Length-1);
        closestZIndex = Mathf.Clamp(closestZIndex, 0, GridCells[0].Length-1);

        return GridCells[closestXIndex][closestZIndex];
    }

    

    public void CreateGrid(int row,int column)
    {
        RowCount = row;
        ColumnCount = column;
        GridCells = new GridCell[row][];
        float xOffset = (row - 1) * GridCellSize/ 2f;
        float zOffset = (column - 1) * GridCellSize / 2f;
        float yOffset = 0;
        for (int i = 0; i < row; i++)
        {
            GridCells[i] = new GridCell[column];
            for(int j = 0;j < column; j++)
            {
                Vector3 cellPosition = transform.position + new Vector3(i * GridCellSize - xOffset,yOffset,j * GridCellSize - zOffset);
                GameObject cellGO = Instantiate(GridPrefab, cellPosition,GridPrefab.transform.rotation,transform);
                cellGO.name = "Cell " + i + "_" + j;
                GridCell cell = cellGO.GetComponent<GridCell>();
                GridCells[i][j] = cell;
                cell.SetGridPosition(new Vector2Int(i, j));
            }
        }
        CreateWalls(row, column);
    }


    public void CreateWalls(int rowCount,int columnCount)
    {
        float xOffset = (rowCount - 1) * GridCellSize / 2f;
        float zOffset = (columnCount - 1) * GridCellSize / 2f;
        float wallOffset = 0.3f;

        //Bottom Walls
        for (int i = 0; i < rowCount; i++)
        {
            Vector3 position = GetEdgePosition(new Vector2Int(i, 0), Vector2Int.down, wallOffset, out Quaternion rotation);
            Instantiate(WallPrefab, position, rotation, transform);
        }

        //Top Walls
        for (int i = 0; i < rowCount; i++)
        {
            Vector3 position = GetEdgePosition(new Vector2Int(i, columnCount),Vector2Int.up, wallOffset, out Quaternion rotation);
            Instantiate(WallPrefab, position, rotation, transform);
        }

        //Right Walls
        for (int i = 0; i <columnCount;i++)
        {
            Vector3 position = GetEdgePosition(new Vector2Int(rowCount, i), Vector2Int.right, wallOffset, out Quaternion rotation);
            Instantiate(WallPrefab, position, rotation, transform);


        }

        //Left Walls
        for (int i = 0; i < columnCount;i++)
        {
            Vector3 position = GetEdgePosition(new Vector2Int(0, i), Vector2Int.left, wallOffset, out Quaternion rotation);

            Instantiate(WallPrefab, position, rotation, transform);

        }
        CreateCorners(rowCount, columnCount);
    }

    void CreateCorners(int rowCount,int columnCount)
    {
        float xOffset = (rowCount - 1) * GridCellSize / 2f;
        float zOffset = (columnCount - 1) * GridCellSize / 2f;
        float cornerOffset = 0.3f;

        //Top Right Corner
        Vector3 topRightPosition = transform.position + new Vector3(rowCount * GridCellSize - xOffset - cornerOffset,0, columnCount * GridCellSize - zOffset - cornerOffset);
        Instantiate(CornerPrefab, topRightPosition,Quaternion.Euler(0,0,0),transform);

        //Top Left Corner
        Vector3 topLeftPosition = transform.position + new Vector3(-(rowCount * GridCellSize - xOffset - cornerOffset), 0, columnCount * GridCellSize - zOffset - cornerOffset);
        Instantiate(CornerPrefab,topLeftPosition, Quaternion.Euler(0,-90f,0),transform);

        //BottomRightCorner
        Vector3 bottomRightPosition = transform.position + new Vector3(rowCount * GridCellSize - xOffset - cornerOffset,0, -(columnCount * GridCellSize - zOffset) + cornerOffset);
        Instantiate(CornerPrefab,bottomRightPosition, Quaternion.Euler(0,90f,0),transform);

        //BottomLeftCorner
        Vector3 bottomLeftPosition = transform.position + new Vector3(-(rowCount * GridCellSize - xOffset - cornerOffset),0, -(columnCount * GridCellSize - zOffset) + cornerOffset);
        Instantiate(CornerPrefab,bottomLeftPosition, Quaternion.Euler(0,180f,0), transform);

    }

    public Vector3 GetEdgePosition(Vector2Int gridPosition, Vector2Int direction, float Offset,out Quaternion rotation)
    {
        float xOffset = (RowCount - 1) * GridCellSize / 2f;
        float zOffset = (ColumnCount - 1) * GridCellSize / 2f;

        Vector3 calculatedPosition = transform.position + new Vector3(gridPosition.x * GridCellSize - xOffset, 0f, gridPosition.y * GridCellSize - zOffset);
        rotation = Quaternion.identity;

        Vector3 position = calculatedPosition;

        if(direction == Vector2Int.left)
        {
            position = calculatedPosition + Vector3.right * Offset - Vector3.right * GridCellSize;
            rotation = Quaternion.Euler(0, 90, 0);
        }
        if(direction == Vector2Int.right)
        {
            position = calculatedPosition - Vector3.right * Offset;
            rotation = Quaternion.Euler(0, -90, 0);
        }
        if(direction == Vector2Int.down)
        {
            position = calculatedPosition + Vector3.forward * Offset - Vector3.forward * GridCellSize;
            rotation = Quaternion.Euler(0, 0, 0);
        }
        if (direction == Vector2Int.up)
        {
            position = calculatedPosition - Vector3.forward * Offset;
            rotation = Quaternion.Euler(0, 180, 0);
        }

        return position;

    }




}
