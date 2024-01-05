using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathfindingAlgorithm : MonoBehaviour
{
    public Tilemap walkableTilemap;
    public GenericDictionary<tileDirections, Vector3> directionVectors;

    List<AStarNode> openSet = new List<AStarNode>();
    HashSet<AStarNode> closedSet = new HashSet<AStarNode>();
    List<Vector3> path = new List<Vector3>();

    public class AStarNode
    {
        public TopologyTile tile;
        public int F;
        public int G;
        public int H;
        public Vector3 position;
        public AStarNode parent;
    }

    public List<Vector3> FindPath(Vector3 start, Vector3 end)
    {
        openSet.Clear();
        closedSet.Clear();
        path.Clear();

        Vector3 startWorldPos = new Vector3(start.x, start.y, 0);
        Vector3 endWorldPos = new Vector3(end.x, end.y, 0);

        Vector3Int startCellPos = walkableTilemap.WorldToCell(startWorldPos);
        Vector3Int endCellPos = walkableTilemap.WorldToCell(endWorldPos);

        TopologyTile startTile = walkableTilemap.GetTile<TopologyTile>(startCellPos);
        TopologyTile endTile = walkableTilemap.GetTile<TopologyTile>(endCellPos);

        AStarNode startNode = new AStarNode
        {
            tile = startTile,
            G = 0,
            H = CalculateHeuristic(startWorldPos, endWorldPos),
            F = CalculateHeuristic(startWorldPos, endWorldPos),
            position = startWorldPos,
            parent = null
        };

        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            AStarNode currentNode = GetLowestF(openSet);
            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode.tile == endTile)
            {
                ReconstructPath(currentNode);
                Debug.Log("Path found!");
                return path;
            }

            FindNeighbours(currentNode.tile, currentNode.position, currentNode);
        }

        Debug.Log("No path found!");
        return path;
    }

    private void ReconstructPath(AStarNode node)
    {
        while (node != null)
        {
            path.Add(node.position);
            node = node.parent;
        }

        path.Reverse();
    }

    private void FindNeighbours(TopologyTile tile, Vector3 tilePosition, AStarNode parent)
    {
        foreach (tileDirections direction in tile.directions)
        {
            Vector3 neighbourPosition = tilePosition + directionVectors[direction];
            Vector3Int neighbourCellPos = walkableTilemap.WorldToCell(neighbourPosition);

            TopologyTile neighbour = walkableTilemap.GetTile<TopologyTile>(neighbourCellPos);

            if (neighbour != null)
            {
                AStarNode neighbourNode = new AStarNode
                {
                    tile = neighbour,
                    G = parent.G + 1,
                    H = CalculateHeuristic(neighbourPosition, parent.position),
                    F = parent.G + 1 + CalculateHeuristic(neighbourPosition, parent.position),
                    position = neighbourPosition,
                    parent = parent
                };

                if (!openSet.Contains(neighbourNode))
                {
                    openSet.Add(neighbourNode);
                }
            }
        }
    }

    private int CalculateHeuristic(Vector3 start, Vector3 end)
    {
        return Mathf.FloorToInt(Vector3.Distance(start, end));
    }

    private AStarNode GetLowestF(List<AStarNode> list)
    {
        AStarNode lowest = list[0];
        foreach (AStarNode node in list)
        {
            if (node.F < lowest.F)
            {
                lowest = node;
            }
        }
        return lowest;
    }
}
