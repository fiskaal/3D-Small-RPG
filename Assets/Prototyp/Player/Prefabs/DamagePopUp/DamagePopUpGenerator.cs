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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F10))
        {
            CreatePopUp(transform.position, 100.ToString());
        }
    }

    public void CreatePopUp(Vector3 position, string text)
    {
        var popup = Instantiate(prefab, position, quaternion.identity);
        var temp = popup.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        temp.text = "-" + text;
        
        //destroy timer
        Destroy(popup, 1f);
    }
}
