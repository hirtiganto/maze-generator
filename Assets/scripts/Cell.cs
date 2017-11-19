using UnityEngine;

public class Cell {

    // In this class we store the informations about the cell - it's walls,
    // position, etc.

    public Vector3 position;
    public bool[] walls; // top, right, bottom, left
    public bool visited = false; // by default the cell is unvisited

    public Cell (Vector3 pos) {
        position = pos;

        // By default top and right walls are disabled so they won't overlap
        // the bottom and right walls of other cells
        walls = new bool[4] { false, true, true, false };

        // If the cell is on the top or the left border of the maze we set 
        // the top or left wall to be true so the player won't fall off
        if (position.z < 0.5f) {
            walls[0] = true;
        }

        if (position.x < 0.5f) {
            walls[3] = true;
        }
    }
}
