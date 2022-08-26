using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DK
{
    public class SelectCard : MonoBehaviour
    {
        private bool cardAssigned;
        private Card card;

        [SerializeField] private GameObject cardNotAssignedMenu;
        [SerializeField] private GameObject cardAssignedMenu;

        private void Start()
        {
            card = new Card();
            cardAssigned = false;
            
            cardNotAssignedMenu.SetActive(true);
            cardAssignedMenu.SetActive(false);
        }

        public void AssignCard(Card _card , Texture _texture)
        {
            card = _card;
            cardAssigned = true;
            
            cardNotAssignedMenu.SetActive(false);
            cardAssignedMenu.SetActive(true);
            
            //AssignTheCorrectImage
            RawImage rawImage = cardAssignedMenu.GetComponentInChildren<RawImage>();
            rawImage.texture = _texture;
        }
    }
}