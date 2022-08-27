using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DK
{
    public class CreateAllCardsOnGameBegin : MonoBehaviour
    {
        private const float TOLERANCE = 0.0005f;
        
        [SerializeField] private CardNameAndIndex[] cardsToCheck;
        private CreateNewCard createNewCard;

        [HideInInspector] public float percentDone;
        private int loopCount;

        [SerializeField] private Slider slider;
        
        [HideInInspector] public UnityEvent allLoadedEvent;
        
        private void Awake()
        {
            
            allLoadedEvent ??= new UnityEvent(); //if null make new
            
            if(cardsToCheck.Length == 0) Destroy(this);
            createNewCard = GetComponent<CreateNewCard>();

            if (slider == null) return;
            slider.minValue = 0;
            slider.maxValue = 100;
        }

        private void Update()
        {
            if (loopCount >= cardsToCheck.Length) return;
            CheckExists(cardsToCheck[loopCount]);
            loopCount += 1;

            if (loopCount == cardsToCheck.Length)
                StartCoroutine(EventAfterTime(2f));
        }

        IEnumerator EventAfterTime(float _time)
        {
            yield return new WaitForSeconds(_time);
            allLoadedEvent.Invoke();
            
        }

        /// <summary>
        /// check if a card exists and if not create it
        /// </summary>
        private void CheckExists(CardNameAndIndex c)
        {
            string path = Application.persistentDataPath + "/Pokemons/" + c.cardName;
            if (!File.Exists(path))
            {
                createNewCard.NewCard(c.cardName , c.cardIndex);
            }
            percentDone += 1.0f / cardsToCheck.Length;
            
            if (slider == null) return;
            slider.value = percentDone * 100;
        }
    }
}