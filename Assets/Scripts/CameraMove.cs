using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField]
    private Camera m_Camera;

    private Vector3 Origin;
    private Vector3 Diff;

    private bool drag = false;


    private void LateUpdate()
    {
        if(Input.GetMouseButton(0))
        {
            Diff = m_Camera.ScreenToWorldPoint(Input.mousePosition) - m_Camera.transform.position;
            if(drag == false)
            {
                drag = true;
                Origin = m_Camera.ScreenToWorldPoint(Input.mousePosition);
            }
        }
        else
        {
            drag = false;
        }

        if(drag)
        {
            m_Camera.transform.position = Origin - Diff;
        }
    }
}
