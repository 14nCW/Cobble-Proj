using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour, ICharacterMovement
{
    [Header("References")]
    public CharacterInformation characterInformation;
    public SpriteRenderer spriteRenderer;

    [Header("Information")]
    public bool isMoving = false;

    [Header("Unity Events")]
    public UnityEvent OnLeaderMove;
    public UnityEvent OnFollowerMove;



    private List<Vector3> path;
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
            Vector3 targetPosition = (Vector3)(object)direction;

            moveFollowerTween = this.transform.DOMove(targetPosition, characterInformation.moveTime).OnComplete(() => OnFollowerMove?.Invoke());
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
                OnLeaderMove?.Invoke();
                GroupManager.Instance.MoveFollowers(this.transform.position);
                MoveLeader(index + 1);
            });
    }
}
