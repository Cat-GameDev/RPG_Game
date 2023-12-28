using UnityEngine;

public class UI_Pause : UICanvas
{
    public GameObject[] menu;
    public void SwitchTo(GameObject _menu)
    {
        for(int i =0; i< menu.Length; i++)
        {
            menu[i].SetActive(false);
        }

        if(_menu != null)
        {
            _menu.SetActive(true);
        }
    }
}