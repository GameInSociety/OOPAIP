using System.Collections.Generic;

[System.Serializable]
public class Speech
{
    public string id;
    public string[] speaker;
    public string start;
    public string end;
    public string stream;
    public string media;
    public List<string> annotations;
    public List<string> context_before;
}
