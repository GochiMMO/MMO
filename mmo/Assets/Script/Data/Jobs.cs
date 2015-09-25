using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

[XmlRoot("Jobs")]
public class Jobs {
    [XmlElement("Job")]
    public List<Job> job { get; set; }
}
