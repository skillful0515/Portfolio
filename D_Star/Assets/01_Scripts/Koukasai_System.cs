using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;
using System;
using UnityEngine.UI;
using System.IO;

public class Koukasai_System : MonoBehaviour
{
    public GameObject prefabWall;
    public GameObject originWall;
    public Transform field;
    private int sizeField;

    private Vector3 insPos;

    private Wall[,] walls;

    private Wall goalCell;
    private Wall startCell;

    public List<Route> routes = new List<Route>();

    private int maxId;

    public Text numText;
    public Text numTextTrue;
    private int num = -1;
    private int befNum = -1;

    public GameObject agent;

    // Use this for initialization
    void Start()
    {
        sizeField = (int)field.localScale.x * 10 + 1;
        walls = new Wall[sizeField, sizeField];

        actMaking();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            var startTime = DateTime.Now;
            A_Star();
            var endTime = DateTime.Now;

            double time = (endTime - startTime).TotalMilliseconds;
            //Debug.Log("A*.Time = " + time + "ms");
            // テキストファイルにA*の時間を追加
            textSave("" + time, "01_astarLog.txt");
        }

        // 最短経路情報を取得
        if (Input.GetKeyDown(KeyCode.I))
        {
            ScanShortestRoute();
        }

        // 最短経路を各マスに格納
        if (Input.GetKeyDown(KeyCode.O))
        {
            InputtingRoutes();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            displayPickUpRoute();
        }

        // 紅華祭用処理
        if (Input.GetKeyDown(KeyCode.K))
        {
            for (int i = 0; i < routes.Count; i++)
            {
                GameObject obj = Instantiate(agent);
                obj.GetComponent<KoukasaiAgent>().SetRoutes(routes[i]);
            }
        }

        // 表示経路リスト用のテキスト処理
        changeNumText();
    }

    public void ScanShortestRoute()
    {
        // リセットしてから計測
        for (int i = routes.Count - 1; i >= 0; i--)
        {
            routes.RemoveAt(i);
        }

        var startTime = DateTime.Now;

        Route route = new Route();
        //route.AddElement(walls[startCell.Position_X, startCell.Position_Y]);
        SerchRoutes(startCell.Position_X, startCell.Position_Y, startCell.Id, route);

        var endTime = DateTime.Now;

        Debug.Log("Scanning.Time = " + (endTime - startTime).TotalMilliseconds + "ms");
        Debug.Log("routeList.Count = " + routes.Count);
    }

    public void InputtingRoutes()
    {
        if (routes.Count == 0)
        {
            Debug.Log("Please Scanning Routes !!! (press 'O' key)");
            return;
        }

        var startTime = DateTime.Now;

        for (int i = 0; i < sizeField; i++)
        {
            for (int j = 0; j < sizeField; j++)
            {
                walls[j, i].clearRoutes();
            }
        }

        for (int i = 0; i < sizeField; i++)
        {
            for (int j = 0; j < sizeField; j++)
            {
                for (int k = 0; k < routes.Count; k++)
                {
                    for (int l = 0; l < routes[k].GetElement.Count; l++)
                    {
                        //inputtingImage.fillAmount
                        //    = (i * sizeField + j * sizeField + k * routes.Count + l)
                        //    / (sizeField * sizeField * routes.Count * routes[k].GetElement.Count);

                        if (walls[j, i].name == routes[k].GetElement[l].name)
                        {
                            walls[j, i].addingRoutes(routes[k]);
                            break;
                        }
                    }
                }
            }
        }

        var endTime = DateTime.Now;

        Debug.Log("ShortestList.Time = " + (endTime - startTime).TotalMilliseconds + "ms");
    }

    // 一番最初に呼び出される生成メソッド
    void actMaking()
    {
        // 壁を敷き詰める
        for (int i = 0; i < sizeField; i++)
        {
            for (int j = 0; j < sizeField; j++)
            {
                New_System ns = originWall.GetComponent<Wall>().newSystem;

                insPos = originWall.transform.position;

                insPos.x += j;
                insPos.z -= i;

                GameObject wallObj = Instantiate(prefabWall, insPos, Quaternion.identity);
                wallObj.name = "wall[" + j + ", " + i + "]";
                walls[j, i] = wallObj.GetComponent<Wall>();
                // 位置情報を付与
                walls[j, i].Position_X = j;
                walls[j, i].Position_Y = i;
                walls[j, i].newSystem = ns;

                // 設置した壁のうち、内側の壁をFreeにし、
                // 外側の壁を変更不可のProtectWallにする。
                if ((i != 0 && i != sizeField - 1)
                 && (j != 0 && j != sizeField - 1))
                {
                    wallObj.GetComponent<Wall>().ChangeState(Wall.StateCell.Free);
                }
                else
                {
                    wallObj.GetComponent<Wall>().ChangeState(Wall.StateCell.ProtectWall);
                }
            }
        }

        // 原点として使用した壁は削除
        Destroy(originWall);
    }

    void A_Star()
    {
        // 全てのチェックを外し、IDをゼロにする
        for (int i = 0; i < sizeField; i++)
        {
            for (int j = 0; j < sizeField; j++)
            {
                walls[j, i].IsChecked = false;
                walls[j, i].Id = 0;
                walls[j, i].ChangeArrow(Wall.StateArrows.None);
                walls[j, i].ResetRelation();
                walls[j, i].IsChanged = false;
            }
        }

        maxId = 0;

        // ゴールを見つける
        for (int i = 0; i < sizeField; i++)
        {
            for (int j = 0; j < sizeField; j++)
            {
                if (walls[j, i].GetState == Wall.StateCell.Goal)
                {
                    goalCell = walls[j, i];
                    goalCell.Position_X = j;
                    goalCell.Position_Y = i;
                    goalCell.IsChecked = true;
                }
            }
        }

        // 同様にスタートを見つける
        for (int i = 0; i < sizeField; i++)
        {
            for (int j = 0; j < sizeField; j++)
            {
                if (walls[j, i].GetState == Wall.StateCell.Start)
                {
                    startCell = walls[j, i];
                }
            }
        }

        // ゴールの上下左右のセルに矢印情報を与え、IDを1にする
        if (walls[goalCell.Position_X + 1, goalCell.Position_Y].BeAbleChange == true)
        {
            walls[goalCell.Position_X + 1, goalCell.Position_Y].ChangeArrow(Wall.StateArrows.Right);
            walls[goalCell.Position_X + 1, goalCell.Position_Y].Id = 1;
            walls[goalCell.Position_X + 1, goalCell.Position_Y].IsChecked = true;
            walls[goalCell.Position_X + 1, goalCell.Position_Y].AddingRelation(goalCell);
        }

        if (walls[goalCell.Position_X - 1, goalCell.Position_Y].BeAbleChange == true)
        {
            walls[goalCell.Position_X - 1, goalCell.Position_Y].ChangeArrow(Wall.StateArrows.Left);
            walls[goalCell.Position_X - 1, goalCell.Position_Y].Id = 1;
            walls[goalCell.Position_X - 1, goalCell.Position_Y].IsChecked = true;
            walls[goalCell.Position_X - 1, goalCell.Position_Y].AddingRelation(goalCell);
        }

        if (walls[goalCell.Position_X, goalCell.Position_Y + 1].BeAbleChange == true)
        {
            walls[goalCell.Position_X, goalCell.Position_Y + 1].ChangeArrow(Wall.StateArrows.Down);
            walls[goalCell.Position_X, goalCell.Position_Y + 1].Id = 1;
            walls[goalCell.Position_X, goalCell.Position_Y + 1].IsChecked = true;
            walls[goalCell.Position_X, goalCell.Position_Y + 1].AddingRelation(goalCell);
        }

        if (walls[goalCell.Position_X, goalCell.Position_Y - 1].BeAbleChange == true)
        {
            walls[goalCell.Position_X, goalCell.Position_Y - 1].ChangeArrow(Wall.StateArrows.Up);
            walls[goalCell.Position_X, goalCell.Position_Y - 1].Id = 1;
            walls[goalCell.Position_X, goalCell.Position_Y - 1].IsChecked = true;
            walls[goalCell.Position_X, goalCell.Position_Y - 1].AddingRelation(goalCell);
        }

        // 全てをチェック
        for (int s = 1; startCell.Id == 0 && s < sizeField * sizeField; s++)
        {
            for (int i = 1; i < sizeField - 1; i++)
            {
                for (int j = 1; j < sizeField - 1; j++)
                {
                    if (walls[j, i].Id == s)
                    {
                        if (walls[j + 1, i].IsChecked == false && walls[j + 1, i].BeAbleChange == true)
                        {
                            walls[j + 1, i].ChangeArrow(Wall.StateArrows.Right);
                            walls[j + 1, i].Id = s + 1;
                            if (s + 1 > maxId) maxId = s + 1;
                            walls[j + 1, i].IsChecked = true;
                            walls[j + 1, i].AddingRelation(walls[j, i]);
                            for (int w = 0; w < walls[j, i].GetRelationCount; w++)
                            {
                                walls[j + 1, i].AddingRelation(walls[j, i].GetRelationWall(w));
                            }
                        }
                        if (walls[j - 1, i].IsChecked == false && walls[j - 1, i].BeAbleChange == true)
                        {
                            walls[j - 1, i].ChangeArrow(Wall.StateArrows.Left);
                            walls[j - 1, i].Id = s + 1;
                            if (s + 1 > maxId) maxId = s + 1;
                            walls[j - 1, i].IsChecked = true;
                            walls[j - 1, i].AddingRelation(walls[j, i]);
                            for (int w = 0; w < walls[j, i].GetRelationCount; w++)
                            {
                                walls[j - 1, i].AddingRelation(walls[j, i].GetRelationWall(w));
                            }
                        }
                        if (walls[j, i + 1].IsChecked == false && walls[j, i + 1].BeAbleChange == true)
                        {
                            walls[j, i + 1].ChangeArrow(Wall.StateArrows.Down);
                            walls[j, i + 1].Id = s + 1;
                            if (s + 1 > maxId) maxId = s + 1;
                            walls[j, i + 1].IsChecked = true;
                            walls[j, i + 1].AddingRelation(walls[j, i]);
                            for (int w = 0; w < walls[j, i].GetRelationCount; w++)
                            {
                                walls[j, i + 1].AddingRelation(walls[j, i].GetRelationWall(w));
                            }
                        }
                        if (walls[j, i - 1].IsChecked == false && walls[j, i - 1].BeAbleChange == true)
                        {
                            walls[j, i - 1].ChangeArrow(Wall.StateArrows.Up);
                            walls[j, i - 1].Id = s + 1;
                            if (s + 1 > maxId) maxId = s + 1;
                            walls[j, i - 1].IsChecked = true;
                            walls[j, i - 1].AddingRelation(walls[j, i]);
                            for (int w = 0; w < walls[j, i].GetRelationCount; w++)
                            {
                                walls[j, i - 1].AddingRelation(walls[j, i].GetRelationWall(w));
                            }
                        }
                    }
                }
            }
        }
    }

    // 最短経路の経路情報を取得
    // 今回はスタート地点から計測する
    void SerchRoutes(int _x, int _y, int _id, Route _route)
    {
        if ((_x >= 0 && _x < sizeField && _y >= 0 && _y < sizeField) == false)
        {
            return;
        }

        //Debug.Log("StartSerch");
        Route route = new Route();
        for (int i = 0; i < _route.GetElement.Count; i++)
        {
            route.AddElement(_route.GetElement[i]);
        }

        route.AddElement(walls[_x, _y]);

        if (_x == goalCell.Position_X && _y == goalCell.Position_Y)
        {
            //Debug.Log("id = 0");

            // スタートマスを含んでいれば追加
            for (int i = 0; i < route.GetElement.Count; i++)
            {
                if (route.GetElement[i].GetState == Wall.StateCell.Start)
                {
                    routes.Add(route);
                    //Debug.Log("EndSerch");
                    if (routes.Count % 100 == 0)
                    {
                        Debug.Log("routes.Count = " + routes.Count);
                    }
                    break;
                }
            }

            return;
        }

        if (walls[_x + 1, _y].Id == _id - 1 && walls[_x + 1, _y].BeAbleToRoute)
        {
            //Debug.Log("Adding_[" + (_x + 1) + ", " + _y + "]");
            SerchRoutes(_x + 1, _y, _id - 1, route);
        }
        if (walls[_x - 1, _y].Id == _id - 1 && walls[_x - 1, _y].BeAbleToRoute)
        {
            //Debug.Log("Adding_[" + (_x - 1) + ", " + _y + "]");
            SerchRoutes(_x - 1, _y, _id - 1, route);
        }
        if (walls[_x, _y + 1].Id == _id - 1 && walls[_x, _y + 1].BeAbleToRoute)
        {
            //Debug.Log("Adding_[" + _x + ", " + (_y + 1) + "]");
            SerchRoutes(_x, _y + 1, _id - 1, route);
        }
        if (walls[_x, _y - 1].Id == _id - 1 && walls[_x, _y - 1].BeAbleToRoute)
        {
            //Debug.Log("Adding_[" + _x + ", " + (_y - 1) + "]");
            SerchRoutes(_x, _y - 1, _id - 1, route);
        }
    }

    void changeNumText()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            int.TryParse(numText.text, out num);
            numTextTrue.text = numText.text;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) || (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.UpArrow)))
        {
            num++;
            if (num >= routes.Count)
            {
                num = routes.Count - 1;
            }
            numTextTrue.text = "" + num;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) || (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.DownArrow)))
        {
            num--;
            if (num < 0)
            {
                num = 0;
            }
            numTextTrue.text = "" + num;
        }

        if (num >= 0 && num < routes.Count && num != befNum)
        {
            // 全ての移動空間の色をリセット
            for (int i = 0; i < sizeField; i++)
            {
                for (int j = 0; j < sizeField; j++)
                {
                    if (walls[j, i].GetState == Wall.StateCell.Free)
                    {
                        //walls[j, i].changeIdTextDef();
                        walls[j, i].changeArrowMaterialDefault();
                        //walls[j, i].changeMaterialDefault();
                    }
                }
            }

            // 指定のリストの移動経路を色付け
            for (int i = 0; i < routes[num].elementsWall.Count; i++)
            {
                //routes[num].elementsWall[i].changeIdText();
                //routes[num].elementsWall[i].changeArrowMaterialColor();
                //routes[num].elementsWall[i].changeMaterialColor();
                //Debug.Log("walls[" + routes[num].elementsWall[i].Position_X + ", " + routes[num].elementsWall[i].Position_Y + "]");

                // 矢印の向きを修正
                if (i < routes[num].elementsWall.Count - 1)
                {
                    if (routes[num].elementsWall[i].Position_X + 1 == routes[num].elementsWall[i + 1].Position_X)
                    {
                        routes[num].elementsWall[i].ChangeArrow(Wall.StateArrows.Left);
                    }
                    else if (routes[num].elementsWall[i].Position_X - 1 == routes[num].elementsWall[i + 1].Position_X)
                    {
                        routes[num].elementsWall[i].ChangeArrow(Wall.StateArrows.Right);
                    }
                    else if (routes[num].elementsWall[i].Position_Y + 1 == routes[num].elementsWall[i + 1].Position_Y)
                    {
                        routes[num].elementsWall[i].ChangeArrow(Wall.StateArrows.Up);
                    }
                    else if (routes[num].elementsWall[i].Position_Y - 1 == routes[num].elementsWall[i + 1].Position_Y)
                    {
                        routes[num].elementsWall[i].ChangeArrow(Wall.StateArrows.Down);
                    }
                }
            }
        }

        befNum = num;
    }

    public void displayPickUpRoute()
    {
        // 全ての移動空間の色をリセット
        for (int i = 0; i < sizeField; i++)
        {
            for (int j = 0; j < sizeField; j++)
            {
                if (walls[j, i].GetState == Wall.StateCell.Free)
                {
                    //walls[j, i].changeIdTextDef();
                    walls[j, i].changeArrowMaterialDefault();
                    //walls[j, i].changeMaterialDefault();
                }
            }
        }

        // 指定のリストの移動経路を色付け
        for (int i = 0; i < routes[0].elementsWall.Count; i++)
        {
            // 矢印の向きを修正
            if (i < routes[0].elementsWall.Count - 1)
            {
                if (routes[0].elementsWall[i].Position_X + 1 == routes[0].elementsWall[i + 1].Position_X)
                {
                    routes[0].elementsWall[i].ChangeArrow(Wall.StateArrows.Left);
                }
                else if (routes[0].elementsWall[i].Position_X - 1 == routes[0].elementsWall[i + 1].Position_X)
                {
                    routes[0].elementsWall[i].ChangeArrow(Wall.StateArrows.Right);
                }
                else if (routes[0].elementsWall[i].Position_Y + 1 == routes[0].elementsWall[i + 1].Position_Y)
                {
                    routes[0].elementsWall[i].ChangeArrow(Wall.StateArrows.Up);
                }
                else if (routes[0].elementsWall[i].Position_Y - 1 == routes[0].elementsWall[i + 1].Position_Y)
                {
                    routes[0].elementsWall[i].ChangeArrow(Wall.StateArrows.Down);
                }
            }
        }

        Debug.Log("routes.Count = " + routes.Count);
    }

    public void textSave(string _txt, string _fileName)
    {
        StreamWriter sw = new StreamWriter(_fileName, true);
        sw.WriteLine(_txt);
        sw.Flush();
        sw.Close();
    }
}