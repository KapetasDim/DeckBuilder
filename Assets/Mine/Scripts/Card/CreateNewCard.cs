using System.Collections;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace DK
{
    public class CreateNewCard : MonoBehaviour
    {
        [SerializeField] private string pokemonName = "charizard";
        [SerializeField] private int index = 1;

        public void NewCard()
        {
            Card c = API_Helper.GetCardFromApi(pokemonName, index);

            Debug.Log(c.id);
            Debug.Log(c.name);
            Debug.Log(c.image_large_url);

            c.fileName = pokemonName;
            StartCoroutine(SaveImage(c.image_large_url , pokemonName));
            SaveInfo(c , pokemonName);
        }
        public void NewCard(string _cardName , int cardIndex)
        {
            Card c = API_Helper.GetCardFromApi(_cardName, cardIndex);

            // Debug.Log(c.id);
            // Debug.Log(c.name);
            // Debug.Log(c.image_large_url);

            c.fileName = _cardName;
            StartCoroutine(SaveImage(c.image_large_url , _cardName));
            SaveInfo(c , _cardName);
        }

        private static void SaveInfo(Card data , string cardName)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            Directory.CreateDirectory(Application.persistentDataPath + "/Pokemons/");
            string path = Application.persistentDataPath + "/Pokemons/" + cardName;

            FileStream stream = new FileStream(path, FileMode.Create);

            formatter.Serialize(stream, data);
            stream.Close();
        }


        private static IEnumerator SaveImage(string url , string cardName)
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