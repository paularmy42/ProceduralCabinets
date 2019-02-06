using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey("left"))
        {
            Camera.main.transform.RotateAround(Vector3.zero, Vector3.up, 50 * Time.deltaTime);
        }
        if (Input.GetKey("right"))
        {
            Camera.main.transform.RotateAround(Vector3.zero, Vector3.up, -50 * Time.deltaTime);
        }
    }
}
