using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementManager : MonoBehaviour
{

    public GameObject cabinetPrefab;

    public int length;
    public int depth;
    public int height;
    public int thickness;
    public Material doorMat;

    private GameObject objPlacement;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 1000.0f))
            {
                Debug.Log(hit.point);
                objPlacement = Instantiate(cabinetPrefab, hit.point, Quaternion.identity);
            }
        }

        if (Input.GetMouseButton(0) && objPlacement)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 1000.0f))
            {
                objPlacement.transform.position = hit.point;
            }
        }
        if (Input.GetMouseButtonUp(0) && objPlacement)
        {
            objPlacement = null;
        }
    }
}
