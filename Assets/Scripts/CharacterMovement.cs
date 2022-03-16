using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField]
    CharacterController plyController;

    [SerializeField]
    Camera plyCam;

    [SerializeField]
    [Range(0,5)]
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
    bool hasJumped =false;
    float jumpHeight = 3;

    private void Start()
    {
        moveType = walkSpeed;
        runSpeed = walkSpeed * 2;
    }
    void Update()
    {
        // Checks if char. is grounded, resets velocity if so
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        isOnWall = Physics.CheckSphere(wallCheck.position, wallDistance, groundMask);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = 0;
            hasJumped = false;
        }

        // Jump Key
        if ((Input.GetKeyDown(KeyCode.Space) && isGrounded) || (Input.GetKeyDown(KeyCode.Space) && isOnWall && !hasJumped))
        {
            if(isOnWall)
            {
                velocity.y = Mathf.Sqrt((jumpHeight *2) * -1f * gravity);
                hasJumped = true;
            }
            velocity.y = Mathf.Sqrt(jumpHeight * -1f * gravity);
            
        }

        // Sprint Key
        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveType = runSpeed;
        }
        else
            moveType = walkSpeed;

        // Change Player speed if crouched
        if (plyCam.transform.localPosition.y < 0.684)
        {
            moveType = walkSpeed / 4;
        }
        

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
