using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Player : MonoBehaviourPunCallbacks
{
    CharacterController characterController;

    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    public float speed;
    public float normalSpeed = 12f;
    public float sprintingSpeed = 17f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;

    public float crouchingHeight = 1.25f;
    public float standingHeight;

    public GameObject playerCamera;

    public override void OnEnable()
    {
        if (photonView.IsMine)
        {
            characterController = GetComponent<CharacterController>();
        }
        else
        {
            playerCamera.SetActive(false);
        }
    }

    void Update()
    {
        if (photonView.IsMine) // Lokaler Client
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = transform.right * x + transform.forward * z;

            characterController.Move(move * speed * Time.deltaTime);

            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
            }

            velocity.y += gravity * Time.deltaTime;

            characterController.Move(velocity * Time.deltaTime);
            if (Input.GetKey(KeyCode.LeftShift))
            {
                Debug.Log("Sprinting");
                speed = sprintingSpeed;
            }
            else
            {
                speed = normalSpeed;
            }
            if (Input.GetKey(KeyCode.LeftControl))
            {
                Debug.Log("Crouching");
                characterController.height = crouchingHeight;
            }
            else
            {
                characterController.height = standingHeight;
            }
        }
    }
}
