using TMPro;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public Character character;
    public TextMeshProUGUI buttonName;

    public void SetButton()
    {
        buttonName.SetText(character.characterInformation.characterName);
    }

    public void OnClick()
    {
        GroupManager.Instance.ChangeLeader(character);
    }

    public void SetLeader()
    {
        GroupManager.Instance.ChangeLeader(character);
    }
}
