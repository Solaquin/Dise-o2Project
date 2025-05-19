using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIAudioFeedback : MonoBehaviour, IPointerEnterHandler
{
    public AudioSource audioSourceClick;
    public AudioSource audioSourceHover; // Opcional, si quieres hover

    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();

        // Suscribimos la función al evento onClick
        button.onClick.AddListener(PlayClickSound);
    }

    public void PlayClickSound()
    {
        if (audioSourceClick != null)
            audioSourceClick.Play();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (audioSourceHover != null)
            audioSourceHover.Play();
    }
}
