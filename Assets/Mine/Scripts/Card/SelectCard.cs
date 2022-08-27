using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DK
{
    public class SelectCard : MonoBehaviour
    {
        [HideInInspector] public bool cardAssigned;
        [HideInInspector] public Card card;
        [HideInInspector] public RawImage rawImage;

        [SerializeField] private GameObject cardNotAssignedMenu;
        [SerializeField] private GameObject cardAssignedMenu;

        private void Start()
        {
            card = new Card();
            cardAssigned = false;
            
            cardNotAssignedMenu.SetActive(true);
            cardAssignedMenu.SetActive(false);
        }

        public void AssignCard(Card _card , string _texture)
        {
            card = _card;
            cardAssigned = true;
            
            cardNotAssignedMenu.SetActive(false);
            cardAssignedMenu.SetActive(true);
            
            //AssignTheCorrectImage
            rawImage = cardAssignedMenu.GetComponentInChildren<RawImage>();
            rawImage.texture = Load_Image.GetImage(_texture);
        }
    }
}