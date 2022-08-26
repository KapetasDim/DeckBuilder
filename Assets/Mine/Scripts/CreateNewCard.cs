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

        public void NewCard()
        {
            Card c = API_Helper.GetCardFromApi(pokemonName, 1);

            Debug.Log(c.id);
            Debug.Log(c.name);
            Debug.Log(c.image_large_url);

            StartCoroutine(SaveImage(c.image_large_url));
            SaveInfoToTxt(c);
        }

        private void SaveInfoToTxt(Card data)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Application.dataPath + "/Mine/Pokemons/" + pokemonName;

            FileStream stream = new FileStream(path, FileMode.Create);

            formatter.Serialize(stream, data);
            stream.Close();
        }


        private IEnumerator SaveImage(string url)
        {
            UnityWebRequest spriteRequest = UnityWebRequestTexture.GetTexture(url);
            yield return spriteRequest.SendWebRequest();
            
            string path = Application.dataPath + "/Mine/Images/Pokemon_Images/" + pokemonName;

            byte[] imageBytes = spriteRequest.downloadHandler.data;
            File.WriteAllBytes(path, imageBytes);

        }
    }
}