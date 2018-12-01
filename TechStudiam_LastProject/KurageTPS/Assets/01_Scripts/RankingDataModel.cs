using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniJSON;

public class RankingDataModel
{

    static public List<RankingData> DesirializeFromJson(string sStrJson)
    {
        List<RankingData> ret = new List<RankingData>();
        RankingData tmp = null;

        IList jsonList = (IList)Json.Deserialize(sStrJson);

        foreach (IDictionary jsonOne in jsonList)
        {
            tmp = new RankingData();

            if (jsonOne.Contains("Name"))
            {
                tmp.Name = (string)jsonOne["Name"];
            }
            if (jsonOne.Contains("Score"))
            {
                string str = jsonOne["Score"].ToString();
                int i = int.Parse(str);
                tmp.Score = i;
            }
            if (jsonOne.Contains("Date"))
            {
                tmp.Date = (string)jsonOne["Date"];
            }

            // 現レコード解析終了
            ret.Add(tmp);
            tmp = null;
        }
        return ret;
    }
}
