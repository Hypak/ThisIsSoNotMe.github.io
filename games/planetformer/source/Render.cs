using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Render : MonoBehaviour {

	public bool autoZoom = true;
	public float autoZoomSpeed = 1;
	public float autoZoomMult = 15;

	public float spriteScale = 1;

	public GameObject playerObj;
	public GameObject creatureObj;

	public GameObject planetObj;
	public GameObject visitedObj;

	public GameObject coinObj;

	public float mainView = 5;
	public float map1View = 35;
	public float map2View = 245;

	private Camera minimap1;
	private Camera minimap2;

	private int oldCoinCount = -1;

	public float defaultView = 800;
	float oldView = 0;
	int oldVisitedCount = 0;

	public float scrollSpeed = 1.01f;

	public GameObject player;
	List<GameObject> creatureObjs;

	List<GameObject> planetObjs;
	List<GameObject> coinObjs;

	public GameObject arrowObj;
	public Canvas canvas;

	void Start () {
		player = GameObject.Instantiate (playerObj);
		creatureObjs = new List<GameObject> ();

		planetObjs = new List<GameObject> ();
		coinObjs = new List<GameObject> ();

		minimap1 = GameObject.FindGameObjectWithTag ("Minimap1").GetComponent <Camera> ();
		minimap2 = GameObject.FindGameObjectWithTag ("Minimap2").GetComponent <Camera> ();
	}

	public void Rend (Player player, List<Creature> creatures, List<Planet> planets, List<Coin> coins) {
		Camera.main.orthographicSize = mainView;
		minimap1.orthographicSize = map1View;
		minimap2.orthographicSize = map2View;

		float pScale = 20 * player.radius / defaultView;

		this.player.transform.position = new Vector3 (player.xPos / defaultView, player.yPos / defaultView, 0);
		this.player.transform.localScale = new Vector3 (pScale, pScale, pScale);

		if (player.isColliding) {
			this.player.GetComponent <ParticleSystem>().Play ();
		}

		//Start creature
		int i = 0;

	/*
		foreach (Creature creature in creatures) {
			if (i < creatureObjs.Count) {
				creatureObjs [i].transform.position = new Vector3 (creature.xPos / defaultView, creature.yPos / defaultView, 0);
			} else {
				creatureObjs.Add (GameObject.Instantiate (
					creatureObj, new Vector3 (creature.xPos / defaultView, creature.yPos / defaultView, 0), Quaternion.identity
				));
				creatureObjs[i].name = "Creature";
			}
			float scale = creature.radius * 20 / defaultView;


			creatureObjs [i].transform.localScale = new Vector3 (scale, scale, scale);
			i++;
		}
		*/
		//Start planets
		//Only needs to be rendered if zoomed

		if (
			player.visitedCount != oldVisitedCount
			|| coins.Count != oldCoinCount
		) {
			oldVisitedCount = player.visitedCount;

			i = 0;
			foreach (Planet planet in planets) {
				if (i < planetObjs.Count) {
					if (planet.visited && planetObjs [i].name == "Planet") {
						GameObject.Destroy (planetObjs [i]);
						planetObjs [i] = GameObject.Instantiate (
							visitedObj, new Vector3 (planet.xPos / defaultView, planet.yPos / defaultView, 0), Quaternion.identity
						);
						planetObjs [i].name = "Visited Planet";
					} else {
						planetObjs [i].transform.position = new Vector3 (planet.xPos / defaultView, planet.yPos / defaultView, 0);
					}
				} else {
					planetObjs.Add (GameObject.Instantiate (
						planetObj, new Vector3 (planet.xPos / defaultView, planet.yPos / defaultView, 0), Quaternion.identity
					));
					planetObjs [i].name = "Planet";
				}
				float scale = planet.radius * 2 / defaultView;


				planetObjs [i].transform.localScale = new Vector3 (scale * spriteScale, scale * spriteScale, scale * spriteScale);
				i++;

			}
			for (; i < planetObjs.Count; i++) {
				planetObjs.RemoveAt (i);
			}
			//Start coins
			i = 0;
			foreach (Coin coin in coins) {
				if (i < coinObjs.Count) {				
					coinObjs [i].transform.position = new Vector3 (coin.xPos / defaultView, coin.yPos / defaultView, 0);
				} else {
					coinObjs.Add (GameObject.Instantiate (
						coinObj, new Vector3 (coin.xPos / defaultView, coin.yPos / defaultView, 0), Quaternion.identity
					));
					coinObjs [i].name = "Coin";
				}
				float scale = 80 / defaultView;
				coinObjs [i].transform.localScale = new Vector3 (scale * spriteScale, scale * spriteScale, scale * spriteScale);
				i++;
			}
			for (; i < coinObjs.Count; i++) {
				coinObjs.RemoveAt (i);
			}
		}

		foreach (Camera camera in Camera.allCameras) {
			camera.transform.position = new Vector3 (player.xPos / defaultView, player.yPos / defaultView, camera.transform.position.z);
		}

		float mult = Mathf.Pow (scrollSpeed, Input.mouseScrollDelta.y);
		mainView *= mult;
		map1View *= mult;
		map2View *= mult;

		oldCoinCount = coins.Count;
		float rate = 1 - Mathf.Pow (1 - autoZoomSpeed, Time.deltaTime * 45);
		if (autoZoom) {
			float newMult = (mainView * (1 - rate) + player.autoZoom * autoZoomMult * rate) / mainView;
			mainView *= newMult;
			map1View *= newMult;
			map2View *= newMult;
		}

		//RenderRadar (player);
	}

	public void RenderRadar (Player player) {
		foreach (GameObject arrow in GameObject.FindGameObjectsWithTag ("Frame")) {
			GameObject.Destroy (arrow);
		}

		foreach (GameObject obj in GameObject.FindGameObjectsWithTag ("Render")) {
			if (obj.name == "Planet" && PlanetIsOffScreen (obj) && GetPlanetDist (player, obj) < 1 * mainView) {
				float dir = Mathf.Atan2 (
					            player.yPos / defaultView - obj.transform.position.y,
					            player.xPos / defaultView - obj.transform.position.x

				) + Mathf.PI;
				RectTransform r = canvas.GetComponent <RectTransform> ();
				GameObject.Instantiate (arrowObj, new Vector3 (r.rect.width / 2 + Mathf.Sin (dir) * 150, r.rect.height / 2 + Mathf.Cos (dir) * 150, 0), Quaternion.identity, canvas.transform);
			}
		}
	}
	bool PlanetIsOffScreen (GameObject planet) {
		Vector3 viewPort = Camera.main.WorldToViewportPoint (planet.transform.position);
		return viewPort.x < 0 || viewPort.x > 1 || viewPort.y < 0 || viewPort.y > 1;
	}
	float GetPlanetDist (Player player, GameObject planet) {
		float playerX = player.xPos / defaultView;
		float playerY = player.yPos / defaultView;
		return Mathf.Sqrt (
			(playerX - planet.transform.position.x) * (playerX - planet.transform.position.x) 
			+ (playerY - planet.transform.position.x) * (playerY - planet.transform.position.y) 
		);
	}
}
