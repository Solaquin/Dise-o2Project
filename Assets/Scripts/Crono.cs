using UnityEngine;
using TMPro;

public class Crono : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private float time;
    [SerializeField] private Canvas failCanva;
    [HideInInspector] public float totalTime;

    private bool failedLevel = false;
    private bool timeStopped = false;
    public bool FailedLevel { get; set; }
    private int timerMinutes, timerSeconds;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        totalTime = time;
    }
    void Start()
    {
        failCanva.gameObject.SetActive(false);
    }
    void Cronometro()
    {
        if (timeStopped == true) return;

        time -= Time.deltaTime;

        timerMinutes = Mathf.FloorToInt(time / 60);
        timerSeconds = Mathf.FloorToInt(time % 60);

        timerText.text = string.Format("{0:00}:{1:00}", timerMinutes, timerSeconds);
    }

    public string parseTimer(float time)
    {
        timerMinutes = Mathf.FloorToInt(time / 60);
        timerSeconds = Mathf.FloorToInt(time % 60);

        return string.Format("{0:00}:{1:00}", timerMinutes, timerSeconds);
    }

    public float getActualTime()
    {
        return time;
    }

    public void stopTime()
    {
        timeStopped = true;
    }
    // Update is called once per frame
    void Update()
    {
        Cronometro();
        if (time <= 0 && !FailedLevel)
        {
            timeStopped = true;
            FailedLevel = true;
            Time.timeScale = 0f;
            failCanva.gameObject.SetActive(true);
        }
    }
}
