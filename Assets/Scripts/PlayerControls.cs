using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public new Rigidbody2D rigidbody;      // player Rbody
    public float speed = 3;         // go how fast?
    public float walkSpeed = 3;     // walking
    public float dashSpeed = 20;    // dashing
    public float jumpThrust = 2;    // jump how high?

    public bool isIdle = true;
    public bool isJumping = false;
    public bool isFalling = false;
    public bool goingLeft = false;
    public float currentBoost;
    public float maxBoost = 3.0f;

    public Collider2D normalCollider;
    public Collider2D dashCollider;
    // public Collider2D wallCollider;

    //public Vector2 lastPos;

     public Animator playerAnimator; // controls player animations

    void GoingLeft()
    {
        playerAnimator.transform.Rotate(0, 180, 0);
    }

    void Start()
    {
        //lastPos = player.position;
        rigidbody = GetComponent<Rigidbody2D>();
        isIdle = true;
        playerAnimator.SetBool("isIdle", true);
        isJumping = false;
        isFalling = false;
        currentBoost = 10.0f;
    }
    
    void Update()
    {
        // ---------- Checks ----------
        if(!isJumping && !isFalling)
        {
            if (!Input.anyKey)
            {
                isIdle = false;
                playerAnimator.SetBool("isIdle", true);
            }
            else
            {
                isIdle = true;
                playerAnimator.SetBool("isIdle", false);
            }
        }

        if (rigidbody.velocity.y < 0)
        {
            isJumping = false;
            playerAnimator.SetBool("isJumping", false);
            isFalling = true;
            playerAnimator.SetBool("isFalling", true);
        }
        else if (isFalling && rigidbody.velocity.y == 0)
        {
            isFalling = false;
            playerAnimator.SetBool("isFalling", false);
            playerAnimator.SetBool("isIdle", true);
        }

        // ---------- Movement ----------
        Vector3 tVel = Vector3.zero;
        float tempY = rigidbody.velocity.y;

        if (Input.GetKey(KeyCode.A))
        {
            if (!goingLeft)
            {
                GoingLeft();
                goingLeft = true;
            }
            tVel -= transform.right;
        }
        if (Input.GetKey(KeyCode.D))
        {
            if (goingLeft)
            {
                GoingLeft();
                goingLeft = false;
            }
            tVel += transform.right;
        }

        if (Input.GetKey(KeyCode.LeftShift) && currentBoost > 0)
        {
            playerAnimator.SetBool("isBoosting", true);
            normalCollider.gameObject.SetActive(false);
            dashCollider.gameObject.SetActive(true);
            speed = dashSpeed;
            currentBoost -= Time.deltaTime;
            if(currentBoost < 0)
            {
                currentBoost = 0;
            }
        }
        else
        {
            speed = walkSpeed;
            playerAnimator.SetBool("isBoosting", false);
            normalCollider.gameObject.SetActive(true);
            dashCollider.gameObject.SetActive(false);
        }

        tVel = tVel.normalized * speed;

        tVel.y = tempY;
        rigidbody.velocity = tVel;

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) // TODO remove W later
        {
            if (!isJumping && !isFalling)
            {
                playerAnimator.SetBool("isIdle", false);
                rigidbody.AddForce(Vector2.up * jumpThrust, ForceMode2D.Impulse);
                isJumping = true;
                playerAnimator.SetBool("isJumping", true);
            }
        }
        if (!Input.GetKey(KeyCode.LeftShift) && currentBoost < maxBoost)
        {
            currentBoost += Time.deltaTime;
        }
        if(currentBoost > maxBoost)
        {
            currentBoost = maxBoost;
        }

        //lastPos = player.position;
    }
}