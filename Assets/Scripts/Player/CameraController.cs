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
		Vector3 viewDir = player.position - new Vector3(cameraTarget.transform.position.x, cameraTarget.transform.position.y - 2f, cameraTarget.transform.position.z);
		orientation.forward = viewDir.normalized;

		float horizontalInput = _playerInput.Juego.CameraMove.ReadValue<Vector2>().x;
		float verticalInput = _playerInput.Juego.CameraMove.ReadValue<Vector2>().y;

		Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime);
        playerObj.forward = Vector3.Slerp(playerObj.forward, orientation.forward, Time.deltaTime);
        
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
