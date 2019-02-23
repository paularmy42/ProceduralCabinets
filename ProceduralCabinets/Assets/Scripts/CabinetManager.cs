using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum CabinetState
{
    Instantiated,
    Snapped,
    Placed
}
public class CabinetStateEvent : UnityEvent<CabinetState>
{
    //Do stuff in here if needed

}

public class CabinetManager : MonoBehaviour
{
    public static CabinetStateEvent OnCabinetStateChanged = new CabinetStateEvent();
    private CabinetState _cabinetState = CabinetState.Instantiated;
    public CabinetState cabinetState
    {
        get
        {
            return _cabinetState;
        }

        set
        {
            if (_cabinetState != value)
            {
                Debug.Log(string.Format("Cabinet State before changed from {0} to {1}", _cabinetState, value));
                _cabinetState = value;
                OnCabinetStateChanged.Invoke(_cabinetState);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        OnCabinetStateChanged.AddListener(CabinetStateChangedHandler);
        cabinetState = CabinetState.Instantiated;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<CabinetManager>() && cabinetState == CabinetState.Instantiated)
        {
            Debug.Log(string.Format("Current cabinet position: {0}", collision.transform.position));
            Debug.Log(string.Format("Current cabinet collider size: {0}", collision.gameObject.GetComponent<BoxCollider>().size));
            cabinetState = CabinetState.Snapped;
            if(collision.transform.position.x > gameObject.transform.position.x)
            {
                gameObject.transform.position = collision.transform.position - new Vector3(gameObject.GetComponent<BoxCollider>().size.x, 0, 0);
            }
            else
            {
                gameObject.transform.position = collision.transform.position + new Vector3(collision.gameObject.GetComponent<BoxCollider>().size.x, 0, 0);
            }
        }
    }

    public void CabinetStateChangedHandler(CabinetState newState)
    {
        if (newState == CabinetState.Snapped)
        {
            Debug.Log("Snapped");
        }
        if (newState == CabinetState.Placed)
        {
            Debug.Log("Placed");
        }
    }
}
