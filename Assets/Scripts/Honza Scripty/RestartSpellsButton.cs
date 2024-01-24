using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestartSpellsButton : MonoBehaviour
{
    private SpellManager spellScript;

    // Start is called before the first frame update
    void Start()
    {
        spellScript = FindObjectOfType<SpellManager>();
        Button button = GetComponent<Button>();
        button.onClick.AddListener(RestartSpells);
    }

    void RestartSpells()
    {
        spellScript.RestartSpellStates();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
