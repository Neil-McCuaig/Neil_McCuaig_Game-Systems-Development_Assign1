using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TetrisManager : MonoBehaviour
{
    private TetrisGrid grid;
    public int score;

    [SerializeField]
    public GameObject gameOverText;

    [SerializeField]
    TextMeshProUGUI scoreText;

    [SerializeField]
    TetrisSpawner tetrisSpawner;

    public enum GameState { Menu, Gameplay, GameOver, Options}

    public GameState gameState;

    // Start is called before the first frame update
    void Start()
    {
        grid = FindObjectOfType<TetrisGrid>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckGameOver();
        scoreText.text = "Score: " + score;
    }

    /// <summary>
    ///  Calculates the score
    /// </summary>
    /// <param name="linesCleared"></param>

    public void CalculateScore(int linesCleared)
    {
        switch (linesCleared)
        {
            case 1: score += 100;
                break;
            case 2: score += 300;
                break;
            case 3: score += 500;
                break;
            case 4: score += 800;
                break;
            //If you want multiple cases to have the same result, just go case1: case2: Score += 100; and then break;
        }

        /*switch (gameState) 
        {
            case GameState.GameOver:
                break;
            case GameState.Gameplay: 
                break; 
        }*/

    }
    public void CheckGameOver()
    {
        for (int i = 0; i < grid.width; i++)
        {
            if (grid.IsCellOccupied(new Vector2Int(
            i,
            grid.height - 3))) //Temp was - 3, Might need editing
            {
                Debug.Log("Game over!");
                //Invoke does it after a few seconds. Only works with strings, so you need to wrap it in a function.
                gameOverText.SetActive(true);
                //Enables or disables objects
                //gameObject.SetActive(true);
                tetrisSpawner.gameObject.SetActive(false); //Turns off Tetris Spawner so you don't have it spam out pieces after game over
                Invoke("ReloadScene", 5);
            }

            /*if (grid.IsCellOccupied(new Vector2Int((
                int)Mathf.Floor(grid.width / 2f), 
                grid.height - 1))) //Temp was - 3
                {
                Debug.Log("Game over!");
                //Invoke does it after a few seconds. Only works with strings, so you need to wrap it in a function.
                gameOverText.SetActive(true);
                //Enables or disables objects
                //gameObject.SetActive(true);
                Invoke ("ReloadScene", 5);
                }*/

        }
    }
    public void ReloadScene()
    {
        SceneManager.LoadScene("Tetris");
    }
}
