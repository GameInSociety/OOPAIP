using System.Collections.Generic;


[System.Serializable]
public class Slot
{
    /*public Slot(Slot copy) {
        id = copy.id;
        annotation_type = copy.annotation_type;
        speaker = copy.speaker;
        start = copy.start;
        end = copy.end;
        stream = copy.stream;
        media = copy.media;
        annotations = copy.annotations;
        context_before = copy.context_before;
    }*/

    public string id;
    public string annotation_type;
    public string[] speaker;
    public string start;
    public string end;
    public string stream;
    public string media;
    public List<string> annotations;
    public List<string> context_before;
}
