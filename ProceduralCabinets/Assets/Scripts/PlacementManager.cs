using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class PlacementManager : MonoBehaviour
{
    public GameObject cabinetPrefab;
    public GameObject leftController;
    public GameObject rightController;
    private GameObject activeController;

    private static int _length;
    public static int length
    {
        get { return _length; }
        set { _length = value; }
    }

    private static int _depth;
    public static int depth
    {
        get { return _depth; }
        set { _depth = value; }
    }

    private static int _height;
    public static int height
    {
        get { return _height; }
        set { _height = value; }
    }

    public int thickness;

    public Material[] faceMaterials;

    private static Material _faceMat;
    public static Material faceMat
    {
        get { return _faceMat; }
        set { _faceMat = value; }
    }

    private static string _cabinetType;
    public static string cabinetType
    {
        get { return _cabinetType; }
        set { _cabinetType = value; }
    }

    private GameObject objPlacement;
    private VRTK_Pointer pointer;

    void Start()
    {
        CabinetManager.OnCabinetStateChanged.AddListener(CabinetStateChangedHandler);
        leftController.GetComponent<VRTK_ControllerEvents>().TriggerClicked += new ControllerInteractionEventHandler(OnLeftTriggerClicked);
        leftController.GetComponent<VRTK_ControllerEvents>().ButtonTwoPressed += new ControllerInteractionEventHandler(OnLeftTriggerClicked);
        rightController.GetComponent<VRTK_ControllerEvents>().TriggerClicked += new ControllerInteractionEventHandler(OnRightTriggerClicked);
    }

    void Update()
    {
        if (activeController)
        {
            if (activeController.GetComponent<VRTK_ControllerEvents>().triggerClicked && pointer && objPlacement)
            {
                objPlacement.transform.position = pointer.pointerRenderer.GetDestinationHit().point;
            }
            if (!activeController.GetComponent<VRTK_ControllerEvents>().triggerClicked && objPlacement)
            {
                objPlacement.transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                objPlacement.GetComponent<CabinetManager>().cabinetState = CabinetState.Placed;
                objPlacement = null;
            }
            if(objPlacement)
            {
                if(objPlacement.gameObject.GetComponent<CabinetManager>().cabinetState == CabinetState.Placed)
                {
                    objPlacement.transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                    objPlacement = null;
                }
            }
        }
    }

    void OnLeftTriggerClicked(object sender, ControllerInteractionEventArgs e)
    {
        activeController = leftController;
        InstantiateCabinet(activeController);
    }

    void OnRightTriggerClicked(object sender, ControllerInteractionEventArgs e)
    {
        activeController = rightController;
        //InstantiateCabinet(activeController);
    }

    void InstantiateCabinet(GameObject _controller)
    {
        VRTK_Pointer[] pointers = _controller.GetComponents<VRTK_Pointer>();
        pointer = pointers[1];
        Transform hit = pointer.pointerRenderer.GetDestinationHit().transform;
        if (hit.gameObject.layer == 8)
        {
            objPlacement = Instantiate(cabinetPrefab, pointer.pointerRenderer.GetDestinationHit().point, Quaternion.identity);
        }
    }

    public void CabinetStateChangedHandler(CabinetState newState)
    {
        if (newState == CabinetState.Snapped)
        {
            Debug.Log(string.Format("Placement Manager - Cabinet Snapped: {0}", objPlacement.gameObject.transform.position.ToString()));
            objPlacement.GetComponent<CabinetManager>().cabinetState = CabinetState.Placed;
        }
        if (newState == CabinetState.Placed)
        {
            Debug.Log("Placement Manager - Cabinet Placed");
            objPlacement.transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            objPlacement = null;
        }
    }
}



