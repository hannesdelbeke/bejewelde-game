using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {

	private bool dragMap ;
	private Vector2 mouseDragInput ;
	private Vector3 camDragInput ;

	private Vector3 camOriginalPos ;
	private Vector3 camMapPos ;


	void OnEnable()
	{
		camOriginalPos = Camera.main.transform.position ;
		if(camMapPos != new Vector3(0,0,0) )
			Camera.main.transform.position = camMapPos;
	}

	void OnDisable()
	{
		camMapPos = Camera.main.transform.position ;
		Camera.main.transform.position = camOriginalPos;
	}
	//public float bufferDrag = 0.1f ;

	void Update () {

		if (Input.GetMouseButton (0)) {
			CameraPan ();
		} else {
			dragMap = false; 
		}
	}


	void CameraPan (){
		Vector2 clickScreen = new Vector2(Input.mousePosition.x  / Screen.width,Input.mousePosition.y  / Screen.height );
		if (dragMap == false ) {
			mouseDragInput = clickScreen;
			camDragInput = Camera.main.transform.position;
			dragMap = true;
		}
		Vector2 relativeMovement = clickScreen - mouseDragInput;
		Vector3 camOffset = new Vector3 (relativeMovement.x * 10 , relativeMovement.y *10 , 0 );
		Camera.main.transform.position = camDragInput - camOffset;
	}	
}
