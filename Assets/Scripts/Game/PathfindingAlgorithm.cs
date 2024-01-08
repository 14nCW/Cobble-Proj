using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class PathfindingAlgorithm : MonoBehaviour
{
    [Header("References")]
    public Tilemap walkableTilemap;

    [Header("Configuration")]
    public GenericDictionary<tileDirections, Vector3> directionVectors;
    public float appriximity = 0.001f;

    [Header("Information")]
    public List<Vector3> path = new List<Vector3>();

    [Header("Unity Events")]
    public UnityEvent OnPathFound;
    public UnityEvent OnPathNotFound;

    List<AStarNode> closedSet = new List<AStarNode>();
    List<AStarNode> openSet = new List<AStarNode>();
    private Vector3 endWorldPos;

    public class AStarNode
    {
        public TopologyTile tile;
        public float F;
        public float G;
        public float H;
        public Vector3 position;
        public AStarNode parent;
    }

    public List<Vector3> FindPath(Vector3 start, Vector3 end)
    {
        openSet.Clear();
        closedSet.Clear();
        path.Clear();

        AStarNode finalNode = new AStarNode { };

        Vector3 startWorldPos = start;
        Vector3 endWorldPos = end;

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

            if (CheckIfIsCloseEnough(currentNode.position, endWorldPos))
            {
                ReconstructPath(currentNode);
                Debug.Log("Path found!");
                OnPathFound?.Invoke();
                return path;
            }

            FindNeighbours(currentNode.tile, currentNode.position, currentNode);
        }

        Debug.Log("No path found!");
        OnPathNotFound?.Invoke();
        return path;
    }

    private bool CheckIfIsCloseEnough(Vector3 position, Vector3 end)
    {
        if (Mathf.Abs(position.x - end.x) < appriximity && Mathf.Abs(position.y - end.y) < appriximity)
        {
            return true;
        }
        return false;
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

            if (neighbour != null && !closedSet.Exists(node => node.position == neighbourPosition))
            {
                AStarNode neighbourNode = new AStarNode
                {
                    tile = neighbour,
                    G = parent.G + 0.5197f,
                    H = CalculateHeuristic(neighbourPosition, endWorldPos),
                    F = parent.G + 0.5197f + CalculateHeuristic(neighbourPosition, parent.position),
                    position = neighbourPosition,
                    parent = parent
                };

                if (!openSet.Exists(node => node.position == neighbourNode.position))
                {
                    openSet.Add(neighbourNode);
                }
            }
        }
    }

    private float CalculateHeuristic(Vector3 start, Vector3 end)
    {
        return Vector3.Distance(start, end);
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
