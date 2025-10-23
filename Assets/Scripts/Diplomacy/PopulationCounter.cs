using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class PopulationCounter : MonoBehaviour
{
    public long CurrentPopulation = 3193447025;
    public long DeathCount = 0;

    public static PopulationCounter Instance;
    [SerializeField] private TMP_Text counterText;

    private void Awake()
    {
        Instance = this;
        GameTick.onTick += Growth;
        GameTick.onDay += ShowPop;
        ShowPop();
    }


    public void Growth()
    {
        CurrentPopulation += (long)(CurrentPopulation * (0.0012 * 0.01 / 20)) ;
    }

    private void ShowPop()
    {
        counterText.text = CurrentPopulation.ToString("N0");
    }

    public void DealDamage(float infDamage)
    {
        long populationLost = (long)(Mathf.Min(CurrentPopulation * 0.01f, infDamage * 20000) * Random.Range(0.5f, 1.5f));
        DeathCount += populationLost;
        CurrentPopulation -= populationLost;
    }
}