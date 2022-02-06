using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public float Result
    {
        get
        {
            return result;
        }
    }
    private float result;

    private float minMoveDistance;
    private float maxDistance;
    private Vector3 firstPos;
    

    public void Start()
    {
        minMoveDistance = Mathf.Sqrt(Screen.width * Screen.height) * 0.025f;
        maxDistance = minMoveDistance *5;
        minMoveDistance = 0;
    }
    public void Update()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
            case RuntimePlatform.IPhonePlayer:
                if (Input.touchCount > 0)
                {
                    Touch item = Input.GetTouch(0);
                    switch (item.phase)
                    {
                        case TouchPhase.Began:
                            firstPos = item.position;
                            break;
                        case TouchPhase.Moved:
                            Vector3 deltapos = Input.mousePosition - firstPos;
                            float xdist = item.position.x - firstPos.x;
                            result = Mathf.Clamp(xdist / maxDistance, -1, 1);

                            break;
                        case TouchPhase.Ended:
                        case TouchPhase.Canceled:
                            result = 0;
                            break;
                    }
                }
                break;
            default:
                if (Input.GetMouseButtonDown(0))//tek dokunma
                {
                    firstPos = Input.mousePosition;
                    
                }
                else if (Input.GetMouseButton(0))//sürükleme
                {
                    Vector3 deltapos = Input.mousePosition - firstPos;
                    float xdist = Input.mousePosition.x - firstPos.x;
                    result = Mathf.Clamp(xdist / maxDistance, -1, 1);
                }
                else if (Input.GetMouseButtonUp(0))//kalkma
                {
                    result = 0;
                }
                break;
        }

      
    }


}
