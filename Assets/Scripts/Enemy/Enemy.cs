using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Public Variable



    #endregion Public Variable

    #region Private Variable

    /// <summary>
    /// TODO
    /// </summary>
    private float maxHealth = 100;

    /// <summary>
    /// TODO
    /// </summary>
    private float currentHealth;

    /// <summary>
    /// TODO
    /// </summary>
    private Animator animator;

    #endregion Private Variable

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        animator.SetBool("is_dead", false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        animator.SetTrigger("hurt");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("Enemy slain!");
        animator.SetBool("is_dead", true);
        this.enabled = false;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().isKinematic = true;
    }
}
