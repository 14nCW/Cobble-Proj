using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public Character character;

    public void OnClick()
    {
        GroupManager.Instance.ChangeLeader(character);
    }
}
