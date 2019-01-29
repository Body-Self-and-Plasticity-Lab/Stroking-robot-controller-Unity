using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotMessengerForTesting : MonoBehaviour {

	public SerialController serialManager;

	[Range(0,100)]
	public int distance;
	[Range(2,16)]
	public int speed;
	[Range(0,90)]
	public int angle;


	private int storedDistance, storedSpeed, storedAngle;
	private string error;
	private bool initialized;

	void Start() {
		storedDistance = distance;
		storedSpeed = speed;
		storedAngle = angle;

	}

	//SEND MOVEMENT SEGMENT SHOULD BE SENT N NUMBER OF TIMES, IN ORDER TO SUM 100 UNITS OF DISTANCE.
	//AFTER ALL THE SEGMENTS ARE SENT, THE STARTMOVEMENT() SHOULD BE CALLED
	//CLEAR NUMBER SEGMENT SHOULD BE CALLED BEFORE APPLYING A NEW SET OF MOVEMENT SEGMENTS.
		
	void Update(){


		/*
		if (serialManager.ReadFromPort (5) != null){// | serialManager.ReadFromPort (5) != " ") {
			error = serialManager.ReadFromPort (5);

			if (error != null)
				Debug.Log ("The stroker says: " + error);
		}*/

		if (error == "ACK")
			initialized = true;
		
		if(Input.GetKeyDown("space")) SendMovementSegment (distance, speed, angle); //if (distance != storedDistance || speed != storedSpeed || angle != storedAngle)

		if (Input.GetKeyDown ("s"))		StartMovement ();

		if (Input.GetKeyDown ("c")) 	ClearNumberSegment ();

		if (Input.GetKeyDown ("r")) 	ReadSegment ();

		if (Input.GetKeyDown ("t"))		ReadDuration();
	}

	public void SendMovementSegment(int distance, int speed, int angle){

		string message = ("SET,L" + distance.ToString("000") + ",MS" + speed.ToString("00") + ",SV" + angle.ToString("00") + ",R" + speed.ToString("00")); //("00") sets the number of digits to 2

		if (distance < 0 || distance > 100)
			Debug.LogError ("wrong distance value, should be between 0 and 100");
		else if (speed < 2 || speed > 16)
			Debug.LogError ("wrong speed value, should be between 2 and 16");
		else if (angle < 0 || angle > 180)
			Debug.LogError ("wrong angle value, should be between 0 and 180");

		else 
			serialManager.SendSerialMessage (message);
	}

	public void ClearNumberSegment(){
		serialManager.SendSerialMessage ("CLEAR");
	}

	public void StartMovement(){
		serialManager.SendSerialMessage ("START");
	}

	public void ReadSegment(){
		serialManager.SendSerialMessage ("READ");
	}

	public void ReadDuration(){
		serialManager.SendSerialMessage ("TIME");
	}


}
