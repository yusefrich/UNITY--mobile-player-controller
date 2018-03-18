using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(PlayerController))]
public class Player : MonoBehaviour {

    //variables for player movement
    Vector3 velocity;
    float moveSpeed = .1f;

    //variables for buttom
    public Transform inputButtom;
    public Transform TouchInput;
    public Transform maxValue;
    public Transform minValue;
    public Camera gameCamera;

    //variables to reset and set the TouchInput object position
    bool firstTouch = true;
    Vector3 inputLastPossition;

    PlayerController controller;
	void Start () {
        //setting the first possition to reset the TouchInput object position
        inputLastPossition = TouchInput.localPosition;
        //getting reference to the PlayerController
        controller = GetComponent<PlayerController>(); 

    }

    void Update () {
        //getting the player input
        Vector2 input = InputController();
        Debug.Log("player input values = " + input);
        velocity.x = input.x * moveSpeed;
        velocity.y = input.y * moveSpeed;
        controller.Move(velocity);


	}
    //this function will return the input value, between 0 and 1, from the TouchInput object
    Vector2 InputController()
    {
        Ray ray = gameCamera.ScreenPointToRay(Input.mousePosition);
        Plane screenPlane = new Plane (Vector3.back, Vector3.zero);
        float rayDist;


        if (Input.GetMouseButton(0))
        {
            if (screenPlane.Raycast(ray, out rayDist))
            {

                Vector3 point = ray.GetPoint(rayDist);
                //print(point);
                //checkin if is the first touch on the screen to reposition the TouchInputObject
                if (firstTouch)
                {
                    //inputLastPossition = TouchInput.position;
                    TouchInput.position = new Vector3(point.x, point.y, TouchInput.position.z);
                    firstTouch = false;
                }

                inputButtom.position = new Vector3(Mathf.Clamp(point.x, minValue.position.x, maxValue.position.x), Mathf.Clamp(point.y, minValue.position.y, maxValue.position.y), inputButtom.position.z);

                Debug.DrawLine(ray.origin, point, Color.red);

            }

        }else
        {
            //reseting all possitions if not touching
            inputButtom.localPosition = new Vector3(0, 0, 0);
            TouchInput.localPosition = inputLastPossition;
            firstTouch = true;

        }
        //transforming the input possition in input values(between 0 and 1)
        

        return new Vector2(inputButtom.localPosition.x / maxValue.localPosition.x, inputButtom.localPosition.y / maxValue.localPosition.y);
    }
}
