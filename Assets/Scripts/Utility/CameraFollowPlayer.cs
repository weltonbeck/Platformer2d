﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    public GameObject targetPlayer;
    public float cameraSpeed = 10;
    private float multiple;

    public float FinalUnitSize 		{ get { return finalUnitSize; } }
	public int   PixelsPerUnit 		{ get { return pixelsPerUnit; } }
	public int   VertUnitsOnScreen 	{ get { return verticalUnitsOnScreen; } }

	[SerializeField]
	private int pixelsPerUnit = 32;
	[SerializeField]
	private int verticalUnitsOnScreen = 4;
	private float finalUnitSize;
	private new Camera camera;
    

	void Awake()
	{
		camera = gameObject.GetComponent<Camera>();

		SetOrthographicSize();
        multiple = 1.0f / pixelsPerUnit;
	}

	void SetOrthographicSize()
	{
		ValidateUserInput();

		// get device's screen height and divide by the number of units 
		// that we want to fit on the screen vertically. this gets us
		// the basic size of a unit on the the current device's screen.
		var tempUnitSize = Screen.height / verticalUnitsOnScreen;

		// with a basic rough unit size in-hand, we now round it to the
		// nearest power of pixelsPerUnit (ex; 16px.) this will guarantee
		// our sprites are pixel perfect, as they can now be evenly divided
		// into our final device's screen height.
		finalUnitSize = GetNearestMultiple(tempUnitSize, pixelsPerUnit);

		// ultimately, we are using the standard pixel art formula for 
		// orthographic cameras, but approaching it from the view of:
		// how many standard Unity units do we want to fit on the screen?
		// formula: cameraSize = ScreenHeight / (DesiredSizeOfUnit * 2)
		camera.orthographicSize = Screen.height / (finalUnitSize * 2.0f);
	}

	int GetNearestMultiple(int value, int multiple)
	{
		int rem = value % multiple;
		int result = value - rem;
		if (rem > (multiple / 2))
			result += multiple;

		return result;
	}

    private float RoundToMultiple(float value, float multipleOf)
    {
        // Using Mathf.Round at each frame is a performance killer
        return (int)((value / multipleOf) + 0.5f) * multipleOf;
    }

    private void FixedUpdate() {
        float t = RoundToMultiple(cameraSpeed * Time.deltaTime, multiple);
        Vector3 tmpPosition = Vector3.Lerp(transform.position, targetPlayer.transform.position, t);
        tmpPosition.z = -10;
        transform.position = tmpPosition;
    }

	void ValidateUserInput()
	{
		if (pixelsPerUnit <= 0)
		{
			pixelsPerUnit = 1;
			Debug.Log("Warning: Pixels-per-unit must be greater than zero. " +
			          "Resetting to minimum allowed.");
		}
		else if (verticalUnitsOnScreen <= 0)
		{
			verticalUnitsOnScreen = 1;
			Debug.Log("Warning: Units-on-screen must be greater than zero." +
			          "Resetting to minimum allowed.");
		}
	}

}
