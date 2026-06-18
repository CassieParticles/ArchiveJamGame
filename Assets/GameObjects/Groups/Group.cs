using JetBrains.Annotations;
using UnityEngine;


public class Group : MonoBehaviour
{
    [SerializeField] Category category;
    public Category Category => category;

    [CanBeNull]
    public static Group GetGroup(Category category)
    {
        Group[] groups = FindObjectsByType<Group>(FindObjectsSortMode.None);
        foreach (var group in groups)
        {
            if (group.Category == category)
            {
                return group;
            }
        }
        //ERROR: GROUP NOT FOUND
        return null;
    }
}
