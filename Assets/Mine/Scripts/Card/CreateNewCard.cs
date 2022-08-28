using System;
using System.Collections;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using SimpleJSON;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace DK
{
    public class CreateNewCard : MonoBehaviour
    {
        [SerializeField] private string pokemonName = "charizard";
        [SerializeField] private int index = 1;

        [HideInInspector] public bool newCardDone;

        private void Awake()
        {
            newCardDone = true;
        }

        public void NewCard()
        {
            StartCoroutine(NewCardRoutine(pokemonName, index));
        }

        public void NewCard(string _cardName, int cardIndex)
        {
            StartCoroutine(NewCardRoutine(_cardName, cardIndex));
        }

        IEnumerator NewCardRoutine(string _cardName, int cardIndex)
        {
            newCardDone = false;
            
            string url = "https://api.pokemontcg.io/v2/cards?q=name:" + _cardName;
            UnityWebRequest webRequest = UnityWebRequest.Get(url);
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError(webRequest.error);
                yield break;
            }

            JSONNode jNode = JSON.Parse(webRequest.downloadHandler.text);

            // Debug.Log(jNode["data"].Count);
            // Debug.Log(jNode["data"][index]["id"]);
            // Debug.Log(jNode["data"][index]["name"]);
            // Debug.Log(jNode["data"][index]["images"]["large"]);

            var _card = new Card();
            if (jNode["data"] != null)
            {
                if (jNode["data"][cardIndex]["id"] != null)
                {
                    _card.id = jNode["data"][cardIndex]["id"];
                    _card.name = jNode["data"][cardIndex]["name"];
                    _card.image_large_url = jNode["data"][cardIndex]["images"]["large"];
                    _card.subtype = jNode["data"][cardIndex]["subtypes"][0];
                    _card.hp = jNode["data"][cardIndex]["hp"];
                    _card.rarity = jNode["data"][cardIndex]["rarity"];
                    _card.fileName = _cardName;
                }
                else
                {
                    Debug.LogError("(part2) Error With Pokemon: " + _cardName);
                }
            }
            else
            {
                Debug.LogError("(part1) Error With Pokemon: " + _cardName);
            }
            
            //if health is under 100 , add a 0 to the front so for example 80 becomes 080.. that way it will work on arrange by hp latter.
            if (_card.hp.ToIntArray().Length == 2)
            {
                _card.hp = "0" + _card.hp;
            }
            
            // Debug.Log("_card.id: " + _card.id);
            // Debug.Log("_card.name: " + _card.name);
            // Debug.Log("_card.image_large_url: " + _card.image_large_url);
            // Debug.Log("_card.hp: " + _card.hp);
            // Debug.Log("_card.rarity: " + _card.rarity);
            Debug.Log("_card.subtypes: " + _card.subtype);


            StartCoroutine(SaveImage(_card.image_large_url, _cardName));
            SaveInfo(_card, _cardName);

            newCardDone = true;
        }

        private static void SaveInfo(Card data, string cardName)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            Directory.CreateDirectory(Application.persistentDataPath + "/Pokemons/");
            string path = Application.persistentDataPath + "/Pokemons/" + cardName;

            FileStream stream = new FileStream(path, FileMode.Create);

            formatter.Serialize(stream, data);
            stream.Close();
        }


        private static IEnumerator SaveImage(string url, string cardName)
        {
            UnityWebRequest spriteRequest = UnityWebRequestTexture.GetTexture(url);
            yield return spriteRequest.SendWebRequest();

            Directory.CreateDirectory(Application.persistentDataPath + "/Pokemon_Images/");
            string path = Application.persistentDataPath + "/Pokemon_Images/" + cardName;

            byte[] imageBytes = spriteRequest.downloadHandler.data;
            File.WriteAllBytes(path, imageBytes);
        }
    }
}