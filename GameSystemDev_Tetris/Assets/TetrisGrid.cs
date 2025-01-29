using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisGrid : MonoBehaviour
{
    public int width = 10;
    public int height = 20;
    //Previously assumed everything was 3 tall. This somehow has to be reworked into working with 5 tall
    public int maxTetrisHeight = 5;
    private Transform[,] grid;
    public Transform[,] debugGrid;
    TetrisManager tetrisManager;

    // Start is called before the first frame update
    void Awake()
    {
        tetrisManager = FindObjectOfType<TetrisManager>();

        grid = new Transform[width, height + 6];
        debugGrid = new Transform[width, 20];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                // Create a new Gameobject at this position, you can add a cutom prefab instead
                GameObject cell = new GameObject($"Cell ({i},{j})");
                cell.transform.position = new Vector3(i, j, 0);
                debugGrid[i, j] = cell.transform;
            }
        }
    }

    //public void AddToGrid(Transform piece)
    //{
        //foreach (Transform block in piece)
        //{
            //Vector2Int position = Vector2Int.RoundToInt(block.position);
            //grid[position.x, position.y] = block;
        //}
    //}

    public void AddBlockToGrid(Transform block, Vector2Int position)
    {
        grid[position.x, position.y] = block;
    }

    public bool IsCellOccupied(Vector2Int position)
    {
        //int x = Mathf.RoundToInt(position.x);
        //int y = Mathf.RoundToInt(position.y);
        //return (x >= 0 && x < width && y >= 0 && y < height) && grid[x, y] != null;
        if (position.x < 0 || position.x >= width || position.y < 0 || position.y >= height + 3)
        {
            return true;
        }
        return grid[position.x, position.y] != null;
    }

    public bool IsLineFull(int rowNumber)
    {
        for(int x = 0;x < width; x++)
        {
            if (grid[x, rowNumber] == null)
            {
                return false;
            }
        }
        return true;
    }

    public void ClearLine(int rowNumber)
    {
        for (int x = 0; x < width; x++)
        {
            Destroy(grid[x, rowNumber].gameObject); // Destroy object in cells
            grid[x, rowNumber] = null; // Removes reference from grid
        }
    }

    public void ClearFullLines()
    {
        int linesCleared = 0;
        for(int y = 0; y < height; y++)
        {
            if (IsLineFull(y)) // Returns true or false.
            {
                ClearLine(y);
                ShiftRowsDown(y);
                y--; //Recheck the current row after shifting.
                linesCleared++;
            }
        }
        if(linesCleared > 0)
        {
            tetrisManager.CalculateScore(linesCleared);
        } //Without the {} it will do whatever the first line below

    }

    //Moves blocks that are above a line being cleared down by 1
    public void ShiftRowsDown(int clearedRow)
    {
        for (int y = clearedRow; y < height - 1; y++) 
        {
            for (int x = 0; x < width; x++) 
            {
                grid[x, y] = grid[x, y + 1];
                if(grid[x, y] != null)
                {
                    grid[x, y].position += Vector3.down;
                }
                grid[x,y + 1] = null;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        if (debugGrid != null)
        {
            for(int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Gizmos.DrawWireCube(debugGrid[i, j].position, Vector3.one);
                }
            }
        }
    }
}
