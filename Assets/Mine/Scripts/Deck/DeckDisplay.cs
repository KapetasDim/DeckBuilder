using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace DK
{
    public class DeckDisplay : MonoBehaviour
    {
        private LoadAllDecks loadAllDecks;

        [SerializeField] private Transform deckParent;
        [SerializeField] private GameObject deckPrefab;
        [SerializeField] private int decksInRow = 5;

        private Vector2 firstDeckOffset = new Vector2(0, 0);
        [SerializeField] private Vector2 targetFirstDeckOffset = new Vector2();
        private Vector2 initialDeckOffset;

        [SerializeField] private Vector2 deckSize = new Vector2(312, 435);
        [SerializeField] private Vector2 deckDistance = new Vector2(100, 100);
        [SerializeField] private float scrollAmount = 350;
        [SerializeField] private float scrollDuration = 1;
        private float scrollTimer;

        private int[] deckDisplayOrder;
        private List<GameObject> deck_instances = new List<GameObject>();

        private void Start()
        {
            loadAllDecks = GetComponent<LoadAllDecks>();
            loadAllDecks.listUpdated.AddListener(Init);
            
            Init();
        }

        private void Init()
        {
            deckDisplayOrder = new int[loadAllDecks.allDecks.Count];
            for (int i = 0; i < deckDisplayOrder.Length; i++)
            {
                deckDisplayOrder[i] = i;
            }

            scrollTimer = 999999;
            firstDeckOffset = targetFirstDeckOffset;
            initialDeckOffset = targetFirstDeckOffset;

            DestroyInstances();
            
            CreateInstances();
            MakeDisplay();

        }

        private void DestroyInstances()
        {
            if(deck_instances.Count == 0) return;

            foreach (var deck_instance in deck_instances)
            {
                Destroy(deck_instance);
            }
        }
        
        //make all the necessary instances
        private void CreateInstances()
        {
            deck_instances.Clear();
            for (int i = 0; i < deckDisplayOrder.Length; i++)
            {
                //make all the necessary instances
                GameObject _instance = Instantiate(deckPrefab, Vector3.zero, Quaternion.identity,
                    deckParent != null ? deckParent : this.transform);
                deck_instances.Add(_instance);
                
                //initialize the Card
                DeckInstance _deckInstance = _instance.GetComponent<DeckInstance>();
                _deckInstance.Init(loadAllDecks.allDecks[i] , loadAllDecks);
                
                //Assign The Correct names
                _deckInstance.text.text = loadAllDecks.allDecks[i].deckName;
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

        //move each instance to the correct location based on the deckDisplayOrder[]
        private void PlaceInstances()
        {
            if (deckDisplayOrder.Length == 0) return;

            for (int i = 0; i < deckDisplayOrder.Length; i++)
            {
                int xLoc = deckDisplayOrder[i] % decksInRow;
                int yLoc = (int)(deckDisplayOrder[i] / decksInRow);

                float xPos = xLoc * (deckSize.x + deckDistance.x) + firstDeckOffset.x;
                float yPos = -yLoc * (deckSize.y + deckDistance.y) + firstDeckOffset.y;

                // Debug.Log("xLoc: " + xLoc);
                // Debug.Log("yLoc: " + yLoc);
                // Debug.Log("xPos: " + xPos);
                // Debug.Log("yPos: " + yPos);

                RectTransform rT = deck_instances[i].GetComponent<RectTransform>();
                rT.localPosition = new Vector3(xPos, yPos, 0);
            }
        }

        private void SmoothScroll()
        {
            float lerpPoint = scrollTimer / scrollDuration;
            if (lerpPoint > 1) return;

            firstDeckOffset = Vector2.Lerp(firstDeckOffset, targetFirstDeckOffset, lerpPoint);
            scrollTimer += Time.deltaTime;
            MakeDisplay();
        }

        public void UpScrollButton()
        {
            targetFirstDeckOffset.y = firstDeckOffset.y - scrollAmount;
            scrollTimer = 0;

            //Clamp MIN MAX
            const int minimumVisibleRows = 3;
            int cardRows = (int)(math.ceil((float)deckDisplayOrder.Length / decksInRow));
            targetFirstDeckOffset.y = Mathf.Clamp(targetFirstDeckOffset.y, initialDeckOffset.y,
                (cardRows - minimumVisibleRows) * (deckSize.y + deckDistance.y) + initialDeckOffset.y);
        }

        public void DownScrollButton()
        {
            targetFirstDeckOffset.y = firstDeckOffset.y + scrollAmount;
            scrollTimer = 0;

            //Clamp MIN MAX
            const int minimumVisibleRows = 3;
            int cardRows = (int)(math.ceil((float)deckDisplayOrder.Length / decksInRow));
            targetFirstDeckOffset.y = Mathf.Clamp(targetFirstDeckOffset.y, initialDeckOffset.y,
                (cardRows - minimumVisibleRows) * (deckSize.y + deckDistance.y) + initialDeckOffset.y);
        }
    }
}