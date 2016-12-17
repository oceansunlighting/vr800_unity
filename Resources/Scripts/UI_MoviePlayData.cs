using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using RE = System.Text.RegularExpressions.Regex;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using System.Security.Cryptography;

namespace VRStandardAssets.Utils
{
    public class UI_MoviePlayData
    {
        public static string JsonStr = "";
        public static string GetHTTP(int MovieId)
        {
            string MainPartId = "id=" + MovieId;
            string MainPartdevice = "device=GearVR";
            string CurrentUrl = "http://api.vr800.com/vr/video/play?" + MainPartdevice + "&" + MainPartId;

            HttpWebRequest request = WebRequest.Create(new Uri(CurrentUrl)) as HttpWebRequest;
            request.Method = "GET";
            request.Headers["access_key"] = "doubo_user_key";

            HMACSHA1 hmacsha1 = new HMACSHA1(Encoding.UTF8.GetBytes("doubo_user_secret"));
            byte[] dataBuffer = Encoding.UTF8.GetBytes(MainPartdevice+MainPartId);

            //CryptoStream cs = new CryptoStream(Stream.Null, hmacsha1, CryptoStreamMode.Write);
            //cs.Write(dataBuffer, 0, dataBuffer.Length);
            //cs.Close();
            byte[] hashBytes = hmacsha1.ComputeHash(dataBuffer);
            string str = BaseToHexString(hmacsha1.Hash);
            //Debug.Log(str);
            string ContentDe = Convert.ToBase64String(Encoding.UTF8.GetBytes(str));
            //Debug.Log(ContentDe);
            request.Headers["signature"] = ContentDe;
            //注释解析：
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            Dictionary<string, string> HeaderList = new Dictionary<string, string>();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();
            response.Close();
            return retString;
        }

        public static string BaseToHexString(byte[] hex)
        {
            if (hex == null) return null;
            if (hex.Length == 0) return string.Empty;

            string s = "";
            foreach (byte b in hex)
            {
                s += b.ToString("x2");
            }
            return s.ToString();
        }

        public static void SendData(int MovieId)
        {
            // channelId =0 ; 默认选中的是第一个，page =0 默认选中的是第一页；  分类选项里面的。
            //播放解析：string CurrentUrl = "http://api.test.vr800.com/vr/video/play?id=1000&device=GearVR";
            //注释解析：{"status":"success","code":0,"data":{"img":"http://dl.vr800.com/vr_video/1464662515331.jpg","nextId":1001,"fullView":1,"downUrl":"http://doubo.8686c.com/tran/ff808081557be3ad01557be44a580169/3.mp4?wsSecret=2c9479c9832fdf4ae6bc3b1b2b32db86&wsTime=582d7cb7","viewType":5,"collected":0,"id":1000,"title":"大势Bigbang现场帅气嗨翻天","definitions":[{"memberTitle":"","memberCoins":0,"id":1,"title":"标清","memberId":0},{"memberTitle":"","memberCoins":0,"id":2,"title":"高清","memberId":0},{"memberTitle":"","memberCoins":0,"id":3,"title":"超清","memberId":0}],"playUrl":"http://doubo.8686c.com/tran/ff808081557be3ad01557be44a580169/3.mp4?wsSecret=2c9479c9832fdf4ae6bc3b1b2b32db86&wsTime=582d7cb7","definitionId":3}}
            //fullView：1为全景    0为非全景，viewType：1:左右360   2:上下360   3:左右180    4:上下180   5:全屏360   6:全屏180
            //{"status":"success","code":0,"data":{"channelList":[{"id":10,"title":"美女","img":"http://static.cdn.doubo.tv/null"},{"id":9,"title":"影视","img":"http://static.cdn.doubo.tv/null"},{"id":11,"title":"原创","img":"http://static.cdn.doubo.tv/null"},{"id":4,"title":"惊悚","img":"http://static.cdn.doubo.tv/null"},{"id":2,"title":"极限","img":"http://static.cdn.doubo.tv/null"},{"id":8,"title":"游戏","img":"http://static.cdn.doubo.tv/null"},{"id":3,"title":"旅行","img":"http://static.cdn.doubo.tv/null"},{"id":12,"title":"科技","img":"http://static.cdn.doubo.tv/null"},{"id":6,"title":"娱乐","img":"http://static.cdn.doubo.tv/null"}],"videoList":[{"id":718,"title":"甜心萌妹BongBong","poster":"http://dl.vr800.com/video/1463451176688.jpg","note":"","createTime":0,"playCount":0,"img":"http://dl.vr800.com/video/1463451176688.jpg"},{"id":1783,"title":"美女与黑丝","poster":"http://dl.vr800.com/video/1471511144278.jpg","note":"","createTime":0,"playCount":0,"img":"http://dl.vr800.com/video/1471511144278.jpg"},{"id":1788,"title":"性感足球宝贝李效娇","poster":"http://dl.vr800.com/video/1471595642775.jpg","note":"","createTime":0,"playCount":0,"img":"http://dl.vr800.com/video/1471595642775.jpg"},{"id":1791,"title":"嫩模李宓儿","poster":"http://dl.vr800.com/video/1471831952507.jpg","note":"","createTime":0,"playCount":0,"img":"http://dl.vr800.com/video/1471831952507.jpg"},{"id":1796,"title":"嫩模李宓儿之三","poster":"http://dl.vr800.com/video/1471938230530.jpg","note":"","createTime":0,"playCount":0,"img":"http://dl.vr800.com/video/1471938230530.jpg"},{"id":2042,"title":"粉嫩比基尼的泳池YY","poster":"http://dl.vr800.com/video/1477386018090.jpg","note":"","createTime":0,"playCount":0,"img":"http://dl.vr800.com/video/1477386018090.jpg"},{"id":1957,"title":"制服诱惑","poster":"http://dl.vr800.com/video/1476154945599.jpg","note":"","createTime":0,"playCount":0,"img":"http://dl.vr800.com/video/1476154945599.jpg"},{"id":2134,"title":"美女教你穿丝袜","poster":"http://dl.vr800.com/video/1477566421692.png","note":"","createTime":0,"playCount":0,"img":"http://dl.vr800.com/video/1477566421692.png"},{"id":2133,"title":"AKB48封面playboy拍摄","poster":"http://dl.vr800.com/video/1477565952980.jpg","note":"","createTime":0,"playCount":0,"img":"http://dl.vr800.com/video/1477565952980.jpg"},{"id":2132,"title":"E杯&护士服鼻血hold不住","poster":"http://dl.vr800.com/video/1477564695748.png","note":"","createTime":0,"playCount":0,"img":"http://dl.vr800.com/video/1477564695748.png"},{"id":2131,"title":"衣服上什么小精灵","poster":"http://dl.vr800.com/video/1477564257272.png","note":"","createTime":0,"playCount":0,"img":"http://dl.vr800.com/video/1477564257272.png"},{"id":2130,"title":"衣服真的是太多余啦","poster":"http://dl.vr800.com/video/1477563931098.png","note":"","createTime":0,"playCount":0,"img":"http://dl.vr800.com/video/1477563931098.png"}],"totalPages":25,"totalElements":292}}
            //{"status":"success","code":0,"data":{"img":"http://dl.vr800.com/video/1477473187495.png","nextId":2092,"fullView":0,"downUrl":"http://doubo.8686c.com/tran/ff80808157ffe42d01580043b876000c/1.mp4?wsSecret=17a3d07f89a6dba605632af69146090b&wsTime=5834fd02","viewType":5,"collected":0,"id":2091,"title":"纤腰翘臀超性感MV","definitions":[{"memberTitle":"","memberCoins":0,"id":1,"title":"标清","memberId":0}],"playUrl":"http://doubo.8686c.com/tran/ff80808157ffe42d01580043b876000c/1.mp4?wsSecret=17a3d07f89a6dba605632af69146090b&wsTime=5834fd02","definitionId":1}}
            JsonStr = GetHTTP(MovieId);
            ViewMenuMgr.f_instance.SetMoviePlay();
            //ViewMenuMgr.f_instance.ReceiveData();
        }

    }
}
