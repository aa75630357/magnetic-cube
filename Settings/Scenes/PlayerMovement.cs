using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float MoveSpeed = 3f;    //推動力
    public float torqueforce = 3f;  //扭力

    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.D))
        {
            rb.AddForce(Vector2.right * MoveSpeed);
            rb.AddTorque(-torqueforce);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            rb.AddForce(Vector2.left * MoveSpeed);
            rb.AddTorque(torqueforce);
        }
    }
}
