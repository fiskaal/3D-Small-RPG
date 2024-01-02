using System.Collections;
using UnityEngine;
using TMPro;
using System.Reflection;

public class HealthText : MonoBehaviour
{
    [SerializeField] private HealthSystem healthSystem; // Reference to the HealthSystem script
    [SerializeField] private TMP_Text maxHealthText;    // Reference to TextMeshPro component for MaxHealth
    [SerializeField] private TMP_Text healthText;       // Reference to TextMeshPro component for Health

    void Start()
    {
        // Ensure that the HealthSystem script is attached to the same GameObject
        if (healthSystem == null)
        {
            Debug.LogError("HealthSystem script reference is missing!");
            return;
        }

        // Set initial text values
        UpdateTextValues();
    }

    void Update()
    {
        // Update text values every frame
        UpdateTextValues();
    }

    private void UpdateTextValues()
    {
        // Update MaxHealth and Health TextMeshPro components using reflection
        if (maxHealthText != null)
        {
            maxHealthText.text = GetPrivateField<float>(healthSystem, "maxHealth").ToString("0");
        }

        if (healthText != null)
        {
            healthText.text = GetPrivateField<float>(healthSystem, "health").ToString("0");
        }
    }

    private T GetPrivateField<T>(object instance, string fieldName)
    {
        BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
        FieldInfo field = instance.GetType().GetField(fieldName, bindFlags);
        return (T)field.GetValue(instance);
    }
}
