using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{

    #region Public Variable

    /// <summary>
    /// Layer defining enemy
    /// </summary>
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
    /// Player's sword center point (empty object transform)
    /// </summary>
    public Transform attackPoint;

    /// <summary>
    /// Layer defining ground
    /// </summary>
    public LayerMask groundObject;

    /// <summary>
    /// Player's current health
    /// </summary>
    public float currentHealth = 100f;

    #endregion Public Variable

    #region Private Variable

    /// <summary>
    /// Player's rigibody
    /// </summary>
    private Rigidbody2D rigibody;

    /// <summary>
    /// Player's animator
    /// </summary>
    private Animator animator;

    /// <summary>
    /// Boolean checking player facing direction
    /// </summary>
    private bool is_facingRight = true;

    /// <summary>
    /// Boolean checking player is grounded
    /// </summary>
    private bool is_grounded = false;

    /// <summary>
    /// Boolean checking player is jumping
    /// </summary>
    private bool is_jumping = false;

    /// <summary>
    /// Boolean checking if player is attacking
    /// </summary>
    private bool is_attacking;

    /// <summary>
    /// Boolean checking player is dead
    /// </summary>
    public bool is_dead = false;

    /// <summary>
    /// player's attack rate
    /// </summary>
    private float attackRate = 1f;

    /// <summary>
    /// player's net attack time
    /// </summary>
    private float nextAttackTime = 0f;

    /// <summary>
    /// Player's move direction updating on horizontal input
    /// </summary>
    private float moveDirection;

    /// <summary>
    /// Player Move speed
    /// </summary>
    private float moveSpeed = 8f;

    /// <summary>
    /// Player can apply damage within this range
    /// </summary>
    private float attackRange = 0.4f;

    /// <summary>
    /// Force use to make jump de player
    /// </summary>
    private float jumpForce = 7f;

    /// <summary>
    /// Radius to check if the player is on ground
    /// </summary>
    private float checkRadius = 1f;

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
    /// Trigger attack animation and apply damage to enemy (default 25 damage point).
    /// </summary>
    private IEnumerator Attack()
    {
        if (Input.GetMouseButtonDown(0) && is_dead == false)
        {
            if (Time.time >= nextAttackTime && is_attacking == false)
            {
                is_attacking = true;

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

                is_attacking = false;

                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

    /// <summary>
    /// Update moveDirection value with player input
    /// | Update is_jumping value
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
    /// Animate the player run animation and the player facing direction
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
    /// Player Movement / Jump logic
    /// </summary>
    private void Move()
    {
        if (is_dead == false)
        {
            rigibody.velocity = new Vector2(moveDirection * moveSpeed, rigibody.velocity.y);

            if (is_jumping)
            {
                rigibody.velocity = Vector2.up * jumpForce;
            }
            is_jumping = false;
        }
    }

    /// <summary>
    /// Using facingRight variable, change the character front rotation according to player input (left / right).
    /// </summary>
    private void ChangePlayerDirection()
    {
        if (is_dead == false)
        {
            is_facingRight = !is_facingRight;
            transform.Rotate(0f, 180f, 0f);
        }
    }

    /// <summary>
    /// Apply damage to the player
    /// </summary>
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        animator.SetTrigger("hurt");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Player die
    /// </summary>
    public void Die()
    {
        Debug.Log("Player slained!");
        animator.SetBool("is_dead", true);
        is_dead = true;
    }

    /// <summary>
    /// Draw player attack range
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