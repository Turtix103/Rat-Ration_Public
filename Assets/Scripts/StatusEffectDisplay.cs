using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectDisplay : MonoBehaviour
{
    public GameObject bleedIcon;
    public GameObject burnIcon;
    public GameObject blightIcon;
    public GameObject stunIcon;

    public List<GameObject> displayedIcons;

    public float iconSize;

    public Transform pointOfDisplay;

    public void AddEffect(Enum.weaponEffect effect)
    {
        switch (effect)
        {
            case Enum.weaponEffect.none:
                return;
            case Enum.weaponEffect.bleed:
                bleedIcon.SetActive(true);
                displayedIcons.Add(bleedIcon);
                break;
            case Enum.weaponEffect.burn:
                burnIcon.SetActive(true);
                displayedIcons.Add(burnIcon);
                break;
            case Enum.weaponEffect.blight:
                blightIcon.SetActive(true);
                displayedIcons.Add(blightIcon);
                break;
            case Enum.weaponEffect.stun:
                stunIcon.SetActive(true);
                displayedIcons.Add(stunIcon);
                break;
        }
        Display();
    }

    public void RemoveEffect(Enum.weaponEffect effect)
    {
        switch (effect)
        {
            case Enum.weaponEffect.bleed:
                displayedIcons.Remove(bleedIcon);
                bleedIcon.SetActive(false);
                break;
            case Enum.weaponEffect.burn:
                displayedIcons.Remove(burnIcon);
                burnIcon.SetActive(false);
                break;
            case Enum.weaponEffect.blight:
                displayedIcons.Remove(blightIcon);
                blightIcon.SetActive(false);
                break;
            case Enum.weaponEffect.stun:
                displayedIcons.Remove(stunIcon);
                stunIcon.SetActive(false);
                break;
        }
        Display();
    }

    public void Display()
    {
        if (displayedIcons.Count % 2 == 0)
        {
            int count = displayedIcons.Count / 2;
            float offset = (count * iconSize) - iconSize / 2;

            for (int i = 0; i < displayedIcons.Count; i++)
            {
                displayedIcons[i].transform.position = new Vector3(pointOfDisplay.position.x + offset, pointOfDisplay.position.y);
                offset -= iconSize;
            }
        }
        else
        {
            int count = (displayedIcons.Count - 1) / 2;
            float offset = count * iconSize;

            for (int i = 0; i < displayedIcons.Count; i++)
            {
                displayedIcons[i].transform.position = new Vector3(pointOfDisplay.position.x + offset, pointOfDisplay.position.y);
                offset -= iconSize;
            }
        }
    }
}
