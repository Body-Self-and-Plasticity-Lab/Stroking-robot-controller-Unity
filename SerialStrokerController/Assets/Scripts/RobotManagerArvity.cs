using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RobotManagerArvity : MonoBehaviour { //changed class to use with Arvity for better handling serial messages than own solution.

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

	// Invoked when a line of data is received from the serial device.
	void OnMessageArrived(string msg)
	{
		Debug.Log("Message arrived: " + msg);
		error = msg;

		UseStrokerTime (msg);
	}

	//Deals with the Time response in the format: T:2.50,P:1.50,1.00 -> total time (T:) 2.50, time per segment (P:) 1.50, 1...
	void UseStrokerTime(string msg){
		
		if (msg.Substring (0, 1) == "T") {

			msg = msg.Replace("T:", "");//removes T:
			msg = msg.Replace("P:", "");//removes P:

			List<string> segmentTimes = msg.Split (',').ToList<string>();//makes into list separated by commas

			string totalTime = segmentTimes [0];//first item on the list is total time
			Debug.Log ("total time " + totalTime);

			for (int i = 1; i < segmentTimes.Count; i++) {
				//do something for each segment time.
				//Debug.Log ("The segment " + i + " " + segmentTimes[i]);
			}
		}
	}

	// Invoked when a connect/disconnect event occurs. The parameter 'success'
	// will be 'true' upon connection, and 'false' upon disconnection or
	// failure to connect.
	void OnConnectionEvent(bool success)
	{
		if (success)
			Debug.Log("Connection established");
		else
			Debug.Log("Connection attempt failed or disconnection detected");
	}
		
	void Update(){

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
