using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float speed = 4;
    private bool isFacingLeft = true;
    Vector3 dir = new Vector3(-1, 0, 0);

    void Update()
    {
        transform.position += dir * speed * Time.deltaTime;
        if (transform.position.x > 13 || transform.position.x < -13)
        {
            dir.x *= -1;
        }

        if (dir.x < 0 && !isFacingLeft)
        {
            Flip();
        }
        else if (dir.x > 0 && isFacingLeft)
        {
            Flip();
        }
    }

    private void Flip()
    {
        isFacingLeft = !isFacingLeft;
        transform.Rotate(0f, 180f, 0f);
    }
    public int health = 100;
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        Destroy(gameObject);
    }
}
