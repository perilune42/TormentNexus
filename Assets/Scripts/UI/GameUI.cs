using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public static GameUI instance;
    public Canvas techTreeCanvas;
    public TMP_Text primedAbilityText;

    [SerializeField] Button techTreeButton;
    [SerializeField] Image techTreeButtonOverlay;

    float techFlashTime;
    [SerializeField] Color techFlashColor;

    private void Awake()
    {
        instance = this;

        PlayerControl.Instance.onPrimeAbility += (Ability a) =>
        {
            if (a != null)
            {
                primedAbilityText.text = $"Primed {a.gameObject.name}";
            }
            else
            {
                primedAbilityText.text = "";
            }
        };
        // techTreeCanvas.enabled = false;
    }

    private void Update()
    {
        ShowResearchProgress();
    }

    private void Start()
    {
        techTreeCanvas.enabled = false;
    }

    public void HideTechTree()
    {
        techTreeCanvas.enabled = false;
    }

    public void ToggleTechTree()
    {
        techTreeCanvas.enabled = !techTreeCanvas.enabled;
    }

    public void ShowResearchProgress()
    {
        TechTree playerTechTree = FactionManager.instance.playerFaction.TechTree;
        float progress = playerTechTree.GetCurrentTechProgress();
        if (progress < 0)
        {
            techTreeButtonOverlay.enabled = false;
            if (techFlashTime <= 0)
            {
                techTreeButton.image.color = techFlashColor;
                techFlashTime = 1f;
            }
            else if (techFlashTime <= 0.5f)
            {
                techTreeButton.image.color = Color.white;
            }
            techFlashTime -= Time.deltaTime;
        }
        else
        {
            techTreeButton.image.color = Color.white;
            techTreeButtonOverlay.enabled = true;
            techTreeButtonOverlay.fillAmount = 1 - progress;
        }
    }
}
