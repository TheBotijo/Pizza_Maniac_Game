using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMoveJump : MonoBehaviour
{
    //Rigidbody del player = rb
    Rigidbody rb;

    //Definim variables de Moviment
    [Header("Movement")]
    public float moveSpeed;
    private float lateSpeed;
    //public float rotationSpeed;

    float yRotation;
    public Transform orientation;
    public Transform playerObj;
    // public Transform playerObj;
    private PlayerInputMap _playerInput;
    public bool aiming;
    public Camera cam;
    private int fieldOfView = 60;
    float maxfield;

    public Transform cameraa;
    public Transform cameraRot;

    Vector3 moveDirection;

    //Animations
    public Animator animator;

    //Variables de Detecci� Ground
    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    public float groundDrag;

    [Header("Jump")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    private Shooting shootScript;

    private void Start()
    {
        cam.fieldOfView = fieldOfView;
        maxfield = cam.fieldOfView / 2;
        lateSpeed = moveSpeed;
        _playerInput = new PlayerInputMap();
        _playerInput.Juego.Enable();
        rb = GetComponent<Rigidbody>();
        shootScript = GetComponent<Shooting>();
        rb.freezeRotation= true;

        readyToJump = true;
    }

    public void Update()
    {
        //per comprovar si toca terra amb un vector de la meitat de l'altura del personatge + un marge
        grounded = Physics.Raycast(orientation.transform.position, Vector3.down, playerHeight, whatIsGround);
        
        SpeedControl();
        if(shootScript.shot == false)
        {
            PlayMove();
        }            
        
        //comprovem si toca el terra per aplicar un fregament al player
        if (grounded && readyToJump == false)
        {
            Invoke(nameof(notJump), 0.5f);


        }
        else if (grounded)
        {
            Debug.Log("suelo");
            rb.drag = groundDrag;
        }        
        else
            rb.drag = 0;

        if (_playerInput.Juego.Aim.IsPressed())
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, maxfield, 10f * Time.deltaTime);
            aiming = true;
        }
        else
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fieldOfView, 10f * Time.deltaTime);
            aiming = false;
        }
    }
    

    private void PlayMove() 
    {
        //recollir inputs de moviment en els eixos
        float horizontalInput = _playerInput.Juego.Move.ReadValue<Vector2>().x;
        float verticalInput = _playerInput.Juego.Move.ReadValue<Vector2>().y;

        if (_playerInput.Juego.Jump.WasPressedThisFrame() && readyToJump && grounded )
        {
            readyToJump = false;
            Jump();                     
        }

        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
                
        //moure's seguint el empty orientaci� endavant el eix vertical i orientaci� dreta el eix horitzontal
        Vector3 moveDirection = (orientation.forward * verticalInput + orientation.right * horizontalInput) * Time.deltaTime;

        //apliquem una for�a al moviment quan esta tocant al terra
        if (aiming)
            moveSpeed = lateSpeed / 2;
        else
            moveSpeed = lateSpeed;
        
        if (grounded && _playerInput.Juego.Run.IsPressed())
        {
            //if (_playerInput.Juego.Run.IsPressed() && flatVel.magnitude != 0)
            //{
                animator.SetBool("Run", true);
                animator.SetBool("Walk", false);
                rb.AddForce(moveDirection.normalized * moveSpeed * 25f, ForceMode.Force);
           //}
            //else animator.SetBool("Run", false);
            
        }
        else if (grounded)
        {
            
            if (_playerInput.Juego.Move.IsPressed())
            {
                animator.SetBool("Walk", true);
                animator.SetBool("Run", false);
                rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
            }
            else animator.SetBool("Walk", false);
        }
        else if(!grounded && flatVel.magnitude == 0)
        {
            rb.AddForce(moveDirection.normalized , ForceMode.Force);
        }
        else if (!grounded && flatVel.magnitude > (moveSpeed)) //a l'aire
        {
            rb.AddForce(moveDirection.normalized *10 * airMultiplier, ForceMode.Force);
        }
        else if (!grounded) //a l'aire
        {
            rb.AddForce(moveDirection.normalized * airMultiplier, ForceMode.Force);
        }
        //if (flatVel.magnitude == 0)
        //{
        //    animator.SetBool("Walk", false);
        //    animator.SetBool("Run", false);
        //} 
        if (flatVel.magnitude < 1f)
        {

            animator.SetBool("Run", false);
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // limitar la velocitat si aquesta es mes gran del que volem aconseguir
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            //tornem a aplicar aquesta nova velocitat al player
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
        
    }

    private void Jump()
    {
        // resetejar la velocitat de y per saltar sempre igual
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        //apliquem impulse perque es nomes una vegada
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

        if (animator.GetBool("Run") == true)
        {
            animator.SetBool("RunJump", true);
        }
        else
        {
            animator.SetBool("Jump", true);
        }

        Invoke(nameof(ResetJump), jumpCooldown);
    }
    private void ResetJump()
    {
        readyToJump = true;
    }
    void notJump()
    {
        animator.SetBool("Jump", false);
        animator.SetBool("RunJump", false);
    }
}