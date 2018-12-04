using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System;

public class SerialManager : MonoBehaviour {

	[Tooltip("The name of the serial port")]
	public string portName;//the COM Port

	public int baudRate = 	115200;//Fixed to 115200 for the robot.
	SerialPort serialDevice;


	void Start () {
		serialDevice = new SerialPort (portName, baudRate); //initializes a serial port
		if(serialDevice != null) serialDevice.Close (); //makes sure the device is closed before openning
		serialDevice.Open (); //opens serial device
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


	void OnDisable() {
		serialDevice.Close(); //close device when finished.
	}
}