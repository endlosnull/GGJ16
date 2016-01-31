using UnityEngine;
using System.Collections;

public class PhysicsObj
{
	public bool enabled = true;

    private const float gravity = 8;

    private const float arenaWidth = 22;
    private const float arenaDepth = 12;
    private const float arenaHeight = 8;
    private Vector3 arenaMin = new Vector3(-arenaWidth / 2, 0, -arenaDepth / 2);
    private Vector3 arenaMax = new Vector3(arenaWidth / 2, arenaHeight, arenaDepth / 2);

    public Vector3 position = Vector3.zero;
    public Vector3 velocity = Vector3.zero;

    public float airControlMult = 0.4f;
    public float bounce = 0;
    public float drag = 1.7f;
    public float fullStop = 2.0f;

    public float inputPower = 2;

    private float halfObjSize = 0.5f;

    public float HalfSize
    {
        get { return this.halfObjSize; }
    }

    public static bool TestCollision(PhysicsObj a, PhysicsObj b, out Vector3 normal, out float penetration)
    {
        normal = Vector3.zero;
        penetration = 0;

        Vector3 diff = a.position - b.position;
        //diff.y = 0;
        normal = diff.normalized;
        if (diff.sqrMagnitude == 0)
            return false;

        float distance = diff.magnitude;
        penetration = b.HalfSize + a.HalfSize - distance;
        if (penetration > 0)
            return true;

        return false;
    }

    public void SetSize(float size)
    {
        this.halfObjSize = size / 2;
        this.arenaMin = new Vector3(-arenaWidth / 2 + halfObjSize, halfObjSize, -arenaDepth / 2 + halfObjSize);
        this.arenaMax = new Vector3(arenaWidth / 2 - halfObjSize, arenaHeight, arenaDepth / 2 - halfObjSize);
    }

    public void FixedUpdate(Vector3 input)
    {
		if (!enabled)
		{
			return;
		}

        bool onGround = this.position.y == this.arenaMin.y;

        Vector3 moveDelta = Vector3.zero;
        
        // Build moveDelta
        moveDelta += input * this.inputPower * (onGround ? 1 : airControlMult) * Time.deltaTime;
        moveDelta += this.velocity * Time.deltaTime;

        this.position += moveDelta;

        // Stay in bounds
        DoMove(moveDelta);

        // Apply drag
        if (onGround)
        {
            this.velocity -= this.velocity * Time.deltaTime * drag;
            if (this.velocity.sqrMagnitude < fullStop * fullStop)
                this.velocity = Vector3.zero;
        }

        // Apply gravity
        this.velocity.y -= gravity * Time.deltaTime;
    }

    private void DoMove(Vector3 moveDelta)
    {
        for (int dim = 0; dim < 3; dim++)
        {
            if (this.position[dim] < this.arenaMin[dim])
            {
                this.velocity[dim] = -this.velocity[dim] * this.bounce;
                this.position[dim] = this.arenaMin[dim];
            }
            else if (this.position[dim] > this.arenaMax[dim])
            {
                this.velocity[dim] = -this.velocity[dim] * this.bounce;
                this.position[dim] = this.arenaMax[dim];
            }
            else
            {
                this.position[dim] += moveDelta[dim];
            }
        }
    }
}
