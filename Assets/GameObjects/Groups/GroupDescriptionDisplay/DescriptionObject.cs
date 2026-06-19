using TMPro;
using UnityEngine;

namespace GameObjects.Groups.GroupDescriptionDisplay
{
    public class DescriptionObject : MonoBehaviour
    {
        //Accessors
        public static DescriptionObject Instance => FindAnyObjectByType<DescriptionObject>(FindObjectsInactive.Include);
        public bool Active=>gameObject.activeInHierarchy;
        
        private TextMeshProUGUI _text;

        public void Describe(Group group)
        {
            _text.text = group.GroupDescription.Description;
            gameObject.SetActive(true);
        }

        public void Close()
        {
            _text.text = "";
            gameObject.SetActive(false);
        }
    }
}