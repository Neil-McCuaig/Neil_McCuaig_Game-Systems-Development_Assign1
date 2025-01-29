using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisSpawner : MonoBehaviour
{
    public GameObject[] tetrominoPrefabs;
    //public List<GameObject> tetrominos;
    private TetrisGrid grid;
    //private GameObject grid; Alternate way
    private GameObject nextPiece;
    // Start is called before the first frame update
    TetrisManager manager;

    void Start()
    {
        //manager.CalculateScore You can call from another script this way.
        grid = FindObjectOfType<TetrisGrid>();
        //grid = gameObject.Find<TetrisGrid>();
        if (grid == null)
        {
            //error out here
            return;
        }
        SpawnPiece();

    }
    public void SpawnPiece()
    {
        //Calculate the top centre of the grid and spawn there
        Vector3 spawnPosition = new Vector3(Mathf.Floor(grid.width / 2),grid.height - 3,0);
        if(grid.IsCellOccupied(new Vector2Int((int)Mathf.Floor(grid.width / 2f),
        grid.height - 3)))
        {
            spawnPosition = new Vector3( //Not working
            Random.Range(0, grid.width - 3), //Will also need to be defined so it works with your piece
            grid.height - 3,
            0);
        }

        if (nextPiece != null)
        { 
            nextPiece.SetActive(true);
            nextPiece.transform.position = spawnPosition;
        }
        else
        {
            //instantiate at random.
            nextPiece = InstantiateRandomPiece();
            nextPiece.transform.position = spawnPosition;
        }
        //nextPiece.transform.position = spawnPosition;
        nextPiece = InstantiateRandomPiece();
        nextPiece.SetActive(false); 
    }

    private GameObject InstantiateRandomPiece()
    {
        int index = Random.Range(0, tetrominoPrefabs.Length);
        return Instantiate(tetrominoPrefabs[index]);
    }
}


