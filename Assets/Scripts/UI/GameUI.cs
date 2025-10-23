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

    [SerializeField] TMP_Text popCountText;
    [SerializeField] GameObject EndScreen;

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
        // if (Input.GetKeyDown(KeyCode.W)) ShowEndScreen(true);
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
                techFlashTime = 0.6f;
            }
            else if (techFlashTime <= 0.3f)
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

    public void ShowEndScreen(bool isVictory)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        EndScreen.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = FactionManager.instance.playerFaction.FactionName;
        EndScreen.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = $"Day {GameTick.instance.GetDays()}";
        EndScreen.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = $"Lives Lost {PopulationCounter.Instance.DeathCount:N0}";
        EndScreen.SetActive(true);
    }
}
