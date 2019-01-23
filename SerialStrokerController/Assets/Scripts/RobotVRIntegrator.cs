using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StrokingRobot {
	public class RobotVRIntegrator : MonoBehaviour {

		public RobotManagerArvity robotManager;
		public BrushController virtualVisualManager;

		public float waitTime;
		// Update is called once per frame
		void Update () {
			
			if (Input.GetKeyDown ("space")) 	robotManager.SendMovementSegment (robotManager.distance, robotManager.speed, robotManager.angle); 

			if (Input.GetKeyDown ("s")) {
				StartCoroutine (StartMovementTrajectory ());
			}

			if (Input.GetKeyDown ("c")) 	robotManager.ClearNumberSegment ();
			if (Input.GetKeyDown ("r")) 	robotManager.ReadSegment ();
			if (Input.GetKeyDown ("t"))		robotManager.ReadDuration();	
		}

		private IEnumerator StartMovementTrajectory(){

			robotManager.CallDuration ();

			while (robotManager.ReadDuration() == 0) { //this waits for the duration to be different than 0 (its 0 at the beginning and after each clear)
				yield return null;
			}

			virtualVisualManager.time = robotManager.ReadDuration ();
			robotManager.StartMovement ();	

			yield return new WaitForFixedTime (waitTime);

			virtualVisualManager.TriggerCoroutine ();
			Debug.Log ("TOTAL TIME " + virtualVisualManager.time);

			yield return null;
		}
	}
}
