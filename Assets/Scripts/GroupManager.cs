using System.Collections.Generic;
using UnityEngine;

public class GroupManager : MonoBehaviourSingleton<GroupManager>
{
    public List<Character> characters;

    [Header("Sprites")]
    public Sprite leaderSprite;
    public Sprite followerSprite;

    public Character leader { get; private set; }

    private void Start()
    {
        leader = characters[Random.Range(0, characters.Count)];
        leader.GetComponent<SpriteRenderer>().sprite = leaderSprite;
    }

    public void ChangeLeader(Character character)
    {
        leader = character;

        foreach (Character c in characters)
        {
            c.GetComponent<SpriteRenderer>().sprite = (c.gameObject != character) ? followerSprite : leaderSprite;
        }
    }

    public void MoveLeader(List<Vector3> path)
    {
        leader.MoveTo(path);
    }

    public void MoveFollowers(Vector3 direction)
    {

    }
}
