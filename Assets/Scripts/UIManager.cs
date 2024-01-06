using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public List<ButtonController> buttons;

    private void Start()
    {
        int i = 0;

        foreach (Character c in GroupManager.Instance.characters)
        {
            buttons[i].character = c;
            buttons[i].SetButton();
            i++;
        }
    }
}
