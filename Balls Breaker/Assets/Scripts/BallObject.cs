using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BallObject : MonoBehaviour
{
    Sprite ballSprite;
    public bool isBroken;
    public GameObject splashPrefab;
    SpriteRenderer ballRenderer;

    void Start()
    {
        ballRenderer = GetComponent<SpriteRenderer>();
        GetBallSprite();
    }

    void GetBallSprite()
    {
        ballSprite = ballRenderer.sprite;
    }

    bool AreThereRemainingGroups()
    {
        BallObject[] allBalls = FindObjectsOfType<BallObject>();

        foreach (BallObject ball in allBalls)
        {
            if (!ball.isBroken)
            {
                BallObject[] connectedBalls = GetConnectedBalls(ball);

                // Check if there's a group of at least two balls with the same color.
                if (connectedBalls.Length >= 2)
                {
                    return true; // There's at least one group remaining.
                }
            }
        }

        return false; // No more groups remaining.
    }

    private void OnMouseDown()
    {
        if (!isBroken)
        {
    
            TryBreak(this);
        }
    }

    public void TryBreak(BallObject thisBall)
    {
        if (thisBall == null || thisBall.isBroken)
            return;

        BallObject[] adjacentBalls = GetAdjacentBalls(thisBall);

        // Ensure there are at least two adjacent balls of the same color
        int matchingColorCount = 0;
        foreach (BallObject ball in adjacentBalls)
        {
            if (ball != null && ball.ballSprite == thisBall.ballSprite)
            {
                matchingColorCount++;
            }
        }

        if (matchingColorCount > 0)
        {
            BreakBalls(thisBall);
        }
    }

    void BreakBalls(BallObject selectedBall)
    {
        BallObject[] connectedBalls = GetConnectedBalls(selectedBall);

        foreach (BallObject ball in connectedBalls)
        {
            ball.isBroken = true;
            BreakBall(ball.gameObject);
        }
    }

    BallObject[] GetConnectedBalls(BallObject startBall)
    {
        // Using a queue for a breadth-first search (BFS)
        Queue<BallObject> queue = new Queue<BallObject>();
        List<BallObject> connectedBalls = new List<BallObject>();
        HashSet<BallObject> visited = new HashSet<BallObject>();

        queue.Enqueue(startBall);

        while (queue.Count > 0)
        {
            BallObject currentBall = queue.Dequeue();
            connectedBalls.Add(currentBall);
            visited.Add(currentBall);

            foreach (BallObject adjacentBall in GetAdjacentBalls(currentBall))
            {
                if (!visited.Contains(adjacentBall) && adjacentBall.ballSprite == startBall.ballSprite)
                {
                    queue.Enqueue(adjacentBall);
                    visited.Add(adjacentBall);
                }
            }
        }

        return connectedBalls.ToArray();
    }

    private BallObject[] GetAdjacentBalls(BallObject thisBall)
    {
        List<BallObject> adjacentBalls = new List<BallObject>();

        foreach (Vector2 direction in Breaker.Instance.adjacentDirections)
        {
            Vector2 rayOrigin = (Vector2)thisBall.transform.position + direction;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.zero);

            if (hit.collider != null)
            {
                BallObject ball = hit.collider.GetComponent<BallObject>();

                if (ball != null && ball.ballSprite == thisBall.ballSprite)
                {
                    adjacentBalls.Add(ball);
                }
            }
        }

        return adjacentBalls.ToArray();
    }

    void BreakBall(GameObject obj)
    {
        GameManager.gameManager.AddScore();
        BallSplash();
        obj.SetActive(false);
        Breaker.Instance.UpdateGrid();

        if (!AreThereRemainingGroups())
        {
            GameManager.gameManager.EndGame();
        }
    }

    void BallSplash()
    {   
     
        Instantiate(splashPrefab, transform.position, Quaternion.identity);
        
        
    }


}
