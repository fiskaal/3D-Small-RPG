using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class DamagePopUpGenerator : MonoBehaviour
{
    public static DamagePopUpGenerator current;
    [SerializeField]private GameObject prefab;

    // Update is called once per frame
    private void Awake()
    {
        current = this;
    }

    public void CreatePopUp(Vector3 position, string text, Color color)
    {
        var popup = Instantiate(prefab, position, Quaternion.identity);
        var temp = popup.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        temp.text = "-" + text;
        DamagePopUpAnimation popupScript = popup.transform.GetComponent<DamagePopUpAnimation>();
        popupScript.color = color;
        
        //destroy timer
        Destroy(popup, 1f);
    }
}
