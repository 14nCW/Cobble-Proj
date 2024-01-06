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
    private List<GameObject> gameObjects = new List<GameObject>();

    private void Awake()
    {
        inputSystem = new InputSystem();
    }

    private void OnEnable()
    {
        inputSystem.Enable();
        inputSystem.Gameplay.Pointer.performed += OnLeftClick;
    }

    private void OnDisable()
    {
        inputSystem.Disable();
    }


    private void OnLeftClick(InputAction.CallbackContext result)
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(inputSystem.Gameplay.Position.ReadValue<Vector2>()), Vector2.zero);

        if (hit == false) return;

        if (hit.collider.tag.Equals("Walkable"))
        {
            Vector3 endTilePosition = walkableTilemap.GetCellCenterWorld(walkableTilemap.WorldToCell(hit.point));
            Vector3 startTilePosition = walkableTilemap.GetCellCenterWorld(walkableTilemap.WorldToCell(GroupManager.Instance.leader.transform.position));
            List<Vector3> path = AStar.FindPath(startTilePosition, endTilePosition);

            GroupManager.Instance.MoveLeader(path);
        }
    }
}
