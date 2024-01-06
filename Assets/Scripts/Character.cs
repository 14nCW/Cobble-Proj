using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour, ICharacterMovement
{
    public CharacterInformation characterInformation;
    private List<Vector3> path;
    public SpriteRenderer spriteRenderer;
    private float baseMoveSpeed = 1;
    public bool isMoving = false;

    private Tween moveLeaderTween;
    private Tween moveFollowerTween;
    private void Awake()
    {
        GroupManager.Instance.characters.Add(this);
    }

    public void MoveTo<T>(T direction)
    {
        if (this == GroupManager.Instance.leader)
        {
            isMoving = true;
            path = direction as List<Vector3>;
            int index = 0;
            MoveLeader(index);
        }
        else
        {
            if (moveFollowerTween != null) moveFollowerTween.Kill();
            Vector3 targetPosition = (Vector3)(object)direction; // Convert to Vector3

            moveFollowerTween = this.transform.DOMove(targetPosition, characterInformation.moveTime);
        }
    }

    private void MoveLeader(int index)
    {
        if (index >= path.Count)
        {
            isMoving = false;
            return;
        }

        if (moveLeaderTween != null) moveLeaderTween.Kill();

        moveLeaderTween = this.transform.DOMove(path[index], characterInformation.moveTime).OnComplete(() =>
            {
                GroupManager.Instance.MoveFollowers(this.transform.position);
                MoveLeader(index + 1);
            });
    }
}
