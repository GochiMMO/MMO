using UnityEngine;
using System.Collections;
using System.Xml.Serialization;

public class Job{
    [XmlElement("name")]
    public string name { get; set; }
    [XmlElement("hp")]
    public string hp { get; set; }
    [XmlElement("sp")]
    public string sp { get; set; }
    [XmlElement("str")]
    public string str { get; set; }
    [XmlElement("vit")]
    public string vit { get; set; }
    [XmlElement("int")]
    public string intelligence { get; set; }
    [XmlElement("mnd")]
    public string mnd { get; set; }
}
