using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {

    //To normalize joystick direction
    [SerializeField] Camera mainCam;
    public Vector3 Direction { private set; get; }
    public Vector2 JoyStickInput { private set; get; }

    public void Update()
    {
        JoyStickInput = GetInput();
        Direction = GetDirection();
    }

    Vector3 GetDirection()
    {
        Vector3 forward = GetAjustedForward();
        Vector3 right = GetAjustedRight();
        Debug.DrawRay(transform.position, forward * 10, Color.green);
        Debug.DrawRay(transform.position, right * 10, Color.green);

        Vector3 dir = forward * JoyStickInput.y + right * JoyStickInput.x;
        return (dir.magnitude > 1) ? dir.normalized : dir;
    }

    Vector3 GetAjustedForward()
    {
        Quaternion quaternion = Quaternion.Euler(0, mainCam.transform.eulerAngles.y, 0);
        return quaternion * Vector3.forward;
    }
    Vector3 GetAjustedRight()
    {
        Quaternion quaternion = Quaternion.Euler(0, mainCam.transform.eulerAngles.y, 0);
        return quaternion * Vector3.right;
    }

    Vector2 GetInput()
    {  
        //Mettre rewired
        return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }
}
