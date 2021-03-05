using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    #region Public Variable

    /// <summary>
    /// Player Move speed
    /// </summary>
    public float moveSpeed;

    /// <summary>
    /// TODO
    /// </summary>
    public float attackRange;

    public LayerMask enemyLayer;

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
    /// TODO
    /// </summary>
    public Transform attackPoint;

    #endregion Public Variable

    #region Private Variable

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
    private float attackRate = 1f;

    /// <summary>
    /// TODO
    /// </summary>
    private float nextAttackTime = 0f;

    /// <summary>
    /// TODO
    /// </summary>
    private float moveDirection;

    #endregion Private Variable

    // Call on spawn
    private void Awake()
    {
        // Get components
        rigibody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Call once each frame
    void Update()
    {

        // Get inputs
        ProcessInput();

        Animate();

        StartCoroutine(Attack());
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
    private IEnumerator Attack()
    {
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetMouseButtonDown(0))
            {
                // Trigger attack animation
                animator.SetTrigger("attack");

                // Delay Hit in relation to animation logic
                yield return new WaitForSeconds(0.5f);

                // Detect an attack in range of attack
                Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

                // Do damage for each enemy hit
                foreach (Collider2D enemy in hitEnemies)
                {
                    Debug.Log("Hit an " + enemy.name);
                    enemy.GetComponent<Enemy>().TakeDamage(25);
                }

                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
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

    /// <summary>
    /// TODO commentary
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}