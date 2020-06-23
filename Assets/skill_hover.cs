using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skill_hover : MonoBehaviour
{

    [Header("Handlers")]
    [SerializeField] private TextMesh title;
    [SerializeField] private TextMesh description;

    // 
    private void OnEnable ()
    {
        
    }

    // Clean up once hovering is no longer active
    private void OnDisable()
    {
        title.text = "";
        description.text = "";
    }



}
