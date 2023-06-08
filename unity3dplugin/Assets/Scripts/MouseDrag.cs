
using UnityEngine;
using System.Collections;


public class MouseDrag : MonoBehaviour {

public float minFov = 15f;
public float maxFov = 120f;
float sensitivity = 0.5f;
float rot_x = 90.0f;
float rot_y = 0;
float mouse_x = -1;
float mouse_y = -1;

float fov_mouse_x = -1;
float fov_mouse_y = -1;

private bool _playanim = false;

public static int colortest=0;

public static GameObject body;
public static GameObject color1;
public static GameObject color2;
public static GameObject color3;
public static GameObject cover;
public static GameObject doorFL;
public static GameObject doorFR;
public static GameObject doorRL;
public static GameObject doorRR;
public static GameObject wheel1_RR;
public static GameObject wheel1_RL;
public static GameObject wheel1_FR;
public static GameObject wheel1_FL;
public static GameObject wheel2_RR;
public static GameObject wheel2_RL;
public static GameObject wheel2_FR;
public static GameObject wheel2_FL;

public static GameObject interior;
public static GameObject interior_lowPoly;

public static int LeftSideWidth = 0;


public static GameObject back;
private RaycastHit hit;




		// Use this for initialization
		void Start ()
		{
		rot_x = Camera.main.transform.localRotation.ToEulerAngles().y;
					rot_y = Camera.main.transform.localRotation.ToEulerAngles().x;

	}

	void MouseDown() {

		}

		void Mouse_Drag(Vector2 mousePosition) {
			rot_x += -(mousePosition.x-mouse_x) * sensitivity;
			rot_y += -(mousePosition.y-mouse_y) * sensitivity;
			mouse_x = mousePosition.x;
			mouse_y = mousePosition.y;
			Camera.main.transform.localRotation = Quaternion.Euler( rot_y, rot_x, 0);
		}

		void Zoom(float distance) {
		//2 fingerprint touch

			float fov = Camera.main.fieldOfView;
			fov += distance * sensitivity;
			fov = Mathf.Clamp(fov, minFov, maxFov);
			Camera.main.fieldOfView = fov;
		}

	void MouseUp(Vector2 inputPosition) {
		//last anim has been finished
		
	}

	//void Update () {
	void OnGUI () {
		

/*if (Input.GetMouseButton(0) && Input.GetMouseButton(1))
{
float x = -Input.GetAxis("Mouse X")/2;
float y = Input.GetAxis("Mouse Y")/2;
transform.position = new Vector3(transform.position.x - x, transform.position.y - y, 8);
}
else*/
//left click
		bool leftside = true;

		//if (Input.mousePosition.x > 140) leftside = false;

		if (Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseDrag || Event.current.type == EventType.MouseUp) {
			if (Event.current.mousePosition.x > LeftSideWidth) leftside = false;
		}
		
		if (Input.GetMouseButton(1))
		{
			//if (Physics.Raycast (Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
			Zoom(Input.GetAxis("Mouse Y")*4.0f);
			//Zoom(mouse_yy - Event.current.mousePosition.y);	
            //mouse_yy = Event.current.mousePosition.y;
		} else
		if (!leftside)
		{

/*if (Input.GetMouseButton(0))
{
//if (Physics.Raycast (Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
{
rot_x += -Input.GetAxis("Mouse X") * sensitivity;
rot_y += -Input.GetAxis("Mouse Y") * sensitivity;
Camera.main.transform.localRotation = Quaternion.Euler( rot_y, rot_x, 0);
}
}
else*/
		{
			
			if (Event.current.type == EventType.MouseDown) {
				mouse_x = Event.current.mousePosition.x;
				mouse_y = Event.current.mousePosition.y;
			} else
			if (Input.GetMouseButton(0) && Event.current.type == EventType.MouseDrag) {
				Mouse_Drag(Event.current.mousePosition);
			}			
			else
			if (Event.current.type == EventType.MouseUp)
			{
				MouseUp(Input.mousePosition);	
			}
		} 


	}

		Touch touch;
		if (Input.touchCount == 1) {
			touch = Input.GetTouch(0); //finger 0
			// Touch touch = Input.touches [i];
			// Touch[] touch = Input.touches;
			// for (int i = 0; i < Input.touchCount; i++) {

			if (touch.position.x > LeftSideWidth) {
				switch (touch.phase) {
					case TouchPhase.Began:
						mouse_x = touch.position.x;
						mouse_y = touch.position.y;
					break;

					case TouchPhase.Moved:
						Mouse_Drag(touch.position);
					break;

					case TouchPhase.Ended:
						MouseUp(touch.position);
					break;
				}
			}
		}


		//2 fingers zoom
		if (Input.touchCount > 1) {

			touch = Input.GetTouch(1); //finger 1

			if (touch.position.x > LeftSideWidth) {
				switch (touch.phase) {
					case TouchPhase.Began:
						fov_mouse_x = touch.position.x;
						fov_mouse_y = touch.position.y;
					break;

					case TouchPhase.Moved:
						fov_mouse_x = touch.position.x - Screen.width/2;
						fov_mouse_y = touch.position.y - Screen.height/2;
								
						float fov = 60 - Mathf.Sqrt(fov_mouse_x*fov_mouse_x + fov_mouse_y*fov_mouse_y) / (1+Screen.height) * 100.0f;
						//fov += distance * sensitivity;
						fov = Mathf.Clamp(fov, minFov, maxFov);
						Camera.main.fieldOfView = fov;			
					break;

					case TouchPhase.Ended:
					break;
				}
			}	
		}
		Camera.main.transform.localPosition = Camera.main.transform.forward * - 2.0f;// + Vector3.down * 2.0f;	
	}
}