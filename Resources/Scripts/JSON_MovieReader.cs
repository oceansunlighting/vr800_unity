using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text.RegularExpressions;
using RE = System.Text.RegularExpressions.Regex;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using VRStandardAssets.Utils;

[Serializable]
public class JSON_MovieReader
{
    public string status;
    public int code;
    public MovieData data;
}
[Serializable]
public class MovieData
{
    public string img;
    public int nextId;
    public int fullView;
    public string downUrl;
    public int viewType;
    public int collected;
    public int id;
    public string title;
    public Definitions[] definitions;
    public string playUrl;
    public int definitionId;
}
[Serializable]
public class Definitions
{
    public string memberTitle;
    public int memberCoins;
    public int id;
    public string title;
    public int memberId;
}

