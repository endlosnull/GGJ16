using UnityEngine;

public class Goal : MonoBehaviour
{

	public Renderer[] currentRenderers;

    public PhysicsObj physics = new PhysicsObj();

    public virtual void OnSpawn()
    {
        this.physics.SetSize(3);
        this.physics.fullStop = float.MaxValue;

        this.physics.position = this.transform.position;
    }
}