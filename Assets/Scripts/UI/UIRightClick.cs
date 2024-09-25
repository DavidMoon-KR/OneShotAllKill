using UnityEngine;
using UnityEngine.EventSystems;

public class UIRightClick : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Tutorial1.Instance.OnClickWall();
            Destroy(this.gameObject);
        }
    }
}
