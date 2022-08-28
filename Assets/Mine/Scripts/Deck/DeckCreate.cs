using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace DK
{
    public class DeckCreate : MonoBehaviour
    {
        private LoadAllDecks loadAllDecks;

        private SelectDeck selectDeck;

        private void Start()
        {
            loadAllDecks = GetComponent<LoadAllDecks>();
            selectDeck = GetComponent<SelectDeck>();
        }

        public void NewDeck()
        {
            int currentDeckAmount = loadAllDecks.allDecks.Count;
            string deckNum = (currentDeckAmount + 1).ToString();
            string _deckName = "Deck_" + deckNum;
            
            Deck data = new Deck
            {
                deckName = _deckName
            };

            BinaryFormatter formatter = new BinaryFormatter();
            Directory.CreateDirectory(Application.persistentDataPath + "/Decks/");
            string path = Application.persistentDataPath + "/Decks/" + _deckName;

            FileStream stream = new FileStream(path, FileMode.Create);

            formatter.Serialize(stream, data);
            stream.Close();

            loadAllDecks.Load_AllDecks();
            loadAllDecks.listUpdated.Invoke();
        }

        public void SaveDeck()
        {
            if(!selectDeck.deckAssigned) return;
            
            string _deckName = selectDeck.deck.deckName;
            Deck data = selectDeck.deck;

            BinaryFormatter formatter = new BinaryFormatter();
            Directory.CreateDirectory(Application.persistentDataPath + "/Decks/");
            string path = Application.persistentDataPath + "/Decks/" + _deckName;

            FileStream stream = new FileStream(path, FileMode.Create);

            formatter.Serialize(stream, data);
            stream.Close();

            loadAllDecks.Load_AllDecks();
            loadAllDecks.listUpdated.Invoke();
        }
    }
}