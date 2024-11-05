using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{
    public Animator animator; // Ref till v�r animator
    public Rigidbody2D rb; // Ref till v�r rigidbody
    public GameObject Player;
    private PlayerMovement playerMovement;
    public int playerHealth = 100;
    private void Start()
    {
        playerMovement = Player.GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && playerMovement.isFacingRight)
        {
            rb.AddForce(new Vector2(-25f, 5f), ForceMode2D.Impulse);
            Debug.Log(message: "ow ow ow");
        }
        else if (collision.gameObject.CompareTag("Enemy") && !playerMovement.isFacingRight)
        {
            rb.AddForce(new Vector2(25f, 4f), ForceMode2D.Impulse);
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            playerHealth -= 10;
        }
    }

    private void Update()
    {
        if(playerHealth <= 0)
        {
            Die();
        }
    }
    private void Die() // Spela death animationen och s�tt rigidbody p� static s� vi inte kan r�ra oss
    {
        animator.SetTrigger("Death");
        rb.bodyType = RigidbodyType2D.Static;
    }
    private void RestartLevel() // Starta om scenen n�r vi d�r
    {
        SceneManager.LoadScene(0);
    }
}
