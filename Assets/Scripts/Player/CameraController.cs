﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//TO DO
//Clamp yLerp to top and bottom margins
//Flag to relocate camera on the back of character automatically when running.
public class CameraController : MonoBehaviour
{
	//New Input system
	public PlayerInputMap _playerInput;

	public bool invertMouseYaw = false;
	public bool invertGamepadYaw = false;
	public bool invertMousePitch = false;
	public bool invertGamepadPitch = false;
	public Vector2 pitchLimits = new Vector2(-60f, 60f);
	public Transform cameraTarget;
	public Vector2 zoomLimits = new Vector2(1, 10);
	public float yLerp = 1f;

    private Transform target;
	private float yawAngle;
	private float pitchAngle;
	private Transform cameraTransform;
	private bool hitted;

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
		CameraRootRelocation();
		CameraRootRotation();
		CameraRelocation();
		CameraZoom();
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
		float yaw = _playerInput.Juego.CameraMove.ReadValue<Vector2>().x;	//around Y axis, vertical
		float pitch = (-1) *  _playerInput.Juego.CameraMove.ReadValue<Vector2>().y; //around X axis, horizontal

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

	private void CameraZoom()
	{
		float wheel = Input.GetAxis("Mouse ScrollWheel");

		if (hitted)
		{
			//trying to zoom out with a collider behind
			if(wheel < 0)
				return;
			//if zoom in with collider behind, start in camera position
			else if(wheel > 0)
				cameraTarget.position = cameraTransform.position;
		}

		float z = Mathf.Clamp(cameraTarget.localPosition.z + wheel, -zoomLimits.y, -zoomLimits.x);
		cameraTarget.localPosition = Vector3.forward * z;
	}
}
