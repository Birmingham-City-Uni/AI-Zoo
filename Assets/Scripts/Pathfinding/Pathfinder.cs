using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pathfinder : MonoBehaviour
{

	public Transform seeker, target;
	Grid grid;

	void Awake()
	{
		grid = GetComponent<Grid>();
	}

	void Update()
	{
		FindPath(seeker.position, target.position);
	}

	void FindPath(Vector3 startPos, Vector3 targetPos)
	{
		// Position of seeker
		Node startNode = grid.NodeFromWorldPoint(startPos);
		// Position of target
		Node targetNode = grid.NodeFromWorldPoint(targetPos);

		List<Node> openSet = new List<Node>();
		HashSet<Node> closedSet = new HashSet<Node>();
		openSet.Add(startNode);

		while (openSet.Count > 0)
		{
			Node node = openSet[0];
			for (int i = 1; i < openSet.Count; i++)
			{
				if (openSet[i].fCost < node.fCost || openSet[i].fCost == node.fCost)
				{
					if (openSet[i].hCost < node.hCost)
<<<<<<< HEAD
                    {
						node = openSet[i];
					}
=======
						node = openSet[i];
>>>>>>> dda8a47132e06d8a493ad125e28bd2a4ab5bb60b
				}
			}

			openSet.Remove(node);
			closedSet.Add(node);

			if (node == targetNode)
			{
				RetracePath(startNode, targetNode);
				return;
			}

			foreach (Node neighbour in grid.GetNeighbours(node))
			{
				if (!neighbour.walkable || closedSet.Contains(neighbour))
				{
					continue;
				}

				int newCostToNeighbour = node.gCost + GetDistance(node, neighbour);
				if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
				{
					neighbour.gCost = newCostToNeighbour;
					neighbour.hCost = GetDistance(neighbour, targetNode);
					neighbour.parent = node;

					if (!openSet.Contains(neighbour))
						openSet.Add(neighbour);
				}
			}
		}
	}

	void RetracePath(Node startNode, Node endNode)
	{
		List<Node> path = new List<Node>();
		Node currentNode = endNode;

		while (currentNode != startNode)
		{
			path.Add(currentNode);
			currentNode = currentNode.parent;
		}
		path.Reverse();
<<<<<<< HEAD
		grid.path = path;
=======

		grid.path = path;

>>>>>>> dda8a47132e06d8a493ad125e28bd2a4ab5bb60b
	}

	int GetDistance(Node nodeA, Node nodeB)
	{
		int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
		int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
		int dstZ = Mathf.Abs(nodeA.gridZ - nodeB.gridZ);

		// X is greatest
		if (dstX > dstY && dstX > dstZ)
<<<<<<< HEAD
		{
			// Y is second greatest
			if (dstY > dstZ)
			{
				return 17 * dstZ + 14 * (dstY - dstZ) + 10 * (dstX - dstY);
			}
			// Z is second greatest
			else
			{
=======
        {
			// Y is second greatest
			if (dstY > dstZ)
            {
				return 17 * dstZ + 14 * (dstY - dstZ) + 10 * (dstX - dstY);
			}
			// Z is second greatest
            else
            {
>>>>>>> dda8a47132e06d8a493ad125e28bd2a4ab5bb60b
				return 17 * dstY + 14 * (dstZ - dstY) + 10 * (dstX - dstZ);
			}
		}
		// Y is greatest
		else if (dstY > dstX && dstY > dstZ)
		{
			// X is second greatest
			if (dstX > dstZ)
			{
				return 17 * dstZ + 14 * (dstX - dstZ) + 10 * (dstY - dstX);
			}
			// Z is second greatest
			else
			{
				return 17 * dstX + 14 * (dstZ - dstX) + 10 * (dstY - dstZ);
			}
		}
		// Z is greatest
		else
		{
			// X is second greatest
			if (dstX > dstY)
			{
				return 17 * dstY + 14 * (dstX - dstY) + 10 * (dstZ - dstX);
			}
			// Y is second greatest
			else
			{
				return 17 * dstX + 14 * (dstY - dstX) + 10 * (dstZ - dstY);
			}
		}
	}

	void OnDrawGizmos()
	{
		if (grid != null)
<<<<<<< HEAD
		{
=======
        {
>>>>>>> dda8a47132e06d8a493ad125e28bd2a4ab5bb60b
			Gizmos.color = Color.cyan;
			Node start = grid.NodeFromWorldPoint(seeker.position);
			Gizmos.DrawCube(start.worldPosition, Vector3.one * (grid.nodeRadius * 2 - .1f));
			Node end = grid.NodeFromWorldPoint(target.position);
			Gizmos.color = Color.cyan;
			Gizmos.DrawCube(end.worldPosition, Vector3.one * (grid.nodeRadius * 2 - .1f));
		}
	}
}
