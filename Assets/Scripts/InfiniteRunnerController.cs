﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InfiniteRunnerController : MonoBehaviour {
    private CharacterController controller;
    private Animator animator;
    private Vector3 moveDirection = Vector3.right;
    private bool isRunning = false;
    private Vector3 startPosition;
    private GameObject gameOverPanel;
    private AudioSource playerAudio;

    public float speed = 1;
    public float jumpSpeed = 2;
    public float gravity = 8;

    // Use this for initialization
    void Start () {
        gameOverPanel = GameObject.Find("GameOverPanel");
        gameOverPanel.SetActive(false);

        playerAudio = gameObject.GetComponent<AudioSource>();
        animator = gameObject.GetComponent<Animator>();
        controller = gameObject.GetComponent<CharacterController>();
        startPosition = transform.position;
        startRunning();
	}

    void Update()
    {
        //Button input
        if (Input.GetButton("Jump") && isRunning)
        {
            jump();
        }
        if (Input.GetButtonDown("Pause"))
        {
            setIsRunning(!isRunning);
        }

        if (isRunning)
        {
            //Applying gravity to the controller
            moveDirection.y -= gravity * Time.deltaTime;
            //Making the character move
            controller.Move(moveDirection * Time.deltaTime);
        }

        animator.SetFloat("horizontalVelocity", controller.velocity.y);
        animator.SetBool("isGrounded", controller.isGrounded);
        animator.SetBool("isWalking", controller.velocity.x > 0);

        //print("Move direction:" + controller.velocity);
    }

    void jump()
    {
        if (controller.isGrounded)
        {
            playerAudio.Play();
            moveDirection.y = jumpSpeed;
        }
    }

    public void startRunning()
    {
        moveDirection.x = speed;
    }

    public void setIsRunning(bool isRunning)
    {
        this.isRunning = isRunning;
    }

    public void restart()
    {
        gameOverPanel.SetActive(false);
        transform.position = startPosition;
        setIsRunning(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Hazard"))
        {
            setIsRunning(false);
            gameOverPanel.SetActive(true);
            print("Game over");
        }
        if (other.gameObject.CompareTag("Finish"))
        {
            SceneManager.LoadScene("VictoryScene");
        }
    }
}
