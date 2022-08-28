using UnityEngine;
using UnityEngine.SceneManagement;

namespace DK
{
    public class SwitchSceneOnAllCardsReady : MonoBehaviour
    {
        private CreateAllCardsOnGameBegin c;

        [SerializeField] private string sceneToLoad;

        private string activeScene;

        private void Start()
        {
            c = GetComponent<CreateAllCardsOnGameBegin>();
            c.allLoadedEvent.AddListener(ChangeScenes);

            Scene active = SceneManager.GetActiveScene();
            activeScene = active.name;
        }

        private void ChangeScenes()
        {
            SceneManager.LoadScene(!c.createdSomething ? 
                sceneToLoad : activeScene, LoadSceneMode.Single);
        }
    }
}