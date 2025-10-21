using UnityEngine;

public class WorldTension : MonoBehaviour
{
    public static WorldTension Instance;
    public float CurrentTension = 0;
    public const float MaxTension = 1000f;
    private const float decayRate = 0.005f;

    private void Awake()
    {
        Instance = this;
        GameTick.onTick += TensionDecay;
    }

    public void AddTension(float amount)
    {
        CurrentTension += amount;
    }

    private void TensionDecay()
    {
        CurrentTension -= decayRate;
        if (CurrentTension < 0) CurrentTension = 0;
    }


}
