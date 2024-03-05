using System;
using System.Collections.Generic;
using Unity.VisualScripting;

[System.Serializable]
public class ClipData {

    public Slot[] events;
    public Slot[] mainThematics;
    public Slot[] secondThematics;
    // 
    public enum Field {
        Events,
        MainThematics,
        SecondThematics,
    }

    public Slot[] GetSlots(Field field) {
        switch (field) {
            case Field.Events:
                return events;
            case Field.MainThematics:
                return mainThematics;
            case Field.SecondThematics:
                return secondThematics;
        }

        return null;
    }

    
}