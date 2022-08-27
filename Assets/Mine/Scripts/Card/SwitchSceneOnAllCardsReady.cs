using UnityEngine;
using UnityEngine.SceneManagement;

namespace DK
{
    public class SwitchSceneOnAllCardsReady : MonoBehaviour
    {
        private CreateAllCardsOnGameBegin c;

        [SerializeField] private string sceneToLoad;

        private void Start()
        {
            c = GetComponent<CreateAllCardsOnGameBegin>();
            c.allLoadedEvent.AddListener(ChangeScenes);
        }

        private void ChangeScenes()
        {
            SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);
        }
    }
}