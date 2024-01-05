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
        inputSystem.Gameplay.Mouse.performed += OnLeftClick;
    }

    private void OnDisable()
    {
        inputSystem.Disable();
    }


    private void OnLeftClick(InputAction.CallbackContext result)
    {
        if (gameObjects.Count > 0)
        {
            foreach (GameObject obj in gameObjects)
            {
                Destroy(obj);
            }
            gameObjects.Clear();
        }


        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()), Vector2.zero);

        if (hit.collider.tag.Equals("Walkable"))
        {
            //TopologyTile endTile = walkableTilemap.GetTile<TopologyTile>(walkableTilemap.WorldToCell(hit.point));

            Vector3 endTilePosition = walkableTilemap.GetCellCenterWorld(walkableTilemap.WorldToCell(hit.point));
            Vector3 startTilePosition = walkableTilemap.GetCellCenterWorld(walkableTilemap.WorldToCell(GroupManager.Instance.leader.transform.position));
            Debug.Log(endTilePosition);
            Debug.Log(startTilePosition);
            List<Vector3> path = AStar.FindPath(startTilePosition, endTilePosition);

            foreach (Vector3 node in path)
            {
                Debug.Log(node);
                GameObject obj = Instantiate(test, node, Quaternion.identity);
                gameObjects.Add(obj);
            }
        }
    }
}
