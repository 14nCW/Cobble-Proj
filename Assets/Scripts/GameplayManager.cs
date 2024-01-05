using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class GameplayManager : MonoBehaviourSingleton<GameplayManager>
{
    public PathfindingAlgorithm AStar;
    private InputSystem inputSystem;

    public Tilemap walkableTilemap;
    public GameObject test;

    private void Awake()
    {
        inputSystem = new InputSystem();
    }

    private void OnEnable()
    {
        inputSystem.Enable();
        inputSystem.Gameplay.Mouse.performed += OnLeftClick;
    }

    private void OnDisable()
    {
        inputSystem.Disable();
    }


    private void OnLeftClick(InputAction.CallbackContext result)
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()), Vector2.zero);

        if (hit.collider.tag.Equals("Walkable"))
        {
            TopologyTile topologyTile = walkableTilemap.GetTile<TopologyTile>(walkableTilemap.WorldToCell(hit.point));
            Vector3 tilePosition = walkableTilemap.GetCellCenterWorld(walkableTilemap.WorldToCell(hit.point));

            List<Vector3> path = AStar.FindPath(GroupManager.Instance.leader.transform.position, tilePosition);

            foreach (Vector3 node in path)
            {
                Instantiate(test, node, Quaternion.identity);
            }
        }
    }
}
