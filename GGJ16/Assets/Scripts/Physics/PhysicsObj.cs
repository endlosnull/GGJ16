using UnityEngine;
using System.Collections;

public class PhysicsObj
{
    private const float arenaWidth = 4;
    private const float arenaDepth = 4;
    private const float arenaHeight = 4;
    private Vector3 arenaMin = new Vector3(-arenaWidth / 2, 0, -arenaDepth / 2);
    private Vector3 arenaMax = new Vector3(arenaWidth / 2, arenaHeight, arenaDepth / 2);

    public Vector3 position = Vector3.zero;

    public Vector3 velocity = Vector3.zero;
    public float drag = 0.5f;

    public float inputPower = 2;

    public float halfObjSize = 1;

    public void SetSize(float size)
    {
        this.halfObjSize = size / 2;
        this.arenaMin = new Vector3(-arenaWidth / 2 + halfObjSize, 0, -arenaDepth / 2 + halfObjSize);
        this.arenaMax = new Vector3(arenaWidth / 2 - halfObjSize, arenaHeight, arenaDepth / 2 - halfObjSize);
    }

    public void FixedUpdate(Vector3 input)
    {
        Vector3 moveDelta = Vector3.zero;
        
        // Build moveDelta
        moveDelta += input * Time.deltaTime;
        moveDelta += this.velocity * Time.deltaTime;

        Vector3 oldPosition = this.position;
        this.position += moveDelta;

        // Stay in bounds
        TestBoundsCollision(oldPosition, moveDelta);

        // Apply drag
        this.velocity *= Time.deltaTime * (1 - drag);
	}

    private void TestBoundsCollision(Vector3 oldPosition, Vector3 moveDelta)
    {
        for (int dim = 0; dim < 3; dim++)
        {
            if (this.position[dim] < this.arenaMin[dim])
            {
                this.velocity[dim] = 0; // no bounce currently
                this.position = oldPosition + moveDelta * (moveDelta[dim] / (this.position[dim] - this.arenaMin[dim]));
                this.position[dim] = this.arenaMin[dim];
                moveDelta[dim] = 0;
            }
            else if (this.position[dim] > this.arenaMax[dim])
            {
                this.velocity[dim] = 0; // no bounce currently
                this.position = oldPosition + moveDelta * (moveDelta[dim] / (this.position[dim] - this.arenaMax[dim]));
                this.position[dim] = this.arenaMax[dim];
                moveDelta[dim] = 0;
            }
        }
    }
}
