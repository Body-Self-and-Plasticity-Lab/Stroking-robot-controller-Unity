using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StrokingRobot {
	public class BrushController : MonoBehaviour {

		//private float speed = 0.5f;
		public float time;
		public GameObject targetToMove, fromPlace, toPlace;



		public void TriggerCoroutine(){
			Vector3 fromPlaceMapped = new Vector3 (fromPlace.transform.position.x, targetToMove.transform.position.y, fromPlace.transform.position.z); //only takes the x and z values from the targets
			Vector3 toPlaceMapped = new Vector3 (toPlace.transform.position.x, targetToMove.transform.position.y, toPlace.transform.position.z); //only takes the x and z values from the targets
			StartCoroutine(MoveVRRobot (time, targetToMove.transform, fromPlaceMapped, toPlaceMapped));
		}

		private IEnumerator MoveVRRobot(float time, Transform position, Vector3 origin, Vector3 target){
			
			float elapsedTime = 0f;

			//Forward
			while (elapsedTime < time) {
				elapsedTime += Time.deltaTime;
				position.position = Vector3.LerpUnclamped(origin, target, elapsedTime/time);
				yield return null;
			}

			elapsedTime = 0;

			//Return
			while (elapsedTime < time) {
				elapsedTime += Time.deltaTime;
				position.position = Vector3.LerpUnclamped(target, origin, elapsedTime/time);
				yield return null;
			}

		}
	}
}