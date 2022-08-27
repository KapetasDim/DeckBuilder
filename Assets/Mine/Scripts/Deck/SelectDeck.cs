using UnityEngine;

namespace DK
{
    public class SelectDeck : MonoBehaviour
    {
        
        // //DEBUG
        // //DEBUG
        // //DEBUG
        // [SerializeField] LoadAllCards loadAllCards;
        // //DEBUG
        // //DEBUG
        // //DEBUG
        [HideInInspector] public bool deckAssigned;
        [HideInInspector] public Deck deck;

        [SerializeField] private GameObject deckNotAssignedMenu;
        [SerializeField] private GameObject deckAssignedMenu;

        private DeckCardsDisplay deckCardsDisplay;

        private void Start()
        {
            deck = new Deck();
            deckAssigned = false;
            
            deckNotAssignedMenu.SetActive(true);
            deckAssignedMenu.SetActive(false);

            deckCardsDisplay = GetComponent<DeckCardsDisplay>();
        }

        public void AddCardToDeck(Card c , string cardImage)
        {
            deck.cards.Add(c);
            deck.cards_images.Add(cardImage);
        }

        public void RemoveCardFromDeck(Card c , string cardImage)
        {
            deck.cards.Remove(c);
            deck.cards_images.Remove(cardImage);
        }

        public void AssignDeck(Deck _deck)
        {
            deck = _deck;
            
            // //DEBUG
            // //DEBUG
            // //DEBUG
            // deck.cards = loadAllCards.allCards;
            // deck.cards_images = loadAllCards.allCards_Images;
            // //DEBUG
            // //DEBUG
            // //DEBUG
            
            
            deckAssigned = true;
            
            deckNotAssignedMenu.SetActive(false);
            deckAssignedMenu.SetActive(true);

            deckCardsDisplay.SelectDeck_MakeInstances(deck);
        }

        public void DeselectDeck()
        {
            deck = null;
            deckAssigned = false;
            
            deckNotAssignedMenu.SetActive(true);
            deckAssignedMenu.SetActive(false);
        }
    }
}