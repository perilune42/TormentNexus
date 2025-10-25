using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    Image bar;
    Canvas canvas;
    bool usingCanvas = false;

    private void Awake()
    {
        bar = GetComponent<Image>();
        canvas = bar.transform.parent.GetComponent<Canvas>();
        if (canvas != null) usingCanvas = true;
    }

    public void SetVisible(bool visible)
    {
        if (bar == null) bar = GetComponent<Image>();
        if (usingCanvas)
        {
            canvas.enabled = visible;
        }
        else
        {
            bar.transform.parent.gameObject.SetActive(visible);
        }
            
    }

    public void SetLevel(float level)
    {
        if (bar == null) return;
        bar.fillAmount = level;
    }
}