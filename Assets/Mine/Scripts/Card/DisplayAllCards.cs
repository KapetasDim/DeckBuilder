using System.Collections.Generic;
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
        private List<GameObject> card_instances = new List<GameObject>();

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
                card_instances.Add(_instance);
                
                //AssignTheCorrectImages
                RawImage rawImage = _instance.GetComponentInChildren<RawImage>();
                //rawImage.texture = loadAllCards.allCards_Images[i];
                rawImage.texture = Load_Image.GetImage(loadAllCards.allCards[i].fileName);

                //initialize the Card
                Card_Instance _cardInstance = _instance.GetComponent<Card_Instance>();
                _cardInstance.Init(loadAllCards.allCards[i] , loadAllCards.allCards[i].fileName);
                
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
    }
}