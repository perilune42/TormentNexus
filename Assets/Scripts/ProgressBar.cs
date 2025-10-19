using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    Image bar;
    private void Awake()
    {
        bar = GetComponent<Image>();
    }

    public void SetVisible(bool visible)
    {
        bar.transform.parent.gameObject.SetActive(visible);
    }

    public void SetLevel(float level)
    {
        bar.fillAmount = level;
    }
}