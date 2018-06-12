using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementControls : MonoBehaviour
{
    Rigidbody rigidbody;
    //Centre of mass
    public Vector3 vecCOM = new Vector3(0, 0, 0);
    public WheelCollider[] wc;
    public Transform[] tyres;
    public int wcTorqueLength;
    public int wcDecelerationSpeedLength;
    public float m_Torque;
    public float m_steer = 25f;
    public float m_Brake = 10000f;
    public float m_decelerationSpeed = 1000f;
    public float m_OllieHeight;
    public bool BrakeAllowed;

    private Vector3 startingCOM;

    // Use this for initialization
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.centerOfMass = vecCOM;
        startingCOM = vecCOM;
    }

    private void Update()
    {
        HandBrake();
        RotatingRTyres();
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < wcTorqueLength; i++)
        {
            wc[i].motorTorque = Input.GetAxis("Vertical") * m_Torque;
        }

        wc[0].steerAngle = Input.GetAxis("Horizontal") * m_steer;
        wc[1].steerAngle = Input.GetAxis("Horizontal") * m_steer;

        //if(Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    Unstuck();
        //}
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            HackOllie();
        }

        DecelerationSpeed();

    }

    private void DecelerationSpeed()
    {
        if(!BrakeAllowed && Input.GetButton("Vertical") == false)
        {
            for (int i = 0; i < wcDecelerationSpeedLength; i++)
            {
                wc[i].brakeTorque = m_decelerationSpeed;
                wc[i].motorTorque = 0;
            }
        }
    }

    private void HandBrake()
    {
        if (Input.GetKey(KeyCode.Space))
            BrakeAllowed = true;
        else
            BrakeAllowed = false;

        if(BrakeAllowed)
        {
            for (int i = 0; i < wcTorqueLength; i++)
            {
                wc[i].brakeTorque = m_Brake;
                wc[i].motorTorque = 0.0f;
            }
        }

        else if(!BrakeAllowed && Input.GetButton("Vertical") == true)
        {
            for (int i = 0; i < wcTorqueLength; i++)
            {
                wc[i].brakeTorque = 0;
            }
        }
    }

    private void RotatingRTyres()
    {
        for (int i = 0; i < wcTorqueLength; i++)
        {
            tyres[i].Rotate(wc[i].rpm / 60 * 360 * Time.deltaTime, 0.0f, 0.0f);
        }

        for (int i = 0; i < 2; i++)
        {
            tyres[i].localEulerAngles = new Vector3(tyres[i].localEulerAngles.x, wc[i].steerAngle - tyres[i].localEulerAngles.z, tyres[i].localEulerAngles.z);
        }
    }

    private void ResetCOM()
    {
        vecCOM = startingCOM;
    }

    private void Unstuck()
    {
        Vector3 holder = vecCOM;
        vecCOM = new Vector3(vecCOM.x, vecCOM.y, -0.9f);
        Invoke("ResetCOM", 1.0f);
    }

    private void HackOllie()
    {
        rigidbody.AddForce(Vector3.up * m_OllieHeight, ForceMode.Impulse);
    }
}
