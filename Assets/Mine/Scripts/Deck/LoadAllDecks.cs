using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Events;

namespace DK
{
    public class LoadAllDecks : MonoBehaviour
    {
        [HideInInspector] public List<Deck> allDecks = new List<Deck>();
        
        
        [HideInInspector] public UnityEvent listUpdated;

        private void Awake()
        {
            Load_AllDecks();
            
            listUpdated ??= new UnityEvent(); //if null make new
        }

        public void Load_AllDecks()
        {
            allDecks.Clear();
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Application.persistentDataPath + "/Decks/";
            Directory.CreateDirectory(path);

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

                    Deck foundDeck = formatter.Deserialize(stream) as Deck;

                    allDecks.Add(foundDeck);

                    stream.Close();
                }
                else
                {
                    Debug.LogError("Path not found: " + path_v2);
                }
            }
        }

        public void DestroyDeck(Deck _d , GameObject deckInstance)
        {
            allDecks.Remove(_d);
            Destroy(deckInstance);
            listUpdated.Invoke();
        }
    }
}