using UnityEngine;

public class Actor : MonoBehaviour
{

	public ActorController controller;
	public Body body;

    public PhysicsObj physics = new PhysicsObj();
    public Vector2 inputForce = Vector2.zero;
    private Vector3 directionVector = Vector3.forward;
    
    public void Update()
	{

	}

	public void DoActionAlpha()
	{
		GGJ16.ActionSequencer sequencer = GetComponent<GGJ16.ActionSequencer>();
		sequencer.RunSequence(sequencer.sequences[0]);
	}

	public void DoActionBravo()
	{
		Debug.Log("Bravo!");
		body.SetColor(Color.green, 1f);
	}

    public Vector3 Forward
    {
        get { return this.directionVector; }
    }

    public Vector3 Backward
    {
        get { return -this.directionVector; }
    }

    public Vector3 Left
    {
        get { return Vector3.Cross(this.directionVector, Vector3.up); }
    }

    public Vector3 Right
    {
        get { return -this.Left; }
    }

    public void Turn(float angleDegrees)
    {
        this.directionVector = Quaternion.AngleAxis(angleDegrees, Vector3.up) * this.directionVector;
    }

    public void FixedUpdate()
    {
        if (inputForce.sqrMagnitude > 0)
        {
            this.directionVector.x = inputForce.normalized.x;
            this.directionVector.y = 0;
            this.directionVector.z = inputForce.normalized.y;
        }

        this.physics.FixedUpdate(new Vector3(inputForce.normalized.x, 0, inputForce.normalized.y));
        this.transform.position = this.physics.position;
    }

}