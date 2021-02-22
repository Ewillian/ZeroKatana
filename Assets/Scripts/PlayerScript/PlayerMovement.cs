using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;

    private Rigidbody2D rigibody;
    private bool facingRight = true;
    private float moveDirection;

    // Call on spawn
    private void Awake()
    {
        rigibody = GetComponent<Rigidbody2D>();
    }

    // Call once each frame
    void Update()
    {
        // Get inputs
        ProcessInput();

        //animate
        Animate();

        // Move
        Move();
    }

    /// <summary>
    /// TODO commentary
    /// </summary>
    private void Move()
    {
        rigibody.velocity = new Vector2(moveDirection * moveSpeed, rigibody.velocity.y);
    }

    /// <summary>
    /// TODO commentary
    /// </summary>
    private void Animate()
    {
        if (moveDirection > 0 && !facingRight)
        {
            ChangePlayerDirection();
        }
        else if (moveDirection < 0 && facingRight)
        {
            ChangePlayerDirection();
        }
    }

    /// <summary>
    /// TODO commentary
    /// </summary>
    private void ProcessInput()
    {
        moveDirection = Input.GetAxis("Horizontal");
    }

    /// <summary>
    /// Using facingRight variable, change the character front rotation according to player input (left / right).
    /// </summary>
    /// <returns></returns>
    private void ChangePlayerDirection()
    {
        facingRight = !facingRight; // Reverse bool
        transform.Rotate(0f, 180f, 0f);
    }
}
