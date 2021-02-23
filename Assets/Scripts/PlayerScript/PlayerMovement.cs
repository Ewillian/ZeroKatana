using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    /// <summary>
    /// Player Move speed
    /// </summary>
    public float moveSpeed;

    /// <summary>
    /// Player top for checking if hit ceiling
    /// </summary>
    public Transform topCheck;

    /// <summary>
    /// Player base for checking if hit ground
    /// </summary>
    public Transform baseCheck;

    /// <summary>
    /// TODO
    /// </summary>
    public LayerMask groundObject;

    /// <summary>
    /// TODO
    /// </summary>
    public float jumpForce;

    /// <summary>
    /// TODO
    /// </summary>
    public float checkRadius;

    /// <summary>
    /// Player rigibody
    /// </summary>
    private Rigidbody2D rigibody;

    /// <summary>
    /// TODO
    /// </summary>
    private Animator animator;

    /// <summary>
    /// TODO
    /// </summary>
    private bool is_facingRight = true;

    /// <summary>
    /// TODO
    /// </summary>
    private bool is_grounded = false;

    /// <summary>
    /// TODO
    /// </summary>
    private bool is_jumping = false;

    /// <summary>
    /// TODO
    /// </summary>
    private float moveDirection;

    // Call on spawn
    private void Awake()
    {
        rigibody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Call once each frame
    void Update()
    {
        // Get inputs
        ProcessInput();

        //animate
        Animate();

        if (Input.GetMouseButtonDown(0))
        {
            /// Trigger attack animation
            animator.SetTrigger("attack");
        }
    }

    private void FixedUpdate()
    {
        // Check if touch ground
        is_grounded = Physics2D.OverlapCircle(baseCheck.position, checkRadius, groundObject);

        // Move
        Move();
    }

    /// <summary>
    /// TODO commentary
    /// </summary>
    private void ProcessInput()
    {
        moveDirection = Input.GetAxis("Horizontal");
        if (Input.GetButtonDown("Jump") && is_grounded)
        {
            is_jumping = true;
        }
    }

    /// <summary>
    /// TODO commentary
    /// </summary>
    private void Animate()
    {
        if (moveDirection > 0 && !is_facingRight)
        {
            ChangePlayerDirection();
        }
        else if (moveDirection < 0 && is_facingRight)
        {
            ChangePlayerDirection();
        }

        /// Trigger run animation
        animator.SetFloat("f_moveSpeed", System.Math.Abs(moveDirection));
    }

    /// <summary>
    /// TODO commentary
    /// </summary>
    private void Move()
    {
        rigibody.velocity = new Vector2(moveDirection * moveSpeed, rigibody.velocity.y);

        // No velocity on stop
        //if (moveDirection.Equals(0))
        //{
        //    rigibody.velocity = new Vector2(0, rigibody.velocity.y);
        //}

        if (is_jumping)
        {
            rigibody.velocity = Vector2.up * jumpForce;
        }
        is_jumping = false;
    }

    /// <summary>
    /// Using facingRight variable, change the character front rotation according to player input (left / right).
    /// </summary>
    /// <returns></returns>
    private void ChangePlayerDirection()
    {
        is_facingRight = !is_facingRight; // Reverse bool
        transform.Rotate(0f, 180f, 0f);
    }
}
