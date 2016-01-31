using UnityEngine;

public class Goal : GameEntity
{

	public Renderer[] currentRenderers;

    public override void OnSpawn()
    {
        this.physics.SetSize(1.5f);
        this.physics.fullStop = float.MaxValue;

        base.OnSpawn();
    }
}