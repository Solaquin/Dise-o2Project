using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Mouse enter: " + gameObject.name);
        animator.SetBool("Hover", true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        animator.SetBool("Hover", false);
    }
}