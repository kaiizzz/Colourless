using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image imageToHide;  // Assign in Inspector
    public Image imageToShow;  // Assign in Inspector

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (imageToHide != null) imageToHide.gameObject.SetActive(false);
        if (imageToShow != null) imageToShow.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (imageToHide != null) imageToHide.gameObject.SetActive(true);
        if (imageToShow != null) imageToShow.gameObject.SetActive(false);
    }
}
