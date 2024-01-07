using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public Character character;
    public TextMeshProUGUI buttonName;

    private void Awake()
    {
        UIManager.Instance.buttons.Add(this);
    }

    public void SetButton()
    {
        buttonName.SetText(character.characterInformation.characterName);
    }

    public void SetLeader()
    {
        UIManager.Instance.ChangeButtonsColor(this.GetComponent<Image>());

        GroupManager.Instance.ChangeLeader(character);
    }
}
