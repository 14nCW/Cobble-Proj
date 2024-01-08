using Cinemachine;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GroupManager : MonoBehaviourSingleton<GroupManager>
{
    [Header("References")]
    [HideInInspector] public List<Character> characters;
    public CinemachineVirtualCamera virtualCamera;
    public Character leader { get; private set; }

    [Header("Configuration")]
    public float radiusAroundLeader = 0.15f;

    [Header("Sprites")]
    public Sprite leaderSprite;
    public Sprite followerSprite;

    [Header("Unity Events")]
    public UnityEvent OnLeaderChange;

    private void Start()
    {
        leader = characters[Random.Range(0, characters.Count)];
        leader.spriteRenderer.sprite = leaderSprite;
        virtualCamera.Follow = leader.transform;
    }

    public void ChangeLeader(Character character)
    {
        if (leader.isMoving)
        {
            Debug.Log("Leader is moving. Can't nominate a new one");
            return;
        }

        leader = character;

        foreach (Character c in characters)
        {
            c.spriteRenderer.sprite = (c.gameObject != character) ? followerSprite : leaderSprite;
        }

        leader.spriteRenderer.sprite = leaderSprite;
        virtualCamera.Follow = leader.transform;

        OnLeaderChange?.Invoke();
    }

    public void MoveLeader(List<Vector3> path)
    {
        leader.MoveTo(path);
    }

    public void MoveFollowers(Vector3 leaderPosision)
    {
        List<Vector3> targetPositionList = GetPossisionAroundLeader(leaderPosision, radiusAroundLeader);
        int targetPositionIndex = 0;

        foreach (Character c in characters)
        {
            if (c != leader)
            {
                c.MoveTo(targetPositionList[targetPositionIndex]);
                targetPositionIndex++;
            }
        }
    }

    private List<Vector3> GetPossisionAroundLeader(Vector3 leaderPosition, float distance)
    {
        List<Vector3> positionList = new List<Vector3>();

        int numberOfFollowers = characters.Count - 1;

        for (int i = 0; i < numberOfFollowers; i++)
        {
            float angle = i * (360f / numberOfFollowers);

            Vector3 dir = ApplyRotationToVector(new Vector3(1, 0), angle);
            Vector3 position = leaderPosition + dir * distance;
            positionList.Add(position);
        }

        return positionList;
    }

    private Vector3 ApplyRotationToVector(Vector3 vector, float angle)
    {
        return Quaternion.Euler(0, 0, angle) * vector;
    }
}
