using UnityEngine;

public class Breaker : MonoBehaviour
{
    public static Breaker Instance;
    public Sprite[] ballSprites;
    public Vector2[] adjacentDirections;
    public GameObject ballPrefab;
    [SerializeField] int rows;
    [SerializeField] int columns;
    [SerializeField] float spacing;
    private GameObject[,] grid; // 2D array to store the grid of balls
    

    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        CreateBallGrid();
    }

    private void CreateBallGrid()
    {
        grid = new GameObject[columns, rows]; // Initialize the grid array

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                Vector3 screenCenter = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0));

                float startX = transform.position.x - (columns - 1) * spacing / 2;
                float startY = transform.position.y + (rows - 1) * spacing / 2;

                Vector3 position = new Vector3(startX + j * spacing, startY - i * spacing, 0);
                GameObject ball = Instantiate(ballPrefab, position, Quaternion.identity);
                ball.transform.SetParent(transform);
                grid[j, i] = ball; // Store the ball in the grid

                SpriteRenderer spriteRenderer = ball.GetComponent<SpriteRenderer>();
                spriteRenderer.sprite = ballSprites[Random.Range(0, ballSprites.Length)];
            }
        }
    }

    // Method to update the grid after destroying balls
    public void UpdateGrid()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                if (grid[j, i] != null && !grid[j, i].activeSelf)
                {
                    // Destroy the ball object since it's no longer needed
                    Destroy(grid[j, i]);
                    grid[j, i] = null;
                }
            }
        }
    }


}
