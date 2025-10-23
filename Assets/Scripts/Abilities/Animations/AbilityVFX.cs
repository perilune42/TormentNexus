using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AbilityVFX : MonoBehaviour
{
    [SerializeField] GameObject AttackerCanvasPrefab;
    private GameObject canvas;
    private Image attackerFlag;
    private Image border;

    public virtual void Play(Faction attacker)
    {
        canvas = Instantiate(AttackerCanvasPrefab, transform);
        border = canvas.transform.GetChild(0).GetComponent<Image>();
        attackerFlag = canvas.transform.GetChild(0).GetChild(0).GetComponent<Image>();

        border.color = attacker.FactionColor;
        attackerFlag.sprite = attacker.Flag;

        StartCoroutine(FadeAndDestroyAfterDelay());
    }
    
    private IEnumerator FadeAndDestroyAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        while (attackerFlag.color.a > 0)
        {
            attackerFlag.color -= new Color(0, 0, 0, Time.deltaTime * 1.5f);
            border.color -= new Color(0, 0, 0, Time.deltaTime * 1.5f);
            yield return null;
        }
        Destroy(canvas);
    }
}