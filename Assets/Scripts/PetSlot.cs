using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PetSlot : MonoBehaviour
{
    private PetInfo petInfo;

    [SerializeField] private TextMeshProUGUI petNameText;
    [SerializeField] private Image petIcon;


    public void SetPetInfo(PetInfo newPet)
    {
        petInfo = newPet;
        petNameText.text = petInfo.petName;
        petIcon.sprite = petInfo.icon;
    }

    public void CheckThisPet()
    {
        GameManager.instance.ShowPetInfo(petInfo);
    }
}
