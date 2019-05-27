using System.Collections;
using System.Collections.Generic;

public class Planet {

	public float xPos;
	public float yPos;
	public float radius;
	public float mass;

	public string name;

	public bool visited;

	public Cluster cluster;
	public Cluster sCluster;

	public Planet (float xPos, float yPos, float radius, float mass, Cluster cluster, Cluster sCluster, string name = "Planet") {
		this.xPos = xPos;
		this.yPos = yPos;
		this.radius = radius;
		this.mass = mass;

		this.name = name;

		visited = false;
		this.cluster = cluster;
		this.sCluster = sCluster;
	}
	public Planet () {
		
	}
	public void SetVisited () {
		if (!visited) {
			cluster.visitedCount++;
			sCluster.visitedCount++;
			visited = true;
		}
	}

	public static float GetDist (Planet a, Planet b) {
		return UnityEngine.Mathf.Sqrt ((a.xPos - b.xPos) * (a.xPos - b.xPos) + (a.yPos - b.yPos) * (a.yPos - b.yPos));
	}
}
