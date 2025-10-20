using TMPro;
using UnityEngine;

public class TimeDisplay : MonoBehaviour
{
    [SerializeField] TMP_Text dayText;
    [SerializeField] TMP_Text tickText;
    [SerializeField] TMP_Text speedText;

    public static TimeDisplay instance;

    private void Awake()
    {
        instance = this;
        GameTick.onDay += () =>
        {
            dayText.text = $"Day {(int)GameTick.instance.GetDays()}";
        };
        GameTick.onTick += () =>
        {
            tickText.text = $"Tick {(int)GameTick.instance.ticks}";
        };
    }

    public void ShowSpeed(GameSpeed speed)
    {
        speedText.text = speed.ToString();
    }
}