using System.Collections.Generic;
using UnityEngine;

public class TabGroup : MonoBehaviour
{
    // список всех вкладок, висит на пустом отдельном объекте
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