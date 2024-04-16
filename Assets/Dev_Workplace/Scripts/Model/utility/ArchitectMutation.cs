
using System;
using Unity.VisualScripting;

[Serializable]
public class ArchitectMutation {
    [Inspectable]
    public string baseArchitectCode;
    [Inspectable]
    public string buffArchitectCode;
    [Inspectable]
    public string mutantArchitectCode;
}