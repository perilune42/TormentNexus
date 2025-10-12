using System.Security.Cryptography;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager instance;

    public float researchPoints;


    private void Awake()
    {
        instance = this;

        GameTick.onDay += GainResearchPoint;

    }


    private void GainResearchPoint()
    {
        researchPoints += 1.5f;
        TechTree.instance.AddResearchPoints(researchPoints);
        researchPoints = 0;
    }



}
