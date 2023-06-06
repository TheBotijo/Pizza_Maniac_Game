using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//TO DO
//Clamp yLerp to top and bottom margins
//Flag to relocate camera on the back of character automatically when running.
public class CameraController : MonoBehaviour
{
	[Header("PlayerRotate")]
	//New Input system
	public PlayerInputMap _playerInput;
    public Transform player;
    public Transform playerObj;
	public Transform orientation;
	public Transform combatLook;
	public Rigidbody rb;
	[Range(1f, 5f)]
    public float rotationSpeed;

    public bool invertMouseYaw = false;
	public bool invertGamepadYaw = false;
	public bool invertMousePitch = false;
	public bool invertGamepadPitch = false;
	public Vector2 pitchLimits = new Vector2(-10f, 20f);
	public Transform cameraTarget;
	public Vector2 zoomLimits = new Vector2(1, 10);
	public float yLerp = 1f;

	public Shooting shootScript;
    private Transform target;
	private float yawAngle;
	private float pitchAngle;
	private Transform cameraTransform;
	private bool hitted;
	bool pause = false;

	private void Awake()
	{
		pitchAngle = transform.localRotation.eulerAngles.x;
		yawAngle = transform.localRotation.eulerAngles.y;
		target = transform.parent;
		cameraTransform = GetComponentInChildren<Camera>().transform;
	}

	void Start()
    {
        transform.parent = null;
		_playerInput = new PlayerInputMap();
		_playerInput.Juego.Enable();
		//Ocultar el cursor del usuario para una mejor experiencia con el movimiento de camara
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

    void Update()
    {
		if (pause != true)
		{
			CameraRootRelocation();
			CameraRootRotation();
			PlayerRotation();
			CameraRelocation();
        }
    }

	private void PlayerRotation()
    {
        Vector3 CombatDir = player.position - new Vector3(cameraTarget.transform.position.x, cameraTarget.transform.position.y - 3, cameraTarget.transform.position.z);
        Vector3 viewDir = player.position - new Vector3(cameraTarget.transform.position.x, player.position.y, cameraTarget.transform.position.z);
        orientation.forward = viewDir.normalized;
        combatLook.forward = CombatDir.normalized;

        float horizontalInput = _playerInput.Juego.Move.ReadValue<Vector2>().x;
        float verticalInput = _playerInput.Juego.Move.ReadValue<Vector2>().y;

        Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;
        AimShootLookAt(inputDir);

        //playerObj.forward = Vector3.Slerp(playerObj.forward, orientation.forward, Time.deltaTime * 4);

    }

    private void AimShootLookAt(Vector3 inputDir)
    {
        if (!shootScript.aiming && shootScript.readyToShoot)
            playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * 20f);
        else
        {
            Vector3 WorldAimTarget = shootScript.rayHit.point;
            WorldAimTarget.y = transform.position.y;
            Vector3 AimDirection = (WorldAimTarget - transform.position).normalized;
            playerObj.forward = Vector3.Lerp(playerObj.forward, AimDirection, Time.deltaTime * 20);
        }
    }

    private void CameraRootRelocation()
	{
		//follow vertically with delay 
		float y = Mathf.Lerp(transform.position.y, target.position.y, yLerp);
		
		transform.position = new Vector3(target.position.x, y, target.position.z);
		
		//hard follow
		//transform.position = target.position;
	}

	private void CameraRootRotation()
	{
		float yaw = 0.05f * _playerInput.Juego.CameraMove.ReadValue<Vector2>().x * rotationSpeed;	//around Y axis, vertical
		float pitch = (-0.05f) *  _playerInput.Juego.CameraMove.ReadValue<Vector2>().y * rotationSpeed; //around X axis, horizontal

		pitchAngle += pitch;
		yawAngle += yaw;

		pitchAngle = Mathf.Clamp(pitchAngle, pitchLimits.x, pitchLimits.y);

		transform.localRotation = Quaternion.Euler(new Vector3(pitchAngle, yawAngle, 0));
	}

	private void CameraRelocation()
	{
		RaycastHit hit;
		hitted = Physics.Linecast(transform.position, cameraTarget.position, out hit, ~LayerMask.GetMask("WhatIsPlayer")); //avoid PlayerCharacter layer.
			
		if (hitted)
		{
			cameraTransform.position = hit.point;

			//hack to avoid near plane of the camera get inside collider
			cameraTransform.position = cameraTransform.TransformPoint(new Vector3(0, 0, 0.3f));
		}
		else
		{
			cameraTransform.position = cameraTarget.position;
		}
	}

    public void cameraSwitch(int num)
    {
        if (num == 0) pause = true;
		else pause = false;
    }
}
