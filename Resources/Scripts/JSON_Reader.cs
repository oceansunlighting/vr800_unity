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
public class JSON_Reader 
{
    public string status;
    public int code;
    public Data data;
}
[Serializable]
public class Data
{
    public ChannelList[] channelList;
    public VideoList[] videoList;
    public int totalPages;
    public int totalElements;
}
[Serializable]
public class ChannelList
{
    public int id;
    public string title;
    public string img;
}
[Serializable]
public class VideoList
{
    public int id;
    public string title;
    public string poster;
    public string note;

    public int createTime;
    public int playCount;
    public string img;
}

