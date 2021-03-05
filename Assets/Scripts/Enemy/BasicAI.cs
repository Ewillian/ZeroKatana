using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAI : MonoBehaviour
{
    [SerializeField] Transform player;

    private float agroRange;

    private float moveSpeed;

    private Rigidbody2D rigibody;


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
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if ()
        {

        }
    }
}
