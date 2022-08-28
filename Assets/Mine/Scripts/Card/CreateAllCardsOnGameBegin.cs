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
        private bool sliderNotNull;

        [HideInInspector] public UnityEvent allLoadedEvent;

        [HideInInspector] public bool createdSomething;

        private void Start()
        {
            sliderNotNull = slider != null;
        }

        private void Awake()
        {
            allLoadedEvent ??= new UnityEvent(); //if null make new

            if (cardsToCheck.Length == 0) Destroy(this);
            createNewCard = GetComponent<CreateNewCard>();

            if (slider == null) return;
            slider.minValue = 0;
            slider.maxValue = 100;

            CheckAndDelete();
        }

        /// <summary>
        /// Check the pokemon and the images folder. If there is a file in one that
        /// does not correspond to a file in the other folder then delete that file
        /// </summary>
        private void CheckAndDelete()
        {
            string path = Application.persistentDataPath + "/Pokemons/";
            string path2 = Application.persistentDataPath + "/Pokemon_Images/";
            
            if(!Directory.Exists(path) || !Directory.Exists(path2)) return;
            
            //Delete from path1
            var info = new DirectoryInfo(path);
            var fileInfo = info.GetFiles();
            var info2 = new DirectoryInfo(path2);
            var fileInfo2 = info2.GetFiles();

            foreach (var file in fileInfo)
            {
                string name1 = file.Name;
                bool found = false;
                
                foreach (var file2 in fileInfo2)
                {
                    string name2 = file2.Name;
                    if (name1 == name2)
                        found = true;
                }
                
                if(!found)
                    File.Delete(path + name1);
            }
            
            
            //Delete from path2
            info = new DirectoryInfo(path);
            fileInfo = info.GetFiles();
            info2 = new DirectoryInfo(path2);
            fileInfo2 = info2.GetFiles();

            foreach (var file2 in fileInfo2)
            {
                string name2 = file2.Name;
                bool found = false;
                
                foreach (var file1 in fileInfo)
                {
                    string name1 = file1.Name;
                    if (name2 == name1)
                        found = true;
                }
                
                if(!found)
                    File.Delete(path + name2);
            }
        }

        private void Update()
        {
            if (loopCount < cardsToCheck.Length && createNewCard.newCardDone)
                CheckExists(cardsToCheck[loopCount]);
            loopCount += 1;
            if (loopCount == cardsToCheck.Length)
                loopCount = 0;

            if (sliderNotNull)
            {
                slider.value = percentDone * 100;
            }

            if (Math.Abs(percentDone - 1) < TOLERANCE)
            {
                if(createdSomething)
                    StartCoroutine(InvokeEventAfterTIme());
                else
                    allLoadedEvent.Invoke();
            }
        }

        IEnumerator InvokeEventAfterTIme()
        {
            yield return new WaitForSeconds(7f);
            allLoadedEvent.Invoke();
            
        }

        /// <summary>
        /// check if a card exists and if not create it
        /// </summary>
        private void CheckExists(CardNameAndIndex c)
        {
            string path = Application.persistentDataPath + "/Pokemon_Images/" + c.cardName;
            string path2 = Application.persistentDataPath + "/Pokemons/" + c.cardName;
            if (!File.Exists(path2) || !File.Exists(path2))
            {
                createNewCard.NewCard(c.cardName, c.cardIndex);
                createdSomething = true;
            }
            else
            {
                percentDone += 1.0f / cardsToCheck.Length;
            }
        }
    }
}