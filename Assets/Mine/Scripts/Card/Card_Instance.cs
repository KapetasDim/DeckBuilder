using System;
using TMPro;
using UnityEngine;

namespace DK
{
    public class Card_Instance : MonoBehaviour
    {
        [HideInInspector] public Card card = new Card();

        public TextMeshProUGUI text;

        private SelectCard selectCard;

        private string imageName;

        private void Start()
        {
            selectCard = FindObjectOfType<SelectCard>();
        }

        public void Init(Card _c , string _imageName)
        {
            card = _c;
            imageName = _imageName;
        }

        public string GetCard_imageLargeUrl()
        {
            return card.image_large_url;
        }

        public void SelectCard()
        {
            selectCard.AssignCard(card, imageName);
        }
    }
}