using UnityEngine;
using System;
using System.Collections;

public class AnotherOVRTOuchpad : MonoBehaviour {

	public Transform ForwardDirection;

	 OVRPlayerController OVRC;
	// Use this for initialization

	void Awake()
	{
		
	}
	void Start () 
	{
		OVRC = GetComponent<OVRPlayerController> ();
		OVRTouchpad.Create ();
		OVRTouchpad.TouchHandler += HandleEventHandler;
	}

	void HandleEventHandler (object sender, EventArgs e)
	{
		OVRTouchpad.TouchArgs touchArgs = (OVRTouchpad.TouchArgs)e;
		OVRTouchpad.TouchEvent touchEvent = touchArgs.TouchType;

		switch(touchEvent)
		{
		case OVRTouchpad.TouchEvent.Down:
			//OVRC.UpdateMovement (Vector3.back);
			break;
		case OVRTouchpad.TouchEvent.Up:
			OVRC.UpdateMovement ();
			break;
		case OVRTouchpad.TouchEvent.Left:
			OVRC.UpdateMovement ();
			break;
		case OVRTouchpad.TouchEvent.Right:
			OVRC.UpdateMovement ();
			break;
		case OVRTouchpad.TouchEvent.SingleTap:
			break;
		}

	}
	// Update is called once per frame
	void Update () {
	
	}
}
