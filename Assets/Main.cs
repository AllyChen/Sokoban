using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {

	public Spot spotPrefab;
	public GameObject spotParrent;

	// The map consists of many spot.
	public List<List<Spot>> map;
	private int mapSize;
	// The position of box1.
	private int box1_x, box1_z;
	// The next position of box1.
	private int box1_next_x, box1_next_z;
	// The position of player.
	private int player_x, player_z;
	// The next position of player.
	private int player_next_x, player_next_z;
	// The position of point1.
	private int point1_x, point1_z;
	// The position of point2.
	private int point2_x, point2_z;

	// Use this for initialization
	void Start () {
		// Initial
		mapSize = 10;
		//boxPos_1 = new Vector3(2, 2, 2);
		box1_x = 2;
		box1_z = 2;
		box1_next_x = box1_x;
		box1_next_z = box1_z;
		player_x = 1;
		player_z = 1;
		player_next_x = player_x;
		player_next_z = player_z;
		point1_x = 5;
		point1_z = 5;
		point2_x = 5;
		point2_z = 6;

		// Create new map.
		map = new List<List<Spot>>();
		for (int x = 0; x < mapSize; x++) {
			map.Add(new List<Spot>());
			for (int z = 0; z < mapSize; z++) {
				// Clone the spots.
				Spot spotChild = GameObject.Instantiate<Spot>(spotPrefab);
				spotChild.transform.SetParent(spotParrent.transform);
				spotChild.transform.position = new Vector3(x, 0, z);
				map[x].Add(spotChild);
			}			
		}
		// Set the spot be the wall.
		for(int i = 0; i < mapSize; i++) {
			map[i][0].spotType = Spot.SPOT_TYPE.WALL;
			map[i][mapSize - 1].spotType = Spot.SPOT_TYPE.WALL;
		}
		for (int j = 1; j < mapSize - 1; j++) {
			map[0][j].spotType = Spot.SPOT_TYPE.WALL;
			map[mapSize - 1][j].spotType = Spot.SPOT_TYPE.WALL;
		}
		// Set the spot be the start point.
		map[player_x][player_z].spotType = Spot.SPOT_TYPE.START;

		// Set the spot be the box point.
		map[point1_x][point1_z].spotType = Spot.SPOT_TYPE.POINT;
		map[point2_x][point2_z].spotType = Spot.SPOT_TYPE.POINT;

		// Set the spot be the box initial point.
		map[box1_x][box1_z].spotType = Spot.SPOT_TYPE.BOXOFF;
	}

	// Update is called once per frame
	void Update() {

		// When the player move...
		if (Input.GetKeyDown(KeyCode.UpArrow)) {
			player_next_z = player_z + 1;

		} else if (Input.GetKeyDown(KeyCode.DownArrow)) {
			player_next_z = player_z - 1;

		} else if (Input.GetKeyDown(KeyCode.RightArrow)) {
			player_next_x = player_x + 1;

		} else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
			player_next_x = player_x - 1;

		}

		// Player can move to there or not.
		if (map[player_next_x][player_next_z].spotType != Spot.SPOT_TYPE.WALL &&
			map[player_next_x][player_next_z].spotType != Spot.SPOT_TYPE.BOXOFF &&
			map[player_next_x][player_next_z].spotType != Spot.SPOT_TYPE.BOXON) {
			map[player_x][player_z].spotType = Spot.SPOT_TYPE.FLOOR;
			player_x = player_next_x;
			player_z = player_next_z;

		// Move the box.
		} else if (map[player_next_x][player_next_z].spotType == Spot.SPOT_TYPE.BOXOFF ||
					map[player_next_x][player_next_z].spotType == Spot.SPOT_TYPE.BOXON) {
			// box
			box1_next_x = box1_x + (player_next_x - player_x);
			box1_next_z = box1_z + (player_next_z - player_z);

			if (map[box1_next_x][box1_next_z].spotType != Spot.SPOT_TYPE.WALL &&
				map[box1_next_x][box1_next_z].spotType != Spot.SPOT_TYPE.POINT) {
				box1_x = box1_next_x;
				box1_z = box1_next_z;
				map[box1_x][box1_z].spotType = Spot.SPOT_TYPE.BOXOFF;

				// player
				map[player_x][player_z].spotType = Spot.SPOT_TYPE.FLOOR;
				player_x = player_next_x;
				player_z = player_next_z;

			} else if (map[box1_next_x][box1_next_z].spotType == Spot.SPOT_TYPE.POINT) {
				box1_x = box1_next_x;
				box1_z = box1_next_z;
				map[box1_x][box1_z].spotType = Spot.SPOT_TYPE.BOXON;

				// player
				map[player_x][player_z].spotType = Spot.SPOT_TYPE.FLOOR;
				player_x = player_next_x;
				player_z = player_next_z;

			} else {
				box1_next_x = box1_x;
				box1_next_z = box1_z;
			}
		}
		
		// Show the point.
		if(map[point1_x][point1_z].spotType == Spot.SPOT_TYPE.FLOOR) {
			map[point1_x][point1_z].spotType = Spot.SPOT_TYPE.POINT;

		} else if (map[point2_x][point2_z].spotType == Spot.SPOT_TYPE.FLOOR) {
			map[point2_x][point2_z].spotType = Spot.SPOT_TYPE.POINT;
		}

		player_next_x = player_x;
		player_next_z = player_z;
		map[player_x][player_z].spotType = Spot.SPOT_TYPE.START;
	}
}
