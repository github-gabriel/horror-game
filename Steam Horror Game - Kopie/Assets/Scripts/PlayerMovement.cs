using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;

public class PlayerMovement : NetworkBehaviour
{
    public GameObject Camera;
        
    private float speed;
    public GameObject PlayerModel;
    
    public CharacterController controller;
    
    public float gravity = -9.81f;

    public float normalSpeed = 2f;
    public float sprintingSpeed = 4f;
        
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public List<GameObject> meshes;
        
    Vector3 velocity;
    bool isGrounded;
        
    public float crouchingHeight = 1.25f;
    public float standingHeight;

    private void Start()
    {
        Camera.SetActive(false);
        foreach (GameObject mesh in meshes)
        {
            mesh.SetActive(false);
        }

        standingHeight = controller.height;
        speed = normalSpeed;
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Game")
        {
            if (meshes[0].activeSelf == false)
            {
                SetPosition();
            }
            if (hasAuthority)
            {
                Camera.SetActive(true);
                Movement();
            }
            else
            {
                foreach (GameObject mesh in meshes)
                {
                    mesh.SetActive(true);
                }
            }
        }
    }

    public void SetPosition()
    {
        transform.position = new Vector3(0, 5f, 0); // Spawn at position...
    }

    public void Movement()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);
        
        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = sprintingSpeed;
        }
        else
        {
            speed = normalSpeed;
        }
        if (Input.GetKey(KeyCode.LeftControl))
        {
            controller.height = crouchingHeight;
        }
        else
        {
            controller.height = standingHeight;
        }
    }
    
}
