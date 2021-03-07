using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    #region Private Variable

    /// <summary>
    /// Enemy's animator
    /// </summary>
    private Animator animator;

    /// <summary>
    /// Boolean checking enemy direction
    /// </summary>
    private bool is_facingRight = true;

    /// <summary>
    /// Boolean checking is enemy attacking
    /// </summary>
    private bool is_attacking;

    /// <summary>
    /// Enemy's max health
    /// </summary>
    private float maxHealth = 100;

    /// <summary>
    /// Enemy's current health
    /// </summary>
    private float currentHealth;

    /// <summary>
    /// Enemy's attack rate
    /// </summary>
    private float attackRate = 1f;

    /// <summary>
    /// Time until next attack
    /// </summary>
    private float nextAttackTime = 0f;

    /// <summary>
    /// Enemy's attack range
    /// </summary>
    private float attackRange = 0.5f;

    /// <summary>
    /// Enemy's attack damage
    /// </summary>
    private float attackDamage = 25;

    /// <summary>
    /// Enemy's agro range
    /// </summary>
    private float agroRange = 5f;

    /// <summary>
    /// Enemy's move speed
    /// </summary>
    private float moveSpeed = 2f;

    /// <summary>
    /// Enemy's rigibody
    /// </summary>
    private Rigidbody2D rigibody;

    /// <summary>
    /// Enemy's direction
    /// </summary>
    private int moveDirection;

    #endregion Private Variable

    #region SerializeField

    [SerializeField] Transform player;

    [SerializeField] Transform attackPoint;

    [SerializeField] LayerMask playerLayer;

    #endregion SerializeField

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        rigibody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        animator.SetBool("is_dead", false);
    }

    // Update is called once per frame
    void Update()
    {
        AgroPlayer();

        Animate();

        StartCoroutine(AttackPlayer());
    }

    /// <summary>
    /// Apply damage to himself
    /// </summary>
    /// <param name="damage"> Damage amount </param>
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
    /// Trigger die animation and make enemy no more collidable
    /// </summary>
    public void Die()
    {
        Debug.Log("Enemy slain!");
        animator.SetBool("is_dead", true);
        this.enabled = false;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().isKinematic = true;
    }
    
    /// <summary>
    /// Change enemy direction according to his movement
    /// </summary>
    private void Animate()
    {
        if (moveDirection > 0 && !is_facingRight)
        {
            ChangeEnemyDirection();
        }
        else if (moveDirection < 0 && is_facingRight)
        {
            ChangeEnemyDirection();
        }
    }

    /// <summary>
    /// Follow if player is in range else reset to idle animation
    /// </summary>
    private void AgroPlayer()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer < agroRange)
        {
            Chase();
        }
        else
        {
            // Switch walk animation to idle
            animator.SetFloat("speed", 0);
        }
    }

    /// <summary>
    /// Chase until player within attack range
    /// </summary>
    private void Chase()
    {

        if (transform.position.x > player.position.x && transform.position.x < player.position.x + 1.5 || transform.position.x < player.position.x && transform.position.x > player.position.x - 1.5)
        {
            // Prevent enemy to continue walking to player 
            animator.SetFloat("speed", 0); // Switch walk animation to idle
        }
        else if (transform.position.x < player.position.x)
        {
            // Enemy is on the left and need to move right
            animator.SetFloat("speed", moveDirection);
            moveDirection = 1;
            rigibody.velocity = new Vector2(moveSpeed, rigibody.velocity.y);
        }
        else if (transform.position.x > player.position.x)
        {
            // Enemy is on the right and need to move left
            animator.SetFloat("speed", Math.Abs(moveDirection));
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

    /// <summary>
    ///  Apply damage to player with delay for animation
    /// </summary>
    private IEnumerator AttackPlayer()
    {
        Collider2D playerInfo = Physics2D.OverlapCircle(attackPoint.position, attackRange, playerLayer);
        if (playerInfo != null && playerInfo.GetComponent<Player>().is_dead == false)
        {
            Debug.Log(playerInfo);
            if (Time.time >= nextAttackTime && is_attacking == false)
            {
                is_attacking = true;
                // Trigger attack animation
                animator.SetTrigger("attack");

                Debug.Log("att");

                // Delay Hit in relation to animation logic
                yield return new WaitForSeconds(1f);

                Collider2D player = Physics2D.OverlapCircle(attackPoint.position, attackRange, playerLayer);
                player.GetComponent<Player>().TakeDamage(attackDamage);
                is_attacking = false;

                nextAttackTime = Time.time + 1f / attackRate;
            }

        }
    }

    /// <summary>
    /// Drawn attack range
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    //Stop chasing player when object is in between
    //private void CanSeePlayer(float distance)
    //{
    //    Vector2 endPos = castPoint.position + Vector3.right * distance;
    //    RaycastHit2D hit = Physics2D.Linecast(castPoint.position, endPos, 1 << LayerMask.NameToLayer("Platforms"));

    //    if (hit.collider != null)
    //    {
    //        if (hit.collider.gameObject.CompareTag("Player"))
    //        {
    //            Chase();
    //        }
    //    }
    //}
}
