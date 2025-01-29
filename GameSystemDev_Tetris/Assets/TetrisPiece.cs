using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TetrisPiece : MonoBehaviour
{
    //This is meant to stop the pre-placed blocks from teleporting to the top of the board.
    [SerializeField] public bool isPreplaced;

    private TetrisGrid grid;
    private float dropInterval = 1f;
    private float dropTimer;
    bool isLocked = false;
    public void Start()
    {
        grid = FindObjectOfType<TetrisGrid>();
        dropTimer = dropInterval;

        if (isPreplaced == true)
        {
            LockFrozenPiece();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isPreplaced == false) 
        {
            HandleAutomaticDrop();
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Move(Vector3.left);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                Move(Vector3.right);
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                Move(Vector3.down);
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                RotatePiece();
            }
            if (Input.GetKeyDown(KeyCode.Space)) { FullDrop(); }
        }
        

    }

    //Drops the piece to the bottom immediately
    private void FullDrop()
    {
        do
        {
            Move(Vector3.down);
        } while (isLocked == false);
    }

    public void Move(Vector3 direction)
    {
        transform.position += direction;

        if (!IsValidPosition())
        {
            transform.position -= direction;
            
            if (direction == Vector3.down)
            {
                LockPiece();
            }
        }

    }

    private void RotatePiece()
    {
        transform.Rotate(0, 0, 90);

        //Store the original position and rotation for rollback
        Vector3 originalPosition = transform.position;
        Quaternion originalRotation = transform.rotation;

        if (!IsValidPosition())
        {
            if(!TryWallKick(transform.position, transform.rotation))
            {
                //revert if no wallkick available
                transform.position = originalPosition;
                transform.rotation = originalRotation;
                Debug.Log("Rotation invalid, reverting rotation/position");
            }
            else
            {
                Debug.Log("Rotation/Position adjusted with wall kick");
            }
            //transform.Rotate(0, 0, -90);
        }
    }

    public bool IsValidPosition()
    {
        foreach (Transform block in transform) 
        {
            Vector2Int position = Vector2Int.RoundToInt(block.position);

            if (grid.IsCellOccupied(position))
            {
                return false; // blocked or out of bounds
            }
        }
        return true;
    }

    private void HandleAutomaticDrop()
    {
        dropTimer -= Time.deltaTime;

        if(dropTimer <= 0)
        {
            Move(Vector3.down);
            dropTimer = dropInterval; //Reset timer
        }
    }

    private void LockPiece()
    {
        isLocked = true;
        foreach(Transform block in transform)
        {
            Vector2Int position = Vector2Int.RoundToInt(block.position);
            grid.AddBlockToGrid(block, position); //Add block to grid.
        }

        grid.ClearFullLines(); //Check for any full lines.
        if (FindObjectOfType<TetrisSpawner>())
        {
            FindObjectOfType<TetrisSpawner>().SpawnPiece();
        }
        Destroy(this); // Remove this script

    }

    private void LockFrozenPiece()
    {
        isPreplaced = true;
        foreach (Transform block in transform)
        {
            Vector2Int position = Vector2Int.RoundToInt(block.position);
            grid.AddBlockToGrid(block, position); //Add block to grid.
        }

        Destroy(this); // Remove this script

    }

    private bool TryWallKick(Vector3 originalPosition, Quaternion originalRotation)
    {
        //Define wall kicks under srs guidelines
        Vector2Int[] wallKickOffsets = new Vector2Int[]
        {
            new Vector2Int(1,0), //Move Right by 1
            new Vector2Int(-1,0), //Move Left by 1
            new Vector2Int(0,-1), //Move Down
            new Vector2Int(1,-1), //Move Diagonally right-down
            new Vector2Int(-1,-1), //Move Diagonally left-down

            new Vector2Int(2,0), //Move Right by 2
            new Vector2Int(-2,0), //Move Left by 2
            new Vector2Int(0,-2), //Move Down
            new Vector2Int(2,-1), //Move Diagonally right-down
            new Vector2Int(-2,-1), //Move Diagonally left-down
            new Vector2Int(2,-2), //Move Diagonally right-down
            new Vector2Int(-2,-2), //Move Diagonally left-down

            new Vector2Int(3,0), //Move Right by 3
            new Vector2Int(-3,0), //Move Left by 3
            new Vector2Int(0,-3), //Move Down
            new Vector2Int(3,-1), //Move Diagonally right-down
            new Vector2Int(-3,-1), //Move Diagonally left-down
            new Vector2Int(3,-2), //Move Diagonally right-down
            new Vector2Int(-3,-2), //Move Diagonally left-down
            new Vector2Int(3,-3), //Move Diagonally right-down
            new Vector2Int(-3,-3), //Move Diagonally left-down

            new Vector2Int(4,0), //Move Right by 4
            new Vector2Int(-4,0), //Move Left by 4
            new Vector2Int(0,-4), //Move Down
            new Vector2Int(4,-1), //Move Diagonally right-down
            new Vector2Int(-4,-1), //Move Diagonally left-down
            new Vector2Int(4,-2), //Move Diagonally right-down
            new Vector2Int(-4,-2), //Move Diagonally left-down
            new Vector2Int(4,-3), //Move Diagonally right-down
            new Vector2Int(-4,-3), //Move Diagonally left-down
            new Vector2Int(4,-4), //Move Diagonally right-down
            new Vector2Int(-4,-4), //Move Diagonally left-down

            new Vector2Int(5,0), //Move Right by 5
            new Vector2Int(-5,0), //Move Left by 5
            new Vector2Int(0,-5), //Move Down
            new Vector2Int(5,-1), //Move Diagonally right-down
            new Vector2Int(-5,-1), //Move Diagonally left-down
            new Vector2Int(5,-2), //Move Diagonally right-down
            new Vector2Int(-5,-2), //Move Diagonally left-down
            new Vector2Int(5,-3), //Move Diagonally right-down
            new Vector2Int(-5,-3), //Move Diagonally left-down
            new Vector2Int(5,-4), //Move Diagonally right-down
            new Vector2Int(-5,-4), //Move Diagonally left-down
            new Vector2Int(5,-5), //Move Diagonally right-down
            new Vector2Int(-5,-5), //Move Diagonally left-down

        };
        foreach (Vector2Int offset in wallKickOffsets) 
        {
            //Apply offset to the piece
            transform.position += (Vector3Int)offset; //You have to cast like this to go from vector 2 to 3

            //Check is the new position is valid
            if (IsValidPosition())
            {
                return true;
            }
            //Revert position if invalid
            transform.position -= (Vector3Int)offset;
        }

        return false;

        //loop through all of the offsets to see which one is valid
    }

}
