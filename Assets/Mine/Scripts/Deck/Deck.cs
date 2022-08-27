using System.Collections.Generic;
using UnityEngine;

namespace DK
{
    [System.Serializable]
    public class Deck
    {
        public string deckName;
        public List<Card> cards = new List<Card>();
        public List<string> cards_images = new List<string>();
    }
}