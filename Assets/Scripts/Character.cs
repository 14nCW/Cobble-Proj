using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour, ICharacterMovement
{
    public CharacterInformation characterInformation;
    private List<Vector3> path;


    private Tween moveLeaderTween;
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

        }
    }

    private void MoveLeader(int index)
    {
        if (index >= path.Count) return;

        if (moveLeaderTween != null) moveLeaderTween.Kill();

        moveLeaderTween = this.transform.DOMove(path[index], characterInformation.speed).OnComplete(() => MoveLeader(index + 1));
    }
}
