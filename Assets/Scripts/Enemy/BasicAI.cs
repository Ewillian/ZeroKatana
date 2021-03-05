using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAI : MonoBehaviour
{
    [SerializeField] Transform player;

    private float agroRange;

    private float moveSpeed;

    private Rigidbody2D rigibody;

    private int moveDirection;

    /// <summary>
    /// TODO
    /// </summary>
    private bool is_facingRight = true;


    // Start is called before the first frame update
    void Start()
    {
        rigibody = GetComponent<Rigidbody2D>();
        moveSpeed = 2;
        agroRange = 5;
    }

    // Update is called once per frame
    void Update()
    {
        BaseAI();

        Animate();
    }

    private void Animate()
    {
        Debug.Log(moveDirection);
        if (moveDirection > 0 && !is_facingRight)
        {
            ChangeEnemyDirection();
        }
        else if (moveDirection < 0 && is_facingRight)
        {
            ChangeEnemyDirection();
        }
    }

    private void BaseAI()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer < agroRange)
        {
            Chase();
        }
    }

    private void Chase()
    {
        if (transform.position.x < player.position.x)
        {
            // Enemy is on the left and need to move right
            moveDirection = 1;
            rigibody.velocity = new Vector2(moveSpeed, rigibody.velocity.y);
        }
        else if (transform.position.x > player.position.x)
        {
            // Enemy is on the right and need to move left
            moveDirection = -1;
            rigibody.velocity = new Vector2(-moveSpeed, rigibody.velocity.y);
        }
    }

    /// <summary>
    /// Using facingRight variable, change the character front rotation according to enemy movement.
    /// </summary>
    /// <returns></returns>
    private void ChangeEnemyDirection()
    {
        is_facingRight = !is_facingRight; // Reverse bool
        transform.Rotate(0f, 180f, 0f);
    }
}
