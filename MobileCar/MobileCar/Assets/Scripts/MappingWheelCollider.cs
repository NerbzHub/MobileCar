using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MappingWheelCollider : MonoBehaviour
{

    public WheelCollider wc;

    private Vector3 wcCentre;
    private RaycastHit hit;

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        wcCentre = wc.transform.TransformPoint(wc.center);

        if (Physics.Raycast(wcCentre, -wc.transform.up, out hit, wc.suspensionDistance + wc.radius))
        {
            transform.position = hit.point + (wc.transform.up * wc.radius);
        }
        else
        {
            transform.position = wcCentre - (wc.transform.up * wc.suspensionDistance);
        }
    }
}
