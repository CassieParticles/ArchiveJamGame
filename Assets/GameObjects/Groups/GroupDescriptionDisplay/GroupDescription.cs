using UnityEngine;

public class ArtefactDescription
{
    public ArtefactDescription(Artefact artefact)
    {
        _name = artefact.name;
        //TODO: Get description of artefact
        _description = "TODO";
    }

    private string _name;
    private string _description;

    public string Name => (true ? _name : "?????");
    public string Description => _description;
}

public class GroupDescription : MonoBehaviour
{
    
}
