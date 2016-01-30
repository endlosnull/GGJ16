using UnityEngine;
using System.Collections;

public class PhysicsObj : MonoBehaviour
{
    public Vector3 position = Vector3.zero;

    public Vector3 velocity = Vector3.zero;
    public float drag = 1;

    public Vector3 input = Vector3.zero;
    public float inputPower = 1;


    // Use this for initialization
    void Start()
    {
	}
	
	void FixedUpdate()
    {
        Vector3 moveDelta = Vector3.zero;
        moveDelta += this.input * Time.deltaTime;



        this.position += moveDelta;
	}
}
