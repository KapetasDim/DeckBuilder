using System;
using UnityEngine;
using System.Net;
using System.IO;
using SimpleJSON;

namespace DK
{
    public static class API_Helper
    {
        /// <summary>
        /// if you dont know what to set the index then set it at zero
        /// </summary>
        /// <param name="pokemonName"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static Card GetCardFromApi(string pokemonName, int index)
        {
            HttpWebRequest request =
                (HttpWebRequest)WebRequest.Create("https://api.pokemontcg.io/v2/cards?q=name:" + pokemonName);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            //to read the response , its necessary to pass it through a stream reader
            StreamReader reader =
                new StreamReader(response.GetResponseStream() ??
                                 throw new InvalidOperationException()); //throws exception if null
            string json = reader.ReadToEnd();
            //CreateText(json);

            JSONNode jNode = JSON.Parse(json);

            // Debug.Log(jNode["data"].Count);
            // Debug.Log(jNode["data"][index]["id"]);
            // Debug.Log(jNode["data"][index]["name"]);
            // Debug.Log(jNode["data"][index]["images"]["large"]);

            Card _card = new Card
            {
                id = jNode["data"][index]["id"],
                name = jNode["data"][index]["name"],
                image_large_url = jNode["data"][index]["images"]["large"]
            };
            return _card;
        }

        // static void CreateText(string txt)
        // {
        //     string path = Application.dataPath + "/jsonTest.txt";
        //
        //     //if exists
        //     if (!File.Exists(path))
        //         File.Delete(path);
        //
        //     File.WriteAllText(path, txt);
        // }
    }
}