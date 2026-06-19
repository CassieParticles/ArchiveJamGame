using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ArtefactDescription
{
    public ArtefactDescription(Artefact artefact)
    {
        _name = artefact.name;
        _description = artefact.description;
        
        _artefact =  artefact;
    }

    private string _name;
    private string _description;
    private Artefact _artefact;

    public string Name => (_artefact.isPlaced ? _name : "?????");
    public string Description => _description;
}

public class GroupDescription : MonoBehaviour
{
    List<ArtefactDescription> _artefactDescriptions = new List<ArtefactDescription>();

    public string Description
    {
        get
        {
            StringBuilder builder = new StringBuilder();
            
            foreach(ArtefactDescription artefactDescription in _artefactDescriptions)
            {
                builder.Append(artefactDescription.Name);
                builder.Append("\n");
                builder.Append(artefactDescription.Description);
                builder.Append("\n");
            }
            
            return builder.ToString();
        }
    }
    public void AddDescription(Artefact artefact)
    {
        _artefactDescriptions.Add(new ArtefactDescription(artefact));
    }
}
