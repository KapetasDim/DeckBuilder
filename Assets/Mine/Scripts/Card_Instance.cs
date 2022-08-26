using System;
using TMPro;
using UnityEngine;

namespace DK
{
    public class Card_Instance : MonoBehaviour
    {
        private Card card = new Card();

        public TextMeshProUGUI text;

        private SelectCard selectCard;

        private Texture image;

        private void Start()
        {
            selectCard = FindObjectOfType<SelectCard>();
        }

        public void Init(Card _c , Texture _image)
        {
            card = _c;
            image = _image;
        }

        public string GetCard_imageLargeUrl()
        {
            return card.image_large_url;
        }

        public void SelectCard()
        {
            selectCard.AssignCard(card, image);
        }
    }
}