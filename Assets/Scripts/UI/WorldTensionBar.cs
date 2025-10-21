using UnityEngine;
using UnityEngine.UI;

public class WorldTensionBar : MonoBehaviour
{
    [SerializeField] ProgressBar tensionBar;


    private void Update()
    {
        tensionBar.SetLevel(WorldTension.Instance.CurrentTension / WorldTension.MaxTension);
    }
}
