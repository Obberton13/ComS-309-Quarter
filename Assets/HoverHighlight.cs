using UnityEngine;
using System.Collections;

public class HoverHighlight : MonoBehaviour {

    [SerializeField]
    private GameObject manager;

    void OnMouseEnter()
    {
        Debug.Log("This is a message from mouseEnter");
        manager.GetComponent<MenuManager>().mousedOver = true;
        manager.GetComponent<MenuManager>().selected = MenuManager.Buttons.newGame;
    }
}
