using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour, ICharacterMovement
{
    public CharacterInformation characterInformation;
    private List<Vector3> path;
    public SpriteRenderer spriteRenderer;


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
            path = direction as List<Vector3>;
            int index = 0;
            MoveLeader(index);
        }
        else
        {
            if (moveFollowerTween != null) moveFollowerTween.Kill();
            Vector3 targetPosition = (Vector3)(object)direction; // Convert to Vector3

            moveFollowerTween = this.transform.DOMove(targetPosition, characterInformation.speed);
        }
    }

    private void MoveLeader(int index)
    {
        if (index >= path.Count) return;

        if (moveLeaderTween != null) moveLeaderTween.Kill();

        moveLeaderTween = this.transform.DOMove(path[index], characterInformation.speed).OnComplete(() =>
            {
                GroupManager.Instance.MoveFollowers(this.transform.position);
                MoveLeader(index + 1);
            });
    }
}
