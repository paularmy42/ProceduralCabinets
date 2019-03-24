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
                //Debug.Log(string.Format("Cabinet State before changed from {0} to {1}", _cabinetState, value));
                _cabinetState = value;
                OnCabinetStateChanged.Invoke(_cabinetState);
            }
        }
    }
    [HideInInspector]
    public bool collisionLeft = false;
    [HideInInspector]
    public bool collisionRight = false;
    [HideInInspector]
    public bool collisionRear = false;
    [HideInInspector]
    public Vector3 snapPos;

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
        float dx = collision.transform.position.x - gameObject.transform.position.x;
        float dz = collision.transform.position.z - gameObject.transform.position.z;
        if (!collisionLeft && !collisionRight) //collision.gameObject.GetComponent<CabinetManager>() && 
        {
            Debug.Log(string.Format("Collision object: {0}", collision.gameObject.name));
            Debug.Log(string.Format("Collision object position: {0}", collision.transform.position));
            Debug.Log(string.Format("Collision object collider scale: {0}", collision.gameObject.GetComponent<Collider>().bounds.size));
            Debug.Log(string.Format("Object position: {0}", gameObject.transform.position));
            Debug.Log(string.Format("Object collider scale: {0}", gameObject.GetComponent<Collider>().bounds.size));
            Debug.Log(string.Format("Collision dX: {0}", dx));
            Debug.Log(string.Format("Object size differential X: {0}", (collision.gameObject.GetComponent<Collider>().bounds.size.x / 2 + gameObject.GetComponent<Collider>().bounds.size.x / 2)));
            Debug.Log(string.Format("Collision dZ: {0}", dz));
            Debug.Log(string.Format("Object size differential Z: {0}", (collision.gameObject.GetComponent<Collider>().bounds.size.z / 2 + gameObject.GetComponent<Collider>().bounds.size.z / 2)));
            cabinetState = CabinetState.Snapped;
            //if(collision.transform.position.x > gameObject.transform.position.x) //Determine if cabinet snapped on left or right side
            //{
            //    gameObject.transform.position = collision.transform.position - new Vector3(gameObject.GetComponent<BoxCollider>().size.x, 0, 0);
            //}
            //else
            //{
            //    gameObject.transform.position = collision.transform.position + new Vector3(collision.gameObject.GetComponent<BoxCollider>().size.x, 0, 0);
            //}
            if(Mathf.Abs(dx) >= (collision.gameObject.GetComponent<Collider>().bounds.size.x / 2 + gameObject.GetComponent<Collider>().bounds.size.x / 2) - 0.05f)
            {
                if(dx>0)
                {
                    Debug.Log("Right");
                    collisionRight = true;
                    snapPos.x = collision.transform.position.x - (collision.gameObject.GetComponent<Collider>().bounds.size.x / 2 + gameObject.GetComponent<Collider>().bounds.size.x / 2);
                    snapPos.y = gameObject.transform.position.y;
                    snapPos.z = gameObject.transform.position.z;
                    gameObject.transform.position = snapPos;
                }
                else
                {
                    Debug.Log("Left");
                    collisionLeft = true;
                    snapPos.x = collision.transform.position.x + collision.gameObject.GetComponent<Collider>().bounds.size.x / 2 + gameObject.GetComponent<Collider>().bounds.size.x / 2;
                    snapPos.y = gameObject.transform.position.y;
                    snapPos.z = gameObject.transform.position.z;
                    gameObject.transform.position = snapPos;
                }
            }
        }
        if (!collisionRear)
        {
            cabinetState = CabinetState.Snapped;
            if (Mathf.Abs(dz) >= (collision.gameObject.GetComponent<Collider>().bounds.size.z / 2 + gameObject.GetComponent<Collider>().bounds.size.z / 2) - 0.05f)
            {
                Debug.Log("Rear");
                collisionRear = true;
                snapPos.x = gameObject.transform.position.x;
                snapPos.y = gameObject.transform.position.y;
                snapPos.z = collision.transform.position.z - (collision.gameObject.GetComponent<Collider>().bounds.size.z / 2 + gameObject.GetComponent<Collider>().bounds.size.z / 2);
                gameObject.transform.position = snapPos;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        Debug.Log("Collision exit");
        //cabinetState = CabinetState.Instantiated;
        //collisionLeft = false;
        //collisionRight = false;
        //collisionRear = false;
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
