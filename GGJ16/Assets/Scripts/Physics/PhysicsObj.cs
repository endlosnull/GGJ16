using UnityEngine;
using System.Collections;

public class PhysicsObj
{
    private const float gravity = 8;

    private const float arenaWidth = 22;
    private const float arenaDepth = 12;
    private const float arenaHeight = 8;
    private Vector3 arenaMin = new Vector3(-arenaWidth / 2, 0, -arenaDepth / 2);
    private Vector3 arenaMax = new Vector3(arenaWidth / 2, arenaHeight, arenaDepth / 2);

    public Vector3 position = Vector3.zero;
    public Vector3 velocity = Vector3.zero;

    public float drag = 1.7f;
    public float fullStop = 2.0f;

    public float inputPower = 2;

    private float halfObjSize = 1;

    public float HalfSize
    {
        get { return this.halfObjSize; }
    }

    public void SetSize(float size)
    {
        this.halfObjSize = size / 2;
        this.arenaMin = new Vector3(-arenaWidth / 2 + halfObjSize, halfObjSize, -arenaDepth / 2 + halfObjSize);
        this.arenaMax = new Vector3(arenaWidth / 2 - halfObjSize, arenaHeight, arenaDepth / 2 - halfObjSize);
    }

    public void FixedUpdate(Vector3 input)
    {
        Vector3 moveDelta = Vector3.zero;
        
        // Build moveDelta
        moveDelta += input * this.inputPower * Time.deltaTime;
        moveDelta += this.velocity * Time.deltaTime;

        this.position += moveDelta;

        // Stay in bounds
        DoMove(moveDelta);

        // Apply drag
        if (this.position.y == this.arenaMin.y)
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
                this.velocity[dim] = 0; // no bounce currently
                this.position[dim] = this.arenaMin[dim];
            }
            else if (this.position[dim] > this.arenaMax[dim])
            {
                this.velocity[dim] = 0; // no bounce currently
                this.position[dim] = this.arenaMax[dim];
            }
            else
            {
                this.position[dim] += moveDelta[dim];
            }
        }
    }
}
