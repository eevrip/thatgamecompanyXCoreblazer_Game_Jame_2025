using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class Click : MonoBehaviour
{

    public UnityEvent onClick;
    private void OnMouseDown()
    {

        if (EventSystem.current.IsPointerOverGameObject())
        {

            return;
        }

        onClick.Invoke();
    }





}
