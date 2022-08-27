using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace DK
{
    public class DeckCardsDisplay : MonoBehaviour
    {
        private AddToDeck addToDeck;

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

        private List<int> cardDisplayOrder = new List<int>();
        private List<GameObject> card_instances = new List<GameObject>();

        private void Start()
        {
            addToDeck = GetComponent<AddToDeck>();
            addToDeck.listUpdated.AddListener(SelectDeck_MakeInstances);
        }

        private void SelectDeck_MakeInstances()
        {
            SelectDeck sD = FindObjectOfType<SelectDeck>();
            if (!sD.deckAssigned) return;
            
            SelectDeck_MakeInstances(sD.deck);
        }

        public void SelectDeck_MakeInstances(Deck d)
        {
            if (card_instances.Count != 0)
            {
                foreach (var instance in card_instances)
                {
                    Destroy(instance);
                }
            }
            card_instances.Clear();
            cardDisplayOrder.Clear();

            for (int i = 0; i < d.cards.Count; i++)
            {
                //make all the necessary instances
                GameObject _instance = Instantiate(cardPrefab, Vector3.zero, Quaternion.identity,
                    cardsParent != null ? cardsParent : this.transform);
                card_instances.Add(_instance);
                cardDisplayOrder.Add(cardDisplayOrder.Count);
                
                //AssignTheCorrectImages
                RawImage rawImage = _instance.GetComponentInChildren<RawImage>();
                rawImage.texture = Load_Image.GetImage(d.cards[i].fileName);

                //initialize the Card
                Card_Instance _cardInstance = _instance.GetComponent<Card_Instance>();
                _cardInstance.Init(d.cards[i] , d.cards[i].fileName);
                
                //Assign The Correct names
                _cardInstance.text.text = d.cards[i].name;
            }
            
            MakeDisplay();
        }
        

        private void Update()
        {
            SmoothScroll();
        }

        private void MakeDisplay()
        {
            PlaceInstances();
        }

        //move each instance to the correct location based on the cardDisplayOrder[]
        private void PlaceInstances()
        {
            if (cardDisplayOrder.Count == 0) return;

            for (int i = 0; i < cardDisplayOrder.Count; i++)
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
            int cardRows = (int)(math.ceil((float)cardDisplayOrder.Count / cardsInRow)) + 1;
            targetFirstCardOffset.y = Mathf.Clamp(targetFirstCardOffset.y, initialCardOffset.y,
                (cardRows - minimumVisibleRows) * (cardSize.y + cardDistance.y) + initialCardOffset.y);
        }

        public void DownScrollButton()
        {
            targetFirstCardOffset.y = firstCardOffset.y + scrollAmount;
            scrollTimer = 0;

            //Clamp MIN MAX
            const int minimumVisibleRows = 3;
            int cardRows = (int)(math.ceil((float)cardDisplayOrder.Count / cardsInRow)) + 1;
            targetFirstCardOffset.y = Mathf.Clamp(targetFirstCardOffset.y, initialCardOffset.y,
                (cardRows - minimumVisibleRows) * (cardSize.y + cardDistance.y) + initialCardOffset.y);
        }
    }
}