using UnityEngine;

public class Goal : MonoBehaviour
{

	public Renderer[] currentRenderers;

    public PhysicsObj physics = new PhysicsObj();

    void Awake()
    {
        this.physics.SetSize(3);
        this.physics.fullStop = float.MaxValue;

        this.physics.position = this.transform.position;
        this.physics.position.y = 0;
    }
}