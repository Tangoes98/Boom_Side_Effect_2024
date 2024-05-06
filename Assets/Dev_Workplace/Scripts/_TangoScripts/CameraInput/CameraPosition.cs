using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPosition : MonoBehaviour
{
    public Vector2 LeftBottom; // (101, -227)
    public Vector2 RightBottom; // (227, -101)
    public Vector2 LeftTop; // (12, -138)
    public Vector2 RightTop; // (138, -12)


    private void Update()
    {
        transform.position = LimitCameraPosition(transform.position);
    }

    Vector3 LimitCameraPosition(Vector3 camPos)
    {
        Vector3 newPosition = new Vector3(0, camPos.y, 0);

        if (camPos.x >= LeftTop.x && camPos.x <= LeftBottom.x)
        {
            newPosition.x = camPos.x;
            newPosition.z = Mathf.Clamp(camPos.z, -camPos.x - 126, camPos.x - 150);
            return newPosition;
        }
        else if (camPos.x >= LeftBottom.x && camPos.x <= RightTop.x)
        {
            newPosition.x = camPos.x;
            newPosition.z = Mathf.Clamp(camPos.z, camPos.x - 328, camPos.x - 150);
            return newPosition;
        }
        else if (camPos.x >= RightTop.x && camPos.x <= RightBottom.x)
        {

            newPosition.x = camPos.x;
            newPosition.z = Mathf.Clamp(camPos.z, camPos.x - 328, -camPos.x + 126);
            return newPosition;
        }
        else if (camPos.x < LeftTop.x)
        {
            newPosition.x = LeftTop.x;
            newPosition.z = LeftTop.y;
            return newPosition;
        }
        else if (camPos.x > RightBottom.x)
        {
            newPosition.x = RightBottom.x;
            newPosition.z = RightBottom.y;
            return newPosition;
        }


        Debug.Log("Wrong Position");
        newPosition = Vector3.zero;
        return newPosition;
    }








}
