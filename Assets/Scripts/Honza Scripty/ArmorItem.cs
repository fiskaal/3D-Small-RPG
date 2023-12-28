using UnityEngine;

public class ArmorItem : MonoBehaviour
{
    [SerializeField] float armorHPBonus = 10f;

    // Getter for the armor bonus
    public float GetArmorBonus()
    {
        return armorHPBonus;
    }
}
