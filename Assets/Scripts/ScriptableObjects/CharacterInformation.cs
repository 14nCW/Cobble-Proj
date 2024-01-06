using UnityEngine;

[CreateAssetMenu(fileName = "CharacterInformation", menuName = "Charater/NewCharacter")]
public class CharacterInformation : ScriptableObject
{
    public string characterName;
    public float moveTime;
    public float agility;
    public float endurance;

    private void OnEnable()
    {
        moveTime = Random.Range(0.2f, 0.7f);
        agility = Random.Range(1f, 10f);
        endurance = Random.Range(1f, 10f);
    }
}
