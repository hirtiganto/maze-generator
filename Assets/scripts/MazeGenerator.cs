using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour {

    public int size = 10; 
    public GameObject floor;
    public GameObject wall;

    Cell[,] grid; // grid of cells

    // Stack is a place where we store indexes of cells which we've already visited
    List<int[]> stack;

    void Start () {
        stack = new List<int[]>();

        MakeGrid();
        MakeMaze();
        SetCells();
    }

    void MakeMaze () {
        // At start we set the current position to 0, 0 and keepMoving to true
        // keepMoving is false when there are no more unvisited cells
        int[] current = { 0, 0 };
        bool keepMoving = true;

        while (keepMoving) {
            // We mark the current cell as visited and check if it has unvisited
            // neighbours
            grid[current[0], current[1]].visited = true;
            List<int[]> neighbours = CheckNeighbours(current);

            if (neighbours.Count > 0) {

                // if the cell has unvisited neighbours we pick a random one,
                // add current cell to the stack, and set the current cell to
                // be the neighbour we just picked
                int[] next = neighbours[Random.Range(0, neighbours.Count)];
                stack.Add(current);
                RemoveWalls(current, next);
                current = next;
            } else if (stack.Count != 0) {

                // If there are no unvisited neighbours and the stack is not
                // empty we pick the last element from the stack list, set it as
                // the current position, and remove it from the stack
                int index = stack.Count - 1;
                current = stack[index];
                stack.RemoveAt(index);
            } else if (neighbours.Count == 0 && stack.Count == 0) {

                // If there are no neighbours and the stack is empty we set 
                // keepMoving as false because that means that all cells were
                // visited
                keepMoving = false;
            }

        }

    }

    void RemoveWalls (int[] current, int[] next) {
        // First we check if the wall we want to remove is on top, right,
        // bottom or left (current[] - next[]), then we set the top wall of the
        // current cell and the bottom of the next cell to false and so on
        if (current[1] - next[1] > 0) { // top
            grid[current[0], current[1]].walls[0] = false;
            grid[next[0], next[1]].walls[2] = false;
        } else if (current[0] - next[0] < 0) { // right
            grid[current[0], current[1]].walls[1] = false;
            grid[next[0], next[1]].walls[3] = false;
        } else if (current[1] - next[1] < 0) { // bottom
            grid[current[0], current[1]].walls[2] = false;
            grid[next[0], next[1]].walls[0] = false;
        } else if (current[0] - next[0] > 0) { // left
            grid[current[0], current[1]].walls[3] = false;
            grid[next[0], next[1]].walls[1] = false;
        }
    }

    List<int[]> CheckNeighbours (int[] current) {
        List<int[]> neighbours = new List<int[]>();

        // First we check whether there's a neighbour on a certain side of the
        // cell, then we check if it's already visited, and finally we add it
        // to the neighbours array

        if (current[1] - 1 >= 0) { // top
            if (!grid[current[0], current[1] - 1].visited) {
                neighbours.Add(new int[] { current[0], current[1] - 1 });
            }
        }
        if (current[0] + 1 < size) { // right
            if (!grid[current[0] + 1, current[1]].visited) {
                neighbours.Add(new int[] { current[0] + 1, current[1] });
            }
        }
        if (current[1] + 1 < size) { // bottom
            if (!grid[current[0], current[1] + 1].visited) {
                neighbours.Add(new int[] { current[0], current[1] + 1 });
            }
        }
        if (current[0] - 1 >= 0) { // left
            if (!grid[current[0] - 1, current[1]].visited) {
                neighbours.Add(new int[] { current[0] - 1, current[1] });
            }
        }

        return neighbours;
    }

    void MakeGrid () {
        // Here we make the grid and fill it with cells
        grid = new Cell[size, size];

        for (int x = 0; x < size; x++) {
            for (int y = 0; y < size; y++) {
                grid[x, y] = new Cell(new Vector3(x, 0f, -y));
            }
        }
    }

    void SetCells () {
        // offsets of the walls, because otherwise they would be instantiated
        // in the center of the cell
        Vector3 xoff = new Vector3(0.5f, 0.5f, 0f);
        Vector3 zoff = new Vector3(0f, 0.5f, 0.5f);

        for (int x = 0; x < size; x++) {
            for (int y = 0; y < size; y++) {

                // First we get the position of the cell
                Vector3 position = grid[x, y].position;

                // Then we instantiate the floor
                Instantiate(floor, position, Quaternion.identity);

                // Then we check which walls should be instantiated for this
                // cell, and we instantiate them
                if (grid[x, y].walls[0]) { // top

                    // if it's necesary we rotate the wall by 90 degrees
                    Instantiate(wall, position + zoff, Quaternion.Euler(0, 90, 0));
                }
                if (grid[x, y].walls[1]) { // right
                    Instantiate(wall, position + xoff, Quaternion.identity);
                }
                if (grid[x, y].walls[2]) { // bottom

                    // it it's necesary we change the offset
                    zoff.z *= -1;
                    Instantiate(wall, position + zoff, Quaternion.Euler(0, 90, 0));

                    // and then switch it back again
                    zoff.z *= -1;
                }
                if (grid[x, y].walls[3]) { // left
                    xoff.x *= -1;
                    Instantiate(wall, position + xoff, Quaternion.identity);
                    xoff.x *= -1;
                }
            }
        }
    }
}
