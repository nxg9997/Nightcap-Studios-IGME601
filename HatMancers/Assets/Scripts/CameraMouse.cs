using UnityEngine;
using System.Collections;
/// <summary>
/// Controls the rotation of the camera in relation to the mouse.
/// Authors: Abhi, David, Michael
/// Source: https://www.youtube.com/watch?v=bVo0YLLO43s
/// </summary>
public class CameraMouse : MonoBehaviour
{
    // Modifiable Attributes
    public PlayerController pController;
    public float cameraDistance = 10f;
    public float cameraHeight = 5f;
    public float mouseSensitivity = 4f;
    //public float scrollSensitvity = 2f;
    public float orbitDampening = 10f;
    public float scrollDampening = 6f;
    public float yClampMin = 30.0f;
    public float yClampMax = 90.0f;
    public Vector3 localRotation;
    public bool cameraDisabled = false;

    // Private PlayerNum Property (used for input)
    private int playerNum;

    // Private Camera Transforms
    private Transform cam;
    private Transform pivot;

    // Private Flags
    private bool isLocked;

    // Use this for initialization
    void Start()
    {
        cam = transform;
        pivot = transform.parent;

        LockCursor(true);

        playerNum = pController.GetPlayerNum();
    }


    void LateUpdate()
    {
        // If the game is paused, stop scene activities
        if (PlayerController.isGamePaused)
            return;

        // Unlock cursor if pausing
        if (Input.GetButtonDown("STA" + playerNum))
            isLocked = false;

        // If the player fires, then relock the cursor
        else if (!isLocked && (Input.GetButtonDown("LT" + playerNum) || Input.GetButtonDown("RT" + playerNum)))
        {
            LockCursor(true);
        }

        if (isLocked)
        {
            // Rotation of the Camera based on Mouse Coordinates
            if (Input.GetAxis("RSH" + playerNum) != 0 || Input.GetAxis("RSV" + playerNum) != 0)
            {
                localRotation.x += Input.GetAxis("RSH" + playerNum) * mouseSensitivity;
                localRotation.y -= Input.GetAxis("RSV" + playerNum) * mouseSensitivity;

                // Clamp the y Rotation to horizon and not flipping over at the top
                if (localRotation.y < yClampMin)
                    localRotation.y = yClampMin;
                else if (localRotation.y > yClampMax)
                    localRotation.y = yClampMax;
            }
            // Zooming Input from our Mouse Scroll Wheel
            /*if (Input.GetAxis("Mouse ScrollWheel") != 0f)
            {
                float ScrollAmount = Input.GetAxis("Mouse ScrollWheel") * scrollSensitvity;

                ScrollAmount *= (cameraDistance * 0.3f);

                cameraDistance += ScrollAmount * -1f;

                cameraDistance = Mathf.Clamp(cameraDistance, 1.5f, 100f);
            }*/
        }

        // Actual Camera Rig Transformations
        pivot.rotation = Quaternion.Euler(localRotation.y, localRotation.x, 0);
        //Quaternion QT = Quaternion.Euler(localRotation.y, localRotation.x, 0);
        //pivot.rotation = Quaternion.Lerp(pivot.rotation, QT, Time.deltaTime * orbitDampening);

        if (cam.localPosition.z != cameraDistance * -1f)
        {
            cam.localPosition = new Vector3(0f, cameraHeight, Mathf.Lerp(cam.localPosition.z, cameraDistance * -1f, Time.deltaTime * scrollDampening));
        }
    }

    private void LockCursor(bool l)
    {
        isLocked = l;
        if (isLocked)
        {
            // Make the mouse pointer invisible
            Cursor.visible = false;

            // Lock the mouse pointer within the game area
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            // Make the mouse pointer visible
            Cursor.visible = true;

            // Unlock the mouse pointer so player can click on other windows
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
