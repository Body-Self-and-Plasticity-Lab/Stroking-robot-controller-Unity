using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System;

public class SerialManager : MonoBehaviour {

	[Tooltip("The name of the serial port")]
	public string portName;//the COM Port

	public int baudRate = 115200;//Fixed to 115200 for the robot.
	SerialPort serialDevice;


	void Start () {
		serialDevice = new SerialPort (portName, baudRate); //initializes a serial port
		if(serialDevice != null) serialDevice.Close (); //makes sure the device is closed before openning
		serialDevice.Open (); //opens serial device


	//	StartCoroutine (AsynchronousReadFromPort ((string s) => Debug.Log (s), () => Debug.LogError ("Error!"), 10000f));
	}

	void Update(){
		
	}

	public void WriteToPort(string message){
		if (serialDevice.IsOpen) {
			serialDevice.Write (message); //if the device is open, send string message when function is called.
			Debug.Log("sent message " + message); //writes message to console (this does not confirm that it was received)
		}
	}


	//Read from arduino
	public string ReadFromPort(int timeout = 0) {

		serialDevice.ReadTimeout = timeout;

		try {
			return serialDevice.ReadLine();
		}

		catch (TimeoutException) {
			return null;
		}
	}
		

	public IEnumerator AsynchronousReadFromPort(Action<string> callback, Action fail = null, float timeout = float.PositiveInfinity) {
		DateTime initialTime = DateTime.Now;
		DateTime nowTime;
		TimeSpan diff = default(TimeSpan);

		string dataString = null;

		do {
			try {
				dataString = serialDevice.ReadLine();
			}
			catch (TimeoutException) {
				dataString = null;
			}

			if (dataString != null)
			{
				callback(dataString);
				yield break; // Terminates the Coroutine
			} else
				yield return null; // Wait for next frame

			nowTime = DateTime.Now;
			diff = nowTime - initialTime;

		} while (diff.Milliseconds < timeout);

		if (fail != null)
			fail();
		yield return null;
	}



	void OnDisable() {
		serialDevice.Close(); //close device when finished.
	}
}