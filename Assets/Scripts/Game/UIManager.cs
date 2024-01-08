using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviourSingleton<UIManager>
{
    [Header("References")]
    public List<ButtonController> buttons;

    [Header("Colors")]
    public Color activatedButtonColor;
    public Color desactivatedButtonColor;

    private void Start()
    {
        GroupManager groupManager = GroupManager.Instance;

        int i = 0;

        foreach (Character c in groupManager.characters)
        {
            buttons[i].character = c;
            buttons[i].SetButton();
            i++;
        }

        ButtonController leaderButton = buttons.Where(entry => entry.character == groupManager.leader).First();

        leaderButton.GetComponent<Image>().color = activatedButtonColor;
    }

    public void ChangeButtonsColor(Image buttonToChange)
    {
        if (GroupManager.Instance.leader.isMoving) return;

        foreach (ButtonController b in buttons)
        {
            b.GetComponent<Image>().color = desactivatedButtonColor;
        }
        buttonToChange.color = activatedButtonColor;
    }
}
