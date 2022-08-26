using UnityEditor;
using UnityEngine;

namespace DK
{
    [CustomEditor(typeof(CreateNewCard))]
    public class CreateNewCardEditor : UnityEditor.Editor
    {
        private CreateNewCard createNewCard;
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            createNewCard = (CreateNewCard) target;
            
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("CreateNewCard"))
            {
                createNewCard.NewCard();
            }
            GUILayout.EndHorizontal();
        }
    }
}
