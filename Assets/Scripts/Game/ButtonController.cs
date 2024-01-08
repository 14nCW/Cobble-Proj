using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    [Header("References")]
    public TextMeshProUGUI buttonName;
    public Character character;

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
