using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Rewired;

public class PlayerInput : MonoBehaviour
{

    //To normalize joystick direction
    Camera mainCam;
    [SerializeField] int playerId = 0;

    //public Rewired.Player input { get; private set; }

    public Vector3 Direction { private set; get; }
    public Vector2 JoyStickInput { private set; get; }

    [SerializeField] bool inputEnabled = true;

    private void Start()
    {
        mainCam = Camera.main;
       // input = ReInput.players.GetPlayer(playerId);
    }

    private void Awake()
    {
        SubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        //Disable when used etc
    }

    public void Update()
    {
        JoyStickInput = GetInput();
        Direction = GetDirection(JoyStickInput);
    }

    public bool GetButton(int inputID)
    {
        if (!inputEnabled)
            return false;

        return Input.GetButton(inputID.ToString());
        //return input.GetButton(inputID);
    }
    public bool GetButtonUp(int inputID)
    {
        if (!inputEnabled)
            return false;

        return Input.GetButtonUp(inputID.ToString());

        //return input.GetButtonUp(inputID);
    }
    public bool GetButtonDown(int inputID)
    {
        if (!inputEnabled)
            return false;

        return Input.GetButton(inputID.ToString());
        //return input.GetButtonDown(inputID);
    }


    public Vector3 GetDirection(Vector2 joystickInput)
    {
        Vector3 forward = GetAjustedForward();
        Vector3 right = GetAjustedRight();
        Debug.DrawRay(transform.position, forward * 10, Color.green);
        Debug.DrawRay(transform.position, right * 10, Color.green);

        Vector3 dir = forward * joystickInput.y + right * joystickInput.x;
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
        if (!inputEnabled)
            return Vector2.zero;


        return new Vector2(
            Input.GetAxis("Horizontal"),
            Input.GetAxis("Vertical"));
        //return new Vector2(
        //    input.GetAxis(RewiredConsts.Action.Horizontal),
        //    input.GetAxis(RewiredConsts.Action.Vertical));
    }
}
