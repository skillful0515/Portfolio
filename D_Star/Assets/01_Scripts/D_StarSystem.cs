using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;
using System;
using System.IO;

public class D_StarSystem : MonoBehaviour
{
    public GameObject prefabWall;
    public GameObject originWall;
    public int sizeField;

    private Vector3 insPos;

    private Wall[,] walls;
    [SerializeField]
    private List<Wall> dStarList;

    private Wall goalCell;
    private Wall startCell;
    [SerializeField]
    private Wall changingCell;

    public int limitNum = 2;

    // Use this for initialization
    void Start()
    {
        walls = new Wall[sizeField, sizeField];
        dStarList = new List<Wall>();

        actMaking();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            D_Star();
            limitNum++;
        }
        if (Input.GetButtonDown("Fire1"))
        {
            limitNum = 0;
        }

        if (Input.GetButtonUp("Fire1"))
        {
            var startTime = DateTime.Now;
            Reload();
            var endTime = DateTime.Now;

            double time = (endTime - startTime).TotalMilliseconds;
            Debug.Log("D*.Time = " + time + "ms");
            textSave("" + time, "02_dstarLog.txt");
        }
    }

    // 一番最初に呼び出される生成メソッド
    void actMaking()
    {
        // 壁を敷き詰める
        for (int i = 0; i < sizeField; i++)
        {
            for (int j = 0; j < sizeField; j++)
            {
                insPos = originWall.transform.position;

                insPos.x += j;
                insPos.z -= i;

                GameObject wallObj = Instantiate(prefabWall, insPos, Quaternion.identity);
                wallObj.name = "wall[" + j + ", " + i + "]";
                walls[j, i] = wallObj.GetComponent<Wall>();
                // 位置情報を付与
                walls[j, i].Position_X = j;
                walls[j, i].Position_Y = i;

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

    void D_Star()
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

    void Reload()
    {
        //dStarList.Clear();
        for (int i = dStarList.Count - 1; i > 0; i--)
        {
            dStarList.RemoveAt(i);
        }


        // 変化の有ったセルを発見
        for (int i = 0; i < sizeField; i++)
        {
            for (int j = 0; j < sizeField; j++)
            {
                if (walls[j, i].IsChanged == true)
                {
                    changingCell = walls[j, i];
                }
            }
        }

        // 変化の有ったセルに影響を受けるセルを検索
        for (int i = 0; i < sizeField; i++)
        {
            for (int j = 0; j < sizeField; j++)
            {
                for (int w = 0; w < walls[j, i].GetRelationCount; w++)
                {
                    if (walls[j, i].GetRelationWall(w).IsChanged == true)
                    {
                        dStarList.Add(walls[j, i]);
                        walls[j, i].ChangeArrow(Wall.StateArrows.None);
                        walls[j, i].ChangeState(Wall.StateCell.Free);
                        Debug.Log("j, i = " + j + ", " + i);
                        break;
                    }
                }
            }
        }

        // 影響セルを並び替え ID昇順
        dStarList.Sort((a, b) => a.Id - b.Id);

        // 影響セルのIDを一度0にする
        for (int i = 0; i < dStarList.Count; i++)
        {
            dStarList[i].Id = 0;
        }
        startCell.Id = 0;

        // IDが最小だったセルから。影響セルにIDを振る
        for (int n = 0; n < dStarList.Count; n++)
        {
            Debug.Log(dStarList[n].name);

            int _x = dStarList[n].Position_X;
            int _y = dStarList[n].Position_Y;

            if (walls[_x + 1, _y].IsHavingArrow)
            {
                dStarList[n].ChangeArrow(Wall.StateArrows.Left);
                dStarList[n].Id = walls[_x + 1, _y].Id + 1;
                dStarList[n].IsChecked = true;
                walls[_x + 1, _y].AddingRelation(dStarList[n]);
                for (int w = 0; w < dStarList[n].GetRelationCount; w++)
                {
                    walls[_x + 1, _y].AddingRelation(dStarList[n].GetRelationWall(w));
                }
            }
            if (walls[_x - 1, _y].IsHavingArrow)
            {
                dStarList[n].ChangeArrow(Wall.StateArrows.Right);
                dStarList[n].Id = walls[_x - 1, _y].Id + 1;
                dStarList[n].IsChecked = true;
                walls[_x - 1, _y].AddingRelation(dStarList[n]);
                for (int w = 0; w < dStarList[n].GetRelationCount; w++)
                {
                    walls[_x - 1, _y].AddingRelation(dStarList[n].GetRelationWall(w));
                }
            }
            if (walls[_x, _y + 1].IsHavingArrow)
            {
                dStarList[n].ChangeArrow(Wall.StateArrows.Up);
                dStarList[n].Id = walls[_x, _y + 1].Id + 1;
                dStarList[n].IsChecked = true;
                walls[_x, _y + 1].AddingRelation(dStarList[n]);
                for (int w = 0; w < dStarList[n].GetRelationCount; w++)
                {
                    walls[_x, _y + 1].AddingRelation(dStarList[n].GetRelationWall(w));
                }
            }
            if (walls[_x, _y - 1].IsHavingArrow)
            {
                dStarList[n].ChangeArrow(Wall.StateArrows.Down);
                dStarList[n].Id = walls[_x, _y - 1].Id + 1;
                dStarList[n].IsChecked = true;
                walls[_x, _y - 1].AddingRelation(dStarList[n]);
                for (int w = 0; w < dStarList[n].GetRelationCount; w++)
                {
                    walls[_x, _y - 1].AddingRelation(dStarList[n].GetRelationWall(w));
                }
            }

            Debug.Log("number = " + n);
        }

        //// 検索したセルのみを再更新
        //for (int i = 0; i < dStarList.Count; i++)
        //{
        //    int _x = dStarList[i].Position_X;
        //    int _y = dStarList[i].Position_Y;

        //    if (walls[_x + 1, _y].IsHavingArrow)
        //    {
        //        dStarList[i].ChangeArrow(Wall.StateArrows.Left);
        //        dStarList[i].Id = walls[_x + 1, _y].Id + 1;
        //        dStarList[i].IsChecked = true;
        //        walls[_x + 1, _y].AddingRelation(dStarList[i]);
        //        for (int w = 0; w < dStarList[i].GetRelationCount; w++)
        //        {
        //            walls[_x + 1, _y].AddingRelation(dStarList[i].GetRelationWall(w));
        //        }
        //    }
        //    if (walls[_x - 1, _y].IsHavingArrow)
        //    {
        //        dStarList[i].ChangeArrow(Wall.StateArrows.Right);
        //        dStarList[i].Id = walls[_x - 1, _y].Id + 1;
        //        dStarList[i].IsChecked = true;
        //        walls[_x - 1, _y].AddingRelation(dStarList[i]);
        //        for (int w = 0; w < dStarList[i].GetRelationCount; w++)
        //        {
        //            walls[_x - 1, _y].AddingRelation(dStarList[i].GetRelationWall(w));
        //        }
        //    }
        //    if (walls[_x, _y + 1].IsHavingArrow)
        //    {
        //        dStarList[i].ChangeArrow(Wall.StateArrows.Up);
        //        dStarList[i].Id = walls[_x, _y + 1].Id + 1;
        //        dStarList[i].IsChecked = true;
        //        walls[_x, _y + 1].AddingRelation(dStarList[i]);
        //        for (int w = 0; w < dStarList[i].GetRelationCount; w++)
        //        {
        //            walls[_x, _y + 1].AddingRelation(dStarList[i].GetRelationWall(w));
        //        }
        //    }
        //    if (walls[_x, _y - 1].IsHavingArrow)
        //    {
        //        dStarList[i].ChangeArrow(Wall.StateArrows.Down);
        //        dStarList[i].Id = walls[_x, _y - 1].Id + 1;
        //        dStarList[i].IsChecked = true;
        //        walls[_x, _y - 1].AddingRelation(dStarList[i]);
        //        for (int w = 0; w < dStarList[i].GetRelationCount; w++)
        //        {
        //            walls[_x, _y - 1].AddingRelation(dStarList[i].GetRelationWall(w));
        //        }
        //    }
        //}
    }

    public void textSave(string _txt, string _fileName)
    {
        StreamWriter sw = new StreamWriter(_fileName, true);
        sw.WriteLine(_txt);
        sw.Flush();
        sw.Close();
    }
}
