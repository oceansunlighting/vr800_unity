using UnityEngine;
using System.Collections;
using System;
using System.IO;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Runtime.Remoting.Contexts;
using System.Net;
using System.Text.RegularExpressions;
using System.Text;
using System.Runtime.InteropServices;

namespace VRStandardAssets.Utils
{
    enum LoadType
    {
        SetLoadData =0,
        GetLoadData =1,
    }
    enum SetDataType
    {
        DownMovieJPG = 0,
        PlayingMovie = 1,
    }
    public class LocalDataMgr : MonoBehaviour
    {
        private static LocalDataMgr m_instance;

        private List<string> MovieUrllst = new List<string>();
        private List<string> MovieNamelst = new List<string>();
        private List<string> JPGUrllst = new List<string>();
        private List<string> CanRemovelst = new List<string>();
        private List<GameObject> LocalDatalst = new List<GameObject>();
        private List<GameObject> LocalDataPage = new List<GameObject>();

		public GameObject go_Text;
        private byte[] DataByte = new byte[0];
        private bool b_beginGetData = false;
        private bool GetResearch = false;
        private bool b_GetMovie = false;
        private FileStream pFileStream = null;
        private float m_fLastTime = -99f;
        string TextDec = "";
        private WWW wwwTotal;
        void Awake()
        {

            if (m_instance == null)
            {
                m_instance = this;
            }
        }
        // Use this for initialization
        void Start()
        {

        }

        void OnEnable()
        {

        }
        
        // Update is called once per frame
        void Update()
        {
            if (Time.realtimeSinceStartup - m_fLastTime >= 0.5f)
            {
                if (GetResearch)
                {
                    if (!b_beginGetData)
                    {
                        m_fLastTime = Time.realtimeSinceStartup;
                        if (CanRemovelst.Count >= 1)
                        {
                            GetData();   
                        }
                        else
                        {
                            if (b_GetMovie)
                            {
                                GetMovie();
                            }

                        }
                    }
                }
            }
        }
        void OnDisable()
        {
           
        }
        public void GetMovie()
        {    
            GetResearch = false;
            b_GetMovie = false;
            ViewMenuMgr.f_instance.go_select.SetActive(true);
            List<string> LoadUrl = MovieUrllst;
            for (int i =0;i<LoadUrl.Count;i++)
            {
                //加载数据......;
                StartCoroutine(MoviePicture(LoadUrl[i],i));
            }
            //更新页面
			go_Text.SetActive(false);
            ViewMenuMgr.f_instance.SetCurrentLocalData();
        }
        public void GetData()
        {    
            //访问开始......;
            b_beginGetData = true;
            GetResearch = false;
            List<string> LoadUrl = CanRemovelst;
            foreach (string url in LoadUrl)
            {
                //加载数据......;
                StartCoroutine(FileBack(url));
            }
            //访问结束......;
            b_beginGetData = false;
            GetResearch = true;
        }
        IEnumerator FileBack(string url)
        {
            //访问“可支持的android” 系统路径； 等待加载数据......;          
            WWW www = new WWW(url);
            yield return www;
            if (www.isDone && www.error == null)
            {
                //访问路径......;
                GetCurrent(www.data, www);
            }
            if (www.isDone && www.error != null)
            {
                CanRemovelst.Remove(www.url);
            }     
        }
        public void GetCurrent(string Data,WWW www)
        {
            List<string> ChildTypeName = new List<string>();
            List<string> ChildDeleteUrl = new List<string>();
            string str = Data;
            if (CanRemovelst.Contains(www.url))
            {
                CanRemovelst.Remove(www.url);
            }
            if (str.Contains("<br>"))
            {
                string[] note = Regex.Split(str, "<br>", RegexOptions.IgnoreCase);
                foreach (string strLoat in note)
                {
                    ChildTypeName.Add(strLoat.Trim());
                }
                foreach (string Name in ChildTypeName)
                {
                    if (Name.Contains("hr"))
                    {
                        string UrlChar = Name;
                        ChildDeleteUrl.Add(Name);
                        string[] UrlTest = Regex.Split(UrlChar, "<hr>", RegexOptions.IgnoreCase);
                        foreach (string NameChild in UrlTest)
                        {
                            if (!NameChild.Contains("base"))
                            {
                                if (NameChild != null && NameChild != "")
                                {
                                    CanRemovelst.Add(www.url.Trim()+"/".Trim()+NameChild.Trim());
                                }
                            }
                        }
                    }
                }
                foreach (string Name in ChildTypeName)
                {
                    if (Name.Contains(".mp4"))
                    {
                        string Url = www.url.Trim()+"/".Trim()+Name.Trim();
                        MovieUrllst.Add(Url);
                        MovieNamelst.Add(Name.Trim());
                        ChildDeleteUrl.Add(Name);
                    }
                    if (Name.Contains(".jpg") || Name.Contains(".png"))
                    {
                        string Url = www.url.Trim()+"/".Trim()+Name.Trim();
                        JPGUrllst.Add(Url);
                        ChildDeleteUrl.Add(Name);
                    }
                }
                if (ChildTypeName.Count > 0)
                {
                    foreach (string Name in ChildDeleteUrl)
                    {
                        ChildTypeName.Remove(Name);
                    }
                    foreach (string Name in ChildTypeName)
                    {
                        if (Name.Contains("."))
                        {
                            ChildDeleteUrl.Add(Name);
                        }
                    }
                    foreach (string Name in ChildDeleteUrl)
                    {
                        ChildTypeName.Remove(Name);
                    }
                }
                if (ChildTypeName.Count > 0)
                {
                    foreach (string Name in ChildTypeName)
                    {
                        if (Name != null && Name != "")
                        {
                            string UrlOne = "/".Trim() + Name.Trim();
                            string Urltwo = www.url.Trim();
                            string Url = Urltwo + UrlOne;
                            CanRemovelst.Add(Url.Trim());
                        }
                    }
                }
            }
        }
        //IEnumerator FileGetBack(byte[] NoteByte, LoadType TypeCurrent)
        //{
        //    bool bByte = false;
        //    string Name = "CameraFile";
        //    string path = Application.persistentDataPath + " / Data";
        //    DataByte = NoteByte;
        //    switch (TypeCurrent)
        //    {
        //        case LoadType.SetLoadData:
        //            {
        //                TextDec += "哈哈";
        //                if (System.IO.File.Exists(path) == false)
        //                {
        //                    System.IO.File.Create(path);
        //                    TextDec += "切切";
        //                    StartCoroutine(writeFile(DataByte, path));
        //                    //File.WriteAllBytes(Application.persistentDataPath + "/Data", DataByte);
        //                    yield return null;
        //                }
        //                else
        //                {
        //                    //等待删除数据...
        //                    StartCoroutine(DeleteData(path));
        //                    //重新创建
        //                    System.IO.File.Create(path);
        //                    StartCoroutine(writeFile(DataByte, path));
        //                    yield return null;
        //                }
        //                break;
        //            }
        //        case LoadType.GetLoadData:
        //            {
        //                TextDec += "啦啦";
        //                //StartCoroutine(IsAllowedExtension(Application.persistentDataPath + "/Data"));
        //                DirectoryInfo TheFolder = new DirectoryInfo(path);
        //                foreach (FileInfo NextFile in TheFolder.GetFiles())
        //                {
        //                    TextDec += "呀呀";
        //                    TextDec += NextFile.Name + "nn";
        //                    //listAllType.Add(NextFile.Name);
        //                    Debug.Log(NextFile.Name);
        //                }
        //                yield return null;
        //                break;
        //            }
        //    }
        //    //foreach (DirectoryInfo MenuPath in TheFolder.GetDirectories())
        //    //{
        //    //    TextDec += MenuPath.Name + "dd";
        //    //}
        //    // //foreach (DirectoryInfo Note in TheFolder.GetDirectories())
        //    //{
        //    //    TextDec += Note.Name + "dd";
        //    //}
        //    bByte = true;
        //    TextDec += bByte;
        //   yield return null; 
        //}
        //IEnumerator DeleteData(string path)
        //{
        //    System.IO.File.Delete(path);
        //    yield return null;
        //}
        //IEnumerator IsAllowedExtension(string filePath)
        //{
        //    FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        //    BinaryReader reader = new BinaryReader(stream);
        //    string fileclass = "";
        //    // byte buffer;
        //    try
        //    {
        //        //buffer = reader.ReadByte();
        //        //fileclass = buffer.ToString();
        //        //buffer = reader.ReadByte();
        //        //fileclass += buffer.ToString();

        //        for (int i = 0; i < 2; i++)
        //        {
        //            fileclass += reader.ReadByte().ToString();
        //        }

        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }

        //    if (fileclass == "255216")
        //    {
        //        yield return true;
        //    }
        //    else
        //    {
        //        yield return false;
        //    }

        //    /*文件扩展名说明
        //     * 255216 jpg
        //     * 208207 doc xls ppt wps
        //     * 8075 docx pptx xlsx zip
        //     * 5150 txt
        //     * 8297 rar
        //     * 7790 exe
        //     * 3780 pdf      
        //     * 
        //     * 4946/104116 txt
        //     * 7173        gif 
        //     * 255216      jpg
        //     * 13780       png
        //     * 6677        bmp
        //     * 239187      txt,aspx,asp,sql
        //     * 208207      xls.doc.ppt
        //     * 6063        xml
        //     * 6033        htm,html
        //     * 4742        js
        //     * 8075        xlsx,zip,pptx,mmap,zip
        //     * 8297        rar   
        //     * 01          accdb,mdb
        //     * 7790        exe,dll
        //     * 5666        psd 
        //     * 255254      rdp 
        //     * 10056       bt种子 
        //     * 64101       bat 
        //     * 4059        sgf    
        //     */
        //}
        //IEnumerator ReadFile(string fileName)

        //{
        //    FileStream pFileStream = null;
        //    byte[] pReadByte = new byte[0];
        //    try
        //    {
        //        pFileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
        //        BinaryReader r = new BinaryReader(pFileStream);
        //        r.BaseStream.Seek(0, SeekOrigin.Begin);    //将文件指针设置到文件开  
        //        pReadByte = r.ReadBytes((int)r.BaseStream.Length);               
        //    }
        //    catch
        //    {               
        //    }
        //    finally
        //    {
        //        if (pFileStream != null)
        //            pFileStream.Close();
        //    }
        //    yield return pReadByte;
        //}
        //写byte[]到fileName  
        IEnumerator writeFile(byte[] pReadByte, string fileName)
        {
            pFileStream = null;
            try
            {
                pFileStream = new FileStream(fileName, FileMode.OpenOrCreate);
                pFileStream.Write(pReadByte, 0, pReadByte.Length);
            }
            catch
            {

            }
            finally
            {
                if (pFileStream != null)
                    pFileStream.Close();
            }
            yield return true;
        }
        public void SetPrefabData()
        {
            for (int i = 0; i < 1; i++)
            {
                if (LocalDataPage.Count - 1 < i)
                {
                    LocalDataPage.Add(ViewMenuMgr.f_instance.CreatMenuBaseLine("Prefab/VideoPage", ViewMenuMgr.f_instance.go_Pagetwo.transform));
                }
            }
            for (int i = 0; i < 12; i++)
            {
                RecorderMgr.f_instance.ClearMovieData(i, false);
            }
            ViewMenuMgr.f_instance.PrefabReset(false);
            if (LocalDatalst.Count <= 12)
            {
                for (int i = 0; i < 12; i++)
                {
                    if(i>LocalDatalst.Count-1)
                        LocalDatalst.Add(ViewMenuMgr.f_instance.CreatMenuBaseLine("Prefab/SelectionMovie", ViewMenuMgr.f_instance.go_LocalDatalst.transform));
                }
            }
            for (int i = 0; i < LocalDatalst.Count; i++)
            {
                ClearMovieData(i, false);
            }

            MovieUrllst.Clear();
            JPGUrllst.Clear();
            CanRemovelst.Clear();
            MovieNamelst.Clear();

            string UrlPathOne = "file:///storage/emulated/0/DCIM";
            string UrlPathtwo = "file:///storage/emulated/0/Download";
            string UrlPaththree = "file:///storage/emulated/0/Movies";
            //string UrlPathfour = "file:///storage/emulated/0/tencent";
            CanRemovelst.Add(UrlPathOne);
            CanRemovelst.Add(UrlPathtwo);
            CanRemovelst.Add(UrlPaththree);
            //CanRemovelst.Add(UrlPathfour);
            GetResearch = true;
            b_GetMovie = true;
            TextDec = "";
			go_Text.SetActive(true);
        }
        public void ClearMovieData(int i, bool Active)
        {
            if (LocalDatalst.Count - 1 >= i)
            {
                LocalDatalst[i].SetActive(true);
                LocalDatalst[i].GetComponent<BoxCollider>().enabled = Active;
                LocalDatalst[i].GetComponent<SpriteRenderer>().enabled = Active;
                LocalDatalst[i].GetComponent<SelectionMovie>().go_Scale.SetActive(Active);
                LocalDatalst[i].GetComponent<SelectionMovie>().go_MovieName.SetActive(Active);
                LocalDatalst[i].GetComponent<VRInteractiveItem>().go_Background.SetActive(Active);
            }
            ViewMenuMgr.f_instance.go_selecttwo.SetActive(Active);
            ViewMenuMgr.f_instance.go_Pagetwo.SetActive(Active);
            if (LocalDataMgr.f_instance.f_LocalDataPage.Count >= 1)
                LocalDataMgr.f_instance.f_LocalDataPage[0].SetActive(Active);
        }

        IEnumerator MoviePicture(string url,int i)
        {
            //url = url.Trim();
            //string write_path = Application.persistentDataPath + "/Data" + url.Substring(url.LastIndexOf("/"));
            //if (System.IO.File.Exists(write_path))
            //{
            //    System.IO.File.Delete(write_path);
            //}
            //if (System.IO.File.Exists(write_path) == false)
            //{
            WWW www = new WWW(url);
            yield return www;
            wwwTotal = www;
            if (MovieUrllst.Contains(www.url))
            {
                if (www.isDone && www.error == null)
                {
                    ClearMovieData(i, true);

                    Texture2D tex = www.texture;
                    StartCoroutine(SetImg(www.url, tex, i));

                    //if (string.IsNullOrEmpty(www.error))
                    //{
                    //    if (System.IO.Directory.Exists(Application.persistentDataPath + "/Data") == false)
                    //        System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/Data");
                    //    //System.IO.File.WriteAllBytes(write_path, www.bytes);
                    //}
                    //else
                    //{
                    //    Debug.Log(www.error);
                    //}
                    
                }

            }
        }

        IEnumerator SetImg(string url,Texture2D tex, int i)
        {
            Sprite sprite = new Sprite();
            sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0, 0), 100.0f);
            float Num1 = sprite.texture.width;
            float Num2 = sprite.texture.height;
            float NumX = scale(true, Num1, 0);
            float NumY = scale(true, 0, Num2);
            float ChildX = scale(false, Num1, 0);
            float ChildY = scale(false, 0, Num2);

            SelectionMovie m_SelectionMovie = LocalDatalst[i].GetComponent<SelectionMovie>();
            if(tex!=null)
            m_SelectionMovie.go_MovieName.GetComponent<Text>().text = MovieNamelst[i].ToString();

            m_SelectionMovie.f_MovieName = MovieNamelst[i].ToString();
            m_SelectionMovie.f_MovieUrl = url;
            LocalDatalst[i].GetComponent<SpriteRenderer>().sprite = sprite;
            LocalDatalst[i].GetComponent<RectTransform>().localScale = new Vector3(0.14f * NumX, 0.14f * NumY, 1f);

            LocalDatalst[i].GetComponent<VRInteractiveItem>().BackFx.GetComponent<RectTransform>().localScale = new Vector3(2 * ChildX, 2 * ChildY, 1f);
            LocalDatalst[i].GetComponent<VRInteractiveItem>().BackFx.GetComponent<RectTransform>().localPosition = new Vector3(3f * ChildX, 2.1f * ChildY, 1f);

            LocalDatalst[i].GetComponent<VRInteractiveItem>().go_Background.GetComponent<RectTransform>().localScale = new Vector3(2 * ChildX, 2 * ChildY, 1f);
            LocalDatalst[i].GetComponent<VRInteractiveItem>().go_Background.GetComponent<RectTransform>().localPosition = new Vector3(3f * ChildX, 2.1f * ChildY, 1f);

            LocalDatalst[i].GetComponent<BoxCollider>().size = new Vector3(2 * ChildX, 2 * ChildY, 1f);
            LocalDatalst[i].GetComponent<BoxCollider>().center = new Vector3(3f * ChildX, 2.1f * ChildY, 1f);
            LocalDatalst[i].GetComponent<SelectionMovie>().go_Scale.SetActive(false);
            LocalDatalst[i].SetActive(true);
            yield return true;
        }
        private float scale(bool SetData = false, float widthX = 0, float widthY = 0)
        {
            float Num3 = 0;
            float DataX = 750;
            float DataY = 420;
            if (SetData)
            {
                if (widthX != 0)
                {
                    Num3 = widthX / DataX;
                    float Num4 = 1 / Num3 * 100;
                    int Num5 = (int)Num4;
                    float Num6 = (float)Num5 / 100;
                    return Num6;
                }
                if (widthY != 0)
                {
                    Num3 = widthY / DataY;
                    float Num4 = 1 / Num3 * 100;
                    int Num5 = (int)Num4;
                    float Num6 = (float)Num5 / 100;
                    return Num6;
                }
            }
            else
            {
                if (widthX != 0)
                {
                    Num3 = widthX / DataX;
                    float Num4 = Num3 * 100;
                    int Num5 = (int)Num4;
                    float Num6 = (float)Num5 / 100;
                    return Num6;
                }
                if (widthY != 0)
                {
                    Num3 = widthY / DataY;
                    float Num4 = Num3 * 100;
                    int Num5 = (int)Num4;
                    float Num6 = (float)Num5 / 100;
                    return Num6;
                }
            }
            return Num3;
        }
        public List<GameObject> f_LocalDatalst
        {
            get
            {
                return LocalDatalst;
            }
        }
        public static LocalDataMgr f_instance
        {
            get
            {
                return m_instance;
            }
        }
        public List<string> f_MovieUrllst
        {
            get
            {
                return MovieUrllst;
            }
        }
        public List<GameObject> f_LocalDataPage
        {
            get
            {
                return LocalDataPage;
            }
        }
    }

}
