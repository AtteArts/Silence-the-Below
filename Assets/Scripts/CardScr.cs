using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.Rendering.Universal;


public class CardScr : MonoBehaviour
{
    public Image Logo;
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Description;
    public TextMeshProUGUI EnergyCost;


    public void CreateCard(Card card)
    {
        Name.text = card.Name;
        Description.text = card.Description;
        EnergyCost.text = card.EnergyCost.ToString();
        Logo.sprite = card.Sprite;
    }

    void Awake()
    {
        
    }
}
