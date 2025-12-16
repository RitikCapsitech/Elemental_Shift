using UnityEngine;

public class ElementButton : MonoBehaviour
{
    public PlayerController player;
    public ElementType type;

    public void OnClickElement()
    {
        player.SwitchElement(type);
    }
}
