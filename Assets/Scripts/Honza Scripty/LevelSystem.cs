// LevelSystem.cs

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class LevelSystem : MonoBehaviour
{
    public int playerLevel = 1;
    public float experienceThreshold = 100f;
    public float currentExperience = 0f;
    public GameObject upgradePanels;
    public TextMeshProUGUI levelText;

    // Update this list to have unique keys for each BooleanSpell
    public List<string> booleanSpellKeys;

    public void GainExperience(float experiencePoints)
    {
        currentExperience += experiencePoints;

        if (currentExperience >= experienceThreshold)
        {
            LevelUp();
        }
    }

    void LevelUp()
    {
        playerLevel++;
        currentExperience = 0f;
        experienceThreshold = CalculateExperienceThreshold();
        ActivateRandomUpgradePanels();
        UpdateLevelText();
        Debug.Log("Level Up! New Level: " + playerLevel);
        Time.timeScale = 0f;
    }

    float CalculateExperienceThreshold()
    {
        return experienceThreshold * 2f;
    }

    void ActivateRandomUpgradePanels()
    {
        if (upgradePanels != null)
        {
            int childCount = upgradePanels.transform.childCount;

            // Collect a list of indices representing panels with SpellBought set to true
            List<int> trueIndices = new List<int>();
            for (int i = 0; i < childCount; i++)
            {
                GameObject panelObject = upgradePanels.transform.GetChild(i)?.gameObject;
                if (panelObject != null)
                {
                    BooleanSpell booleanSpellComponent = panelObject.GetComponent<BooleanSpell>();

                    // Check if the panel has the BooleanSpell component and SpellBought is true
                    if (booleanSpellComponent != null && booleanSpellComponent.SpellBought)
                    {
                        trueIndices.Add(i);
                    }
                }
                else
                {
                    Debug.LogError("Panel object at index " + i + " is null!");
                }
            }

            // Activate a maximum of 3 panels with SpellBought set to true
            int panelsToActivate = Mathf.Min(3, trueIndices.Count);
            for (int i = 0; i < panelsToActivate; i++)
            {
                if (trueIndices.Count == 0)
                {
                    Debug.LogWarning("Not enough panels with SpellBought set to true.");
                    break;
                }

                int randomIndex = Random.Range(0, trueIndices.Count);
                int panelIndex = trueIndices[randomIndex];

                GameObject panelObject = upgradePanels.transform.GetChild(panelIndex)?.gameObject;

                // Ensure the panel object is not null
                if (panelObject != null)
                {
                    panelObject.SetActive(true);
                }
                else
                {
                    Debug.LogError("Activated panel object is null!");
                }

                // Remove the index from the list to avoid duplication
                trueIndices.RemoveAt(randomIndex);
            }
        }
        else
        {
            Debug.LogError("UpgradePanels not assigned!");
        }
    }





    void UpdateLevelText()
    {
        if (levelText != null)
        {
            levelText.text = playerLevel.ToString();
        }
    }
}
