using System.Collections.Generic;
using UnityEngine;

public class TabGroup : MonoBehaviour
{
    public List<BookTabButton> tabButtons;

    public void OnTabSelected(BookTabButton selected)
    {
        foreach (var tab in tabButtons)
        {
            bool isActive = (tab == selected);
            tab.pageToShow.SetActive(isActive);
            tab.SetActiveVisual(isActive);
        }
    }
}