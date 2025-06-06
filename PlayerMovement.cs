using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;

    public float speed = 12f;
    public float gravity = -9.81f * 2;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;

    bool isGrounded;
    bool isMoving;

    private Vector3 lastPosition = new Vector3(0f, 0f, 0f);

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }



    private void Update()
    {
        // Проверка земли
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        // Сброс скорости
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // ряакция на нажатие
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Вектор движения
        Vector3 move = transform.right * x + transform.forward * z;

        // Движение
        controller.Move(move * speed * Time.deltaTime);

        // Проверка прыжка
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            // Вверх
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Падение
        velocity.y += gravity * Time.deltaTime;

        // Использование прыжка
        controller.Move(velocity * Time.deltaTime);

        if (lastPosition != gameObject.transform.position && isGrounded == true)
        {
            isMoving = true;

        }
        else
        {
            isMoving = false;
        }

        lastPosition = gameObject.transform.position;
    }
}
