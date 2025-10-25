using JetBrains.Annotations;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public static GameUI instance;
    public Canvas techTreeCanvas;
    public TMP_Text primedAbilityText;
    public TMP_Text primedSubtitle;

    [SerializeField] Button techTreeButton;
    [SerializeField] Image techTreeButtonOverlay;

    float techFlashTime;
    [SerializeField] Color techFlashColor;

    [SerializeField] GameObject EndScreen;

    [SerializeField] TMP_Text researchAvailableText;

    [SerializeField] Color hpColor, garColor, infColor;

    private void Awake()
    {
        instance = this;

        PlayerControl.Instance.onPrimeAbility += (Ability a) =>
        {
            if (a != null)
            {
                primedAbilityText.text = $"Primed {a.Name}";
                primedSubtitle.text = FormatDamageString(a.Damage, a.GarrisonDamage, a.InfrastructureDamage);
            }
            else
            {
                primedAbilityText.text = "";
                primedSubtitle.text = "";
            }
        };

        primedAbilityText.text = "";
        primedSubtitle.text = "";
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
            researchAvailableText.enabled = false;

            if (playerTechTree.techNodeStatuses[playerTechTree.finalNode] == TechNodeStatus.Finished)
            {
                techTreeButton.image.color = Color.white;
            }
            else
            {
                researchAvailableText.enabled = true;
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
        }
        else
        {
            researchAvailableText.enabled = false;
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
        EndScreen.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = $"Day {(int)GameTick.instance.GetDays()}";
        EndScreen.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = $"Lives Lost {PopulationCounter.Instance.DeathCount:N0}";
        EndScreen.SetActive(true);
    }

    public string FormatDamageString(float dmg, float garDmg, float infDmg, bool perDay = false)
    {
        if (perDay) 
        {
            dmg *= 20;
            garDmg *= 20;
            infDmg *= 20;
            return $"<color=#{hpColor.ToHexString()}>{dmg:0.0}</color> | <color=#{garColor.ToHexString()}>{garDmg:0.0}</color> | <color=#{infColor.ToHexString()}>{infDmg:0.0}</color>";
        }
        else
        {
            return $"<color=#{hpColor.ToHexString()}>{(int)dmg}</color> | <color=#{garColor.ToHexString()}>{(int)garDmg}</color> | <color=#{infColor.ToHexString()}>{(int)infDmg}</color>";
        }
        
    }

}
