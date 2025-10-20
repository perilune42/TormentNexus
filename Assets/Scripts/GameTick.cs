using JetBrains.Annotations;
using System;
using UnityEngine;

public enum GameSpeed
{
    Paused, Normal, Fast, Faster
}

// main update loop
public class GameTick : MonoBehaviour
{
    public static GameTick instance;

    const float baseTPS = 20; // ticks per second
    float timePerTick = 1 / baseTPS;

    static float[] speedPresets = new float[] { 0, 1, 3, 20 };
    GameSpeed currSpeed = GameSpeed.Normal;
    GameSpeed savedSpeed = GameSpeed.Normal;

    public const float ticksPerDay = 20;

    private float timeToNextTick;


    public int ticks;
    public static Action onTick;
    public static Action onDay;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetSpeed(GameSpeed.Normal);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetSpeed(GameSpeed.Fast);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetSpeed(GameSpeed.Faster);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TogglePause();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameUI.instance.ToggleTechTree();
        }


        if (currSpeed != GameSpeed.Paused) timeToNextTick -= Time.deltaTime * speedPresets[(int)currSpeed];
        while (timeToNextTick < 0)
        {
            Tick();
            timeToNextTick += timePerTick;
        }
    }

    public void Tick()
    {
        ticks++;
        onTick?.Invoke();
        if (ticks % ticksPerDay == 0)
        {
            onDay?.Invoke();
        }
    }

    public void SetSpeed(GameSpeed speed)
    {
        currSpeed = speed;
        if (speed != GameSpeed.Paused) savedSpeed = speed;

        TimeDisplay.instance.ShowSpeed(speed);
    }

    public void TogglePause()
    {
        if (currSpeed == GameSpeed.Paused) SetSpeed(savedSpeed);
        else SetSpeed(GameSpeed.Paused);
    }

    public float GetDays()
    {
        return (float)ticks / ticksPerDay;
    }

}

