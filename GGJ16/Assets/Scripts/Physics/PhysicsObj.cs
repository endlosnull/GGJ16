using UnityEngine;
using System.Collections;

public class PhysicsObj
{
    public Vector3 position = Vector3.zero;

    public Vector3 velocity = Vector3.zero;
    public float drag = 2.0f;

    public float inputPower = 1;

    public void FixedUpdate(Vector3 input)
    {
        Vector3 moveDelta = Vector3.zero;
        
        moveDelta += input * Time.deltaTime;
        moveDelta += this.velocity * Time.deltaTime;

        this.velocity -= this.velocity * Time.deltaTime * drag;

        this.position += moveDelta;
	}
}

public interface IActionCard
{
    void FixedUpdate(Engine engine);
}

public class DashCard
{
    public void FixedUpdate(Engine engine)
    {
        engine.physics.velocity += Vector3.forward;
    }
}
