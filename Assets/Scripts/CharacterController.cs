using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour {

    public float m_maxSpeed;

    public PhysicMaterial m_material;

    private float m_flatVelocity;

    Rigidbody m_rb;

	// Use this for initialization
	void Start () {
        m_rb.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void FixedUpdate()
    {
        CalculateFlatMovment();
    }

    void CalculateFlatMovment()
    {
        float fNorm = m_rb.mass * m_material.dynamicFriction;
        float acceleration = (fNorm / m_rb.mass);
      //  float initVelocity = m_maxSpeed - acceleration;

        m_flatVelocity = acceleration;
    }

    public void MovePlayer(float xAxis, bool isJumping)
    {
        Vector3 moveVelocity = Vector3.zero;

        m_rb.velocity = moveVelocity;
    }
}
