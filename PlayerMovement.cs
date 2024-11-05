using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Hur snabbt v�r karakt�r f�r r�ra sig
    public float jumpForce = 5f; // Vilken force hopp knappen kan g�ra
    public Transform groundCheck; // Kolla om spelaren har r�rt vid marken
    public float groundCheckRadius = 0.2f; // Inom vilken radie kan vi r�ra marken
    public LayerMask groundLayer; // Vilket lager har marken
    public bool isFacingRight = true; // Kolla vilket h�ll karakt�ren tittar i
    
    private float chargeTime = 1f;
    private float chargeBuildup = 15f;
    private float chargeCooldown = 1f;
    private bool isRushing;
    private bool canRush = true;


    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 24f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;


    private bool isGrounded = false; // Om vi �r p� marken eller inte
    private Rigidbody2D rb; // Ref till v�r rigidbody2D
    private Animator animator;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // H�mta v�r Rigidbody 2D
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (isRushing)
        {
            return;
        }


        if (isDashing)
        {
            return;
        }


        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer); // Kolla om spelaren �r p� marken
        float moveDirection = Input.GetAxis("Horizontal");  // Kolla om vi r�r oss horisontellt
        Move(moveDirection); // Flytta spelaren
        if (moveDirection > 0 && !isFacingRight) // Flippa spelaren s� den tittar i den riktningen som den r�r sig i
        {
            Flip();
        }
        else if (moveDirection < 0 && isFacingRight) // Flippa spelaren s� den tittar i den riktningen som den r�r sig i
        {
            Flip();
        }
        if (Input.GetButtonDown("Jump") && isGrounded) // Om vi tycker p� space, s� l�t spelaren hoppa
        {
            Jump();
        }
        if (Input.GetKey(KeyCode.C))
        {
            chargeBuildup += Time.deltaTime * 2;
        }
        if (Input.GetKeyUp(KeyCode.C))
        {
            StartCoroutine(Rush());
        }



        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
        animator.SetBool("Jump", !isGrounded);
    }

    private void FixedUpdate()
    {

        if (isRushing)
        {
            return;
        }


        if (isDashing)
        {
            return;
        }


    }
    private void Move(float direction)
    {
        Vector2 movement = new Vector2(direction * moveSpeed, rb.velocity.y); // r�kna ut hur vi r�r oss i relation till spelarens velocity
        rb.velocity = movement;
        float absoluteSpeed = Mathf.Abs(direction * moveSpeed);
        animator.SetFloat("Speed", absoluteSpeed);
    }
    private void Jump()
    {
        rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse); // L�gg till vertical force f�r att kunna hoppa
    }
    private void Flip()
    {
        isFacingRight = !isFacingRight;  // Flippa player spriten horisontellt
        transform.Rotate(0f, 180f, 0f);
    }
    private IEnumerator Dash()
    {

        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        if (isFacingRight)
        {
            rb.velocity = new Vector2(dashingPower * 1, 0f);
        }
        else if (!isFacingRight)
        {
            rb.velocity = new Vector2(dashingPower * -1, 0f);
        }
        yield return new WaitForSeconds(dashingTime);
        isDashing = false;
        rb.gravityScale = originalGravity;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
    private IEnumerator Rush()
    {
        canRush = false;
        isRushing = true;
        if (isFacingRight)
        {
            rb.velocity = new Vector2(chargeBuildup * 1f, 0f);
        }
        else if (!isFacingRight)
        {
            rb.velocity = new Vector2(chargeBuildup * -1f, 0f);
        }
        yield return new WaitForSeconds(chargeTime);
        isRushing = false;
        yield return new WaitForSeconds(chargeCooldown);
        canRush = true;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Door" && Input.GetKey(KeyCode.E))
        {
            transform.position = new Vector3(0f, 1.56f, 0f);
        }
    }

}   
