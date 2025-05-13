using UnityEngine;
using TMPro;

public class Crono : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private float time;
    [SerializeField] private Canvas failCanva;

    private bool failedLevel = false;
    public bool FailedLevel { get; set; }
    private int timerMinutes, timerSeconds;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        failCanva.gameObject.SetActive(false);
    }
    void Cronometro()
    {
        time -= Time.deltaTime;

        timerMinutes = Mathf.FloorToInt(time / 60);
        timerSeconds = Mathf.FloorToInt(time % 60);

        timerText.text = string.Format("{0:00}:{1:00}", timerMinutes, timerSeconds);
    }
    // Update is called once per frame
    void Update()
    {
        Cronometro();
        if (time <= 0 && !FailedLevel)
        {
            FailedLevel = true;
            Time.timeScale = 0f;
            failCanva.gameObject.SetActive(true);
        }
    }
}
