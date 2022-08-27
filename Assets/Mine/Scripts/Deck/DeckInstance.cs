using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine;

namespace DK
{
    public class DeckInstance : MonoBehaviour
    {
        private Deck deck = new Deck();
        
        public TextMeshProUGUI text;

        private SelectDeck selectDeck;

        private LoadAllDecks loadAllDecks;

        private void Start()
        {
            selectDeck = FindObjectOfType<SelectDeck>();
        }

        public void Init(Deck _d , LoadAllDecks _l)
        {
            deck = _d;
            loadAllDecks = _l;
        }

        public void SelectDeck()
        {
            selectDeck.AssignDeck(deck);
        }

        public void DestroyDeck()
        {
            string path = Application.persistentDataPath + "/Decks/" + deck.deckName;
            if (File.Exists(path))
            {
                File.Delete(path);
                loadAllDecks.DestroyDeck(deck, this.gameObject);
            }
            else
            {
                Debug.LogError("Path not found: " + path);
            }
        }
    }
}