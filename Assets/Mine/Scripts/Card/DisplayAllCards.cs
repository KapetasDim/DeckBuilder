using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace DK
{
    public class DisplayAllCards : MonoBehaviour
    {
        private LoadAllCards loadAllCards;

        [SerializeField] private Transform cardsParent;
        [SerializeField] private GameObject cardPrefab;
        [SerializeField] private int cardsInRow = 5;

        private Vector2 firstCardOffset = new Vector2(0, 0);
        [SerializeField] private Vector2 targetFirstCardOffset = new Vector2();
        private Vector2 initialCardOffset;

        [SerializeField] private Vector2 cardSize = new Vector2(300, 412.5f);
        [SerializeField] private Vector2 cardDistance = new Vector2(100, 100);
        [SerializeField] private float scrollAmount = 350;
        [SerializeField] private float scrollDuration = 1;
        private float scrollTimer;

        private int[] cardDisplayOrder;
        private readonly List<Card_Instance> card_instances = new List<Card_Instance>();

        private void Start()
        {
            loadAllCards = GetComponent<LoadAllCards>();
            cardDisplayOrder = new int[loadAllCards.allCards.Count];
            for (int i = 0; i < cardDisplayOrder.Length; i++)
            {
                cardDisplayOrder[i] = i;
            }

            scrollTimer = 999999;
            firstCardOffset = targetFirstCardOffset;
            initialCardOffset = targetFirstCardOffset;

            CreateInstances();

            MakeDisplay();
        }

        //make all the necessary instances
        private void CreateInstances()
        {
            for (int i = 0; i < cardDisplayOrder.Length; i++)
            {
                //make all the necessary instances
                GameObject _instance = Instantiate(cardPrefab, Vector3.zero, Quaternion.identity,
                    cardsParent != null ? cardsParent : this.transform);

                //AssignTheCorrectImages
                RawImage rawImage = _instance.GetComponentInChildren<RawImage>();
                //rawImage.texture = loadAllCards.allCards_Images[i];
                rawImage.texture = Load_Image.GetImage(loadAllCards.allCards[i].fileName);

                //initialize the Card
                Card_Instance _cardInstance = _instance.GetComponent<Card_Instance>();
                _cardInstance.Init(loadAllCards.allCards[i], loadAllCards.allCards[i].fileName);
                card_instances.Add(_cardInstance);

                //Assign The Correct names
                _cardInstance.text.text = loadAllCards.allCards[i].name;
            }
        }

        private void Update()
        {
            SmoothScroll();
        }

        public void MakeDisplay()
        {
            PlaceInstances();
        }

        //move each instance to the correct location based on the cardDisplayOrder[]
        private void PlaceInstances()
        {
            if (cardDisplayOrder.Length == 0) return;

            for (int i = 0; i < cardDisplayOrder.Length; i++)
            {
                int xLoc = cardDisplayOrder[i] % cardsInRow;
                int yLoc = (int)(cardDisplayOrder[i] / cardsInRow);

                float xPos = xLoc * (cardSize.x + cardDistance.x) + firstCardOffset.x;
                float yPos = -yLoc * (cardSize.y + cardDistance.y) + firstCardOffset.y;

                // Debug.Log("xLoc: " + xLoc);
                // Debug.Log("yLoc: " + yLoc);
                // Debug.Log("xPos: " + xPos);
                // Debug.Log("yPos: " + yPos);

                RectTransform rT = card_instances[i].GetComponent<RectTransform>();
                rT.localPosition = new Vector3(xPos, yPos, 0);
            }
        }

        private void SmoothScroll()
        {
            float lerpPoint = scrollTimer / scrollDuration;
            if (lerpPoint > 1) return;

            firstCardOffset = Vector2.Lerp(firstCardOffset, targetFirstCardOffset, lerpPoint);
            scrollTimer += Time.deltaTime;
            MakeDisplay();
        }

        public void UpScrollButton()
        {
            targetFirstCardOffset.y = firstCardOffset.y - scrollAmount;
            scrollTimer = 0;

            //Clamp MIN MAX
            const int minimumVisibleRows = 3;
            int cardRows = (int)(math.ceil((float)cardDisplayOrder.Length / cardsInRow));
            targetFirstCardOffset.y = Mathf.Clamp(targetFirstCardOffset.y, initialCardOffset.y,
                (cardRows - minimumVisibleRows) * (cardSize.y + cardDistance.y) + initialCardOffset.y);
        }

        public void DownScrollButton()
        {
            targetFirstCardOffset.y = firstCardOffset.y + scrollAmount;
            scrollTimer = 0;

            //Clamp MIN MAX
            const int minimumVisibleRows = 3;
            int cardRows = (int)(math.ceil((float)cardDisplayOrder.Length / cardsInRow));
            targetFirstCardOffset.y = Mathf.Clamp(targetFirstCardOffset.y, initialCardOffset.y,
                (cardRows - minimumVisibleRows) * (cardSize.y + cardDistance.y) + initialCardOffset.y);
        }

        public void OrderByType()
        {
            loadAllCards.allCards = loadAllCards.allCards.OrderBy(x => x.subtype).ToList();

            ReOrder();
        }

        public void OrderByHP()
        {
            loadAllCards.allCards = loadAllCards.allCards.OrderBy(x => x.hp).ToList();
            ReOrder();
        }

        public void OrderByRarity()
        {
            loadAllCards.allCards = loadAllCards.allCards.OrderBy(x => x.rarity).ToList();
            ReOrder();
        }

        public void OrderByName()
        {
            loadAllCards.allCards = loadAllCards.allCards.OrderBy(x => x.fileName).ToList();
            ReOrder();
        }

        private void ReOrder()
        {
            // foreach (var card in loadAllCards.allCards)
            // {
            //     Debug.Log("card.name: " + card.name + " , card.rarity: " + card.rarity + " , card.subtype: " + card.subtype + " , card.hp: " + card.hp);
            // }
            
            for (int i = 0; i < card_instances.Count; i++)
            {
                var _instance = card_instances[i];
                //AssignTheCorrectImages
                RawImage rawImage = _instance.GetComponentInChildren<RawImage>();
                //rawImage.texture = loadAllCards.allCards_Images[i];
                rawImage.texture = Load_Image.GetImage(loadAllCards.allCards[i].fileName);

                //initialize the Card
                _instance.Init(loadAllCards.allCards[i], loadAllCards.allCards[i].fileName);

                //Assign The Correct names
                _instance.text.text = loadAllCards.allCards[i].name;
            }
            
            MakeDisplay();
        }

        // public void OrderByType()
        // {
        //     List<string> cardTypes = new List<string>();
        //
        //     List<Card> cardsToOrder = loadAllCards.allCards;
        //
        //     List<Card> cardsOrdered = new List<Card>();
        //
        //     cardTypes.Add(cardsToOrder[0].subtype);
        //     cardsOrdered.Add(cardsToOrder[0]);
        //     cardsToOrder.Remove(cardsToOrder[0]);
        //
        //     bool addNextType = false;
        //     
        //     for (int i = 0; i < cardsToOrder.Count; i++)
        //     {
        //         if (cardTypes.Contains(cardsToOrder[i].subtype))
        //         {
        //             cardsOrdered.Add(cardsToOrder[i]);
        //             cardsToOrder.Remove(cardsToOrder[i]);
        //             
        //
        //             if (cardsToOrder.Count != 0)
        //                 i = 0;
        //             else
        //                 break;
        //         }
        //         else
        //         {
        //             if (addNextType)
        //             {
        //                 addNextType = false;
        //                 cardTypes.Add(cardsToOrder[i].subtype);
        //                 cardsOrdered.Add(cardsToOrder[i]);
        //                 cardsToOrder.Remove(cardsToOrder[i]);
        //
        //                 if (cardsToOrder.Count != 0)
        //                     i = 0;
        //                 else
        //                     break;
        //             }
        //         }
        //
        //         if (i == cardsToOrder.Count - 1)
        //         {
        //             addNextType = true;
        //             i = 0;
        //         }
        //     }
        //
        //     if (cardDisplayOrder.Length != cardsOrdered.Count)
        //     {
        //         Debug.LogError("Lengths dont match -->>  cardDisplayOrder.Length = " + cardDisplayOrder.Length + " , cardsOrdered.Count = " + cardsOrdered.Count);
        //     }
        //     
        //     int displayOrder = 0;
        //     for (int i = 0; i < cardsOrdered.Count; i++)
        //     {
        //         int index = GetIndexOfItemInList(loadAllCards.allCards , cardsOrdered[i]);
        //         Debug.Log("index = " + index);
        //         if(index == -1) continue;
        //         cardDisplayOrder[index] = displayOrder;
        //         displayOrder += 1;
        //     }
        //     
        //     
        //     Debug.Log("cardTypes.Count: " + cardTypes.Count);
        //     
        //     MakeDisplay();
        // }

        private static int GetIndexOfItemInList(IReadOnlyList<Card> _list, Card item)
        {
            for (int i = 0; i < _list.Count; i++)
            {
                Debug.Log(" _list[i].fileName = " + _list[i].fileName);
                Debug.Log(" item[i].fileName = " + item.fileName);
                if (_list[i].fileName == item.fileName) return i;
            }

            return -1;
        }
    }
}