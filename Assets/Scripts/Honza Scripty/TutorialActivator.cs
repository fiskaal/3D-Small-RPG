using UnityEngine;

public class TutorialActivator : MonoBehaviour
{
    private TutorialManager tutorialManager;

    // Objects for Smith's tutorial
    public GameObject[] smithCompletedObjects;
    public GameObject[] smithNotCompletedObjects;

    // Objects for Wizard's tutorial
    public GameObject[] wizardCompletedObjects;
    public GameObject[] wizardNotCompletedObjects;

    private void Start()
    {
        tutorialManager = FindObjectOfType<TutorialManager>();
    }

    private void Update()
    {
        if (tutorialManager != null)
        {
            // Check if Smith's tutorial is complete
            if (TutorialManager.isSmithComplete)
            {
                ActivateObjects(smithCompletedObjects);
                DeactivateObjects(smithNotCompletedObjects);
            }
            else
            {
                ActivateObjects(smithNotCompletedObjects);
                DeactivateObjects(smithCompletedObjects);
            }

            // Check if Wizard's tutorial is complete
            if (TutorialManager.isWizardComplete)
            {
                ActivateObjects(wizardCompletedObjects);
                DeactivateObjects(wizardNotCompletedObjects);
            }
            else
            {
                ActivateObjects(wizardNotCompletedObjects);
                DeactivateObjects(wizardCompletedObjects);
            }
        }
    }

    private void DeactivateObjects(GameObject[] objectsToDeactivate)
    {
        foreach (var obj in objectsToDeactivate)
        {
            obj.SetActive(false);
        }
    }

    private void ActivateObjects(GameObject[] objectsToActivate)
    {
        foreach (var obj in objectsToActivate)
        {
            obj.SetActive(true);
        }
    }
}
