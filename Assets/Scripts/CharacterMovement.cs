using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField]
    CharacterController plyController;

    [SerializeField]
    Text healthtxt, staminatxt;

    [SerializeField]
    Camera plyCam;

    [SerializeField]
    [Range(0, 5)]
    float walkSpeed;
    float runSpeed;
    float moveType; // Current speed used, changes if holding sprint or not

    [SerializeField]
    Transform groundCheck, wallCheck;
    float groundDistance = 0.4f;
    float wallDistance = 1;

    [SerializeField]
    LayerMask groundMask;

    Vector3 velocity;
    float gravity = -9.81f * 2; // Change second value to determine fall speed
    bool isGrounded;
    bool isOnWall;
    bool isRunning;
    bool isPaused = false;
    bool hasJumped = false;
    bool hasStamina = true;
    float jumpHeight = 3;
    float stamina = 5;
    CapsuleCollider playerCol;





    private void Start()
    {
        moveType = walkSpeed;
        runSpeed = walkSpeed * 2;

    }
    void Update()
    {

        if (Time.timeScale == 0)
        {
            isPaused = true;
        }
        else
        {
            isPaused = false;
        }

        // Checks if char. is grounded, resets velocity if so
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        isOnWall = Physics.CheckSphere(wallCheck.position, wallDistance, groundMask);
        staminatxt.text = stamina.ToString();

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = 0;
            hasJumped = false;
        }

        // Jump Key
        if ((Input.GetKeyDown(KeyCode.Space) && isGrounded) || (Input.GetKeyDown(KeyCode.Space) && isOnWall && !hasJumped))
        {
            if (isOnWall)
            {
                velocity.y = Mathf.Sqrt((jumpHeight * 2) * -1f * gravity);
                hasJumped = true;
            }
            velocity.y = Mathf.Sqrt(jumpHeight * -1f * gravity);

        }

        // Sprint Key with working stamina
        if (Input.GetKey(KeyCode.LeftShift) && hasStamina)
        {
            isRunning = true;
            moveType = runSpeed;
            stamina -= Time.deltaTime;
            if (stamina <= 0)
            {
                moveType = walkSpeed;
                hasStamina = false;
                //  Debug.Log("No Stamina");
            }
        }
        else
        {
            moveType = walkSpeed;
            isRunning = false;
        }

        if ((stamina < 5 || stamina == 0) && (!isRunning && isGrounded))
        {
            stamina += Time.deltaTime;
            hasStamina = true;
            //  Debug.Log("Regen Stamina");
        }

        // Change Player speed if crouched
        if (plyCam.transform.localPosition.y < 0.684)
        {
            moveType = walkSpeed / 4;
        }
        if(!isPaused)
        {
            // Values to determine look position

            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            // Creating a move directon variable
            Vector3 moveDirection = transform.right * x + transform.forward * z;

            // Translating the character controller in the direction created above
            plyController.Move(moveDirection * moveType * Time.deltaTime);

            // Keeping track of player velocity to calculate gravity
            velocity.y += gravity * Time.deltaTime;

            plyController.Move(velocity * Time.deltaTime);
        }

    }
}


