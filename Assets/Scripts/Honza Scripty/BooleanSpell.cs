using UnityEngine;

public class BooleanSpell : MonoBehaviour
{
    [SerializeField]
    private bool spellBought = false;

    public bool SpellBought
    {
        get { return spellBought; }
        set { spellBought = value; }
    }
}
