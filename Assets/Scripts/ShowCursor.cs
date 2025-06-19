using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShowCursor : MonoBehaviour
{
    #region Singleton
    public static ShowCursor instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of ShowCursor found!");
            return;
        }
        instance = this;
    }
    #endregion
    [SerializeField] private Image cursor;

    // Update is called once per frame
    void Update()
    {
        if (cursor.enabled)
        {
            transform.position = Input.mousePosition;
        }
    }
    public void CursorEnabled()
    {
        cursor.enabled = true;
    }
    public void CursorDisabled()
    {
        cursor.enabled = false;
    }
}