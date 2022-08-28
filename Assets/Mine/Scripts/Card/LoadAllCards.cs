using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace DK
{
    public class LoadAllCards : MonoBehaviour
    {
        [HideInInspector] public List<Card> allCards = new List<Card>();
        //[HideInInspector] public List<Texture> allCards_Images = new List<Texture>();

        [HideInInspector] public bool errorOccured;

        [SerializeField] private bool loadOnAwake = true;

        private void Awake()
        {
            if(loadOnAwake)
                Load_AllCards();
            //Load_AllImages();
        }

        public void Load_AllCards()
        {
            errorOccured = false;
            
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Application.persistentDataPath + "/Pokemons/";

            var info = new DirectoryInfo(path);
            var fileInfo = info.GetFiles();

            // int count = 0;
            // Debug.Log(fileInfo.Length);
            foreach (var file in fileInfo)
            {
                // Debug.Log(count);
                // count += 1;

                //first i need to discard the file if its a .meta file
                if (file.Name.Contains(".meta")) continue;

                //add the file (card) to the list
                string path_v2 = path + file.Name;
                if (File.Exists(path_v2))
                {
                    FileStream stream = new FileStream(path_v2, FileMode.Open);

                    Card foundCard = formatter.Deserialize(stream) as Card;

                    allCards.Add(foundCard);

                    stream.Close();
                }
                else
                {
                    Debug.LogError("Path not found: " + path_v2);
                    errorOccured = true;
                }
            }
        }
    }
}