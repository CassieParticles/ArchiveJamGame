using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;


public class Group : MonoBehaviour
{
    [SerializeField] Category category;
    public Category Category => category;
    
    private List<Artefact> _addedArtefacts = new List<Artefact>();

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

    //Add artefact if it matches group, then return if successful
    public bool AddArtefact(Artefact artefact)
    {
        if (artefact.Category != category)
        {
            //TODO: Wrong artefact
            return false;
        }
        
        //TODO: Right artefact
        _addedArtefacts.Add(artefact);
        
        return true;
    }
}
