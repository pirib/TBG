using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Tilemaps;

public class hover_info : MonoBehaviour
{

    [Header("Handlers")]

    [Header("Skill Icon")]
    [SerializeField] public SpriteRenderer skill_icon;

    [Header("Other Handlers")]
    public Tilemap hud;
    public GameObject bg;


    [Header("Skill info")]
    [SerializeField] public TMPro.TextMeshPro title;
    [SerializeField] public TMPro.TextMeshPro description;

    [Header("Charge condition")]
    [SerializeField] public TMPro.TextMeshPro condition_title;
    [SerializeField] public TMPro.TextMeshPro condition_description;

    [Header("Charge mode")]
    [SerializeField] public TMPro.TextMeshPro mode_title;
    [SerializeField] public TMPro.TextMeshPro mode_description;

}
