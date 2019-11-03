using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Manages the Player HUD elements
/// Authors: Michael
/// </summary>
public class PlayerHUD : MonoBehaviour
{
    // Modifiable Attributes
    public PlayerData playerData;
    public Spells playerSpells;
    public Slider hpSlider;
    public Image hpFill;
    public Slider spellSlider;
    public Image spellFill;

    // Private Attributes
    private int lastHealth;

    /// <summary>
    /// 
    /// </summary>
    void Start()
    {
        // Setting the private attributes
        lastHealth = playerData.health;
    }

    /// <summary>
    /// 
    /// </summary>
    void Update()
    {
        // IF the player health value is different than what it was previously...
        if (lastHealth != playerData.health)
        {
            // Setting the last health value to be the current health
            lastHealth = playerData.health;
            float healthPercent = ((1f * lastHealth) / playerData.maxHealth);

            // Updating the HP Slider
            hpSlider.value = healthPercent;
            hpFill.color = Color.HSVToRGB(healthPercent / 2.55f, 1f, 1f);
        }

        // Updating the Spell Slider + spell fill color
        spellSlider.value = playerSpells.SpellChargePercent();
        spellFill.color = playerSpells.SpellColorUI();
    }
}
