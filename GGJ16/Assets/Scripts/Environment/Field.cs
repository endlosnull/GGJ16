using UnityEngine;
using System.Collections.Generic;
using Pooling;

public class Field : Singleton<Field>
{


	public Renderer[] currentRenderers;

	public Goal goal;
	public Ball ball;
	public List<Actor> allActors;

    private bool roundActive = false;

	public FloorEntity[,] tiles;
	float floorHalfX = 0;
	float floorHalfZ = 0;
	int floorLengthX = 23;
	int floorLengthZ = 13;

    public bool RoundActive
    {
        get
        {
            return this.roundActive;
        }
    }

    public void BeginRound()
	{
			
		GameObject goalObject = GameObjectFactory.Instance.Spawn("p-Goal", null, Vector3.zero, Quaternion.identity) ;
		goalObject.transform.localRotation = Quaternion.AngleAxis(-90f,Vector3.up);
		goalObject.name = "Goal";
		goal = goalObject.GetComponent<Goal>();
		goalObject.BroadcastMessage("OnSpawn", SendMessageOptions.DontRequireReceiver);

		GameObject ballObject = GameObjectFactory.Instance.Spawn("p-Ball", null, Vector3.forward*3f, Quaternion.identity) ;
		ballObject.name = "Ball";
        ball = ballObject.GetComponent<Ball>();
        ball.field = this;
        ballObject.BroadcastMessage("OnSpawn", SendMessageOptions.DontRequireReceiver);

        floorHalfX = floorLengthX*0.5f;
        floorHalfZ = floorLengthZ*0.5f;
        tiles = new FloorEntity[floorLengthX+1,floorLengthZ+1];
        for(int i=0; i<floorLengthX; ++i)
        {
        	for(int j=0; j<floorLengthZ; ++j)
	        {
	        	Vector3 startPos = GetTilePos(i,j);
	        	GameObject tileObject = GameObjectFactory.Instance.Spawn("p-Tile", this.transform, startPos, Quaternion.identity);
	        	tileObject.name = "Tile"+i+","+j;
	        	FloorEntity tile = tileObject.GetComponent<FloorEntity>();
	        	tiles[i,j] = tile;
	        	tile.SetColor(new Color(0f,0f,0f,0f));
        		tileObject.BroadcastMessage("OnSpawn", SendMessageOptions.DontRequireReceiver);
	        }
        }
		
		CamControl.Instance.target = ball.transform;

        this.roundActive = true;
    }

    private void RoundEnd()
    {
        this.roundActive = false;
    }

	public FloorEntity GetTile(Vector3 pos)
	{
		int x = (int)Mathf.Round(pos.x + floorHalfX-0.5f);
		int z = (int)Mathf.Round(pos.z + floorHalfZ-0.5f);
		if( x < floorLengthX && x > 0 && z < floorLengthZ && z > 0 )
		{
			return tiles[x,z];
		}
		return null;	
	}

	public Vector3 GetTilePos(int x, int z)
	{
		Vector3 pos = Vector3.zero;
		pos.x = x-floorHalfX+0.5f;
		pos.z = z-floorHalfZ+0.5f;
		return pos;
	}

    public void OnScore(int teamIndex)
    {
        CamControl.Instance.AddShake(0.2f);
        Debug.LogWarning("SCORE BY TEAM " + teamIndex.ToString());
        RoundEnd();
    }
}