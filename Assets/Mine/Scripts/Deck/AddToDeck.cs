using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DK
{
    public class AddToDeck : MonoBehaviour
    {
        [SerializeField] private SelectCard selectCard;
        [SerializeField] private SelectDeck selectDeck;
        
        [HideInInspector] public UnityEvent listUpdated;

        private void Awake()
        {
            listUpdated ??= new UnityEvent(); //if null make new
        }

        public void AddCardToDeck()
        {
            if(!selectCard.cardAssigned) return;

            selectDeck.AddCardToDeck(selectCard.card, selectCard.card.name);
            
            listUpdated.Invoke();
        }
        
        public void RemoveCardFromDeck()
        {
            if(!selectCard.cardAssigned) return;

            selectDeck.RemoveCardFromDeck(selectCard.card, selectCard.card.name);
            
            listUpdated.Invoke();
        }
    }
}