using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall_D : MonoBehaviour
{
    public enum StateCell
    {
        Free,
        Wall,
        ProtectWall,
        Start,
        Goal,
    }

    public enum StateArrows
    {
        None,
        Up,
        Down,
        Right,
        Left,
    }

    public StateCell state = StateCell.Wall;
    public StateArrows arrow = StateArrows.None;

    public Material matFree;
    public Material matWall;
    public Material matStart;
    public Material matGoal;

    public Material matNone;
    public Material matUp;
    public Material matDown;
    public Material matRight;
    public Material matLeft;

    private Renderer rendArrow;
    private Renderer rendWall;
    private Color defColWall;
    private Color defArrowCol;

    public TextMesh idText;

    private int positionX = 0;
    private int positionY = 0;

    [SerializeField]
    private int id = 0;
    private bool isChecked = false;
    private bool isChanged = false;
    private bool isDstar = false;

    public List<Wall> relationWalll;

    public List<Route> relationRoutes;

    // Use this for initialization
    void Start()
    {
        rendArrow = GetComponentInChildren<Renderer>();
        rendWall = GetComponent<Renderer>();
        defColWall = GetComponent<Renderer>().material.color;
        defArrowCol = rendArrow.material.color;

        idText = transform.Find("IdText").GetComponent<TextMesh>();

        relationWalll = new List<Wall>();
        relationRoutes = new List<Route>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChangeState(StateCell _state)
    {
        // ProtectWall以外の時、変更
        if (state != StateCell.ProtectWall)
        {
            switch (_state)
            {
                case StateCell.Free:
                    GetComponent<Renderer>().material = matFree;
                    break;
                case StateCell.ProtectWall:
                case StateCell.Wall:
                    ChangeArrow(StateArrows.None);
                    GetComponent<Renderer>().material = matWall;
                    break;
                case StateCell.Start:
                    GetComponent<Renderer>().material = matStart;
                    break;
                case StateCell.Goal:
                    GetComponent<Renderer>().material = matGoal;
                    break;
                default:
                    break;
            }

            state = _state;
        }
    }

    public void ChangeArrow(StateArrows _state)
    {
        if (state == StateCell.Free)
        {
            switch (_state)
            {
                case StateArrows.None:
                    rendArrow.material = matNone;
                    break;
                case StateArrows.Up:
                    rendArrow.material = matUp;
                    break;
                case StateArrows.Down:
                    rendArrow.material = matDown;
                    break;
                case StateArrows.Right:
                    rendArrow.material = matRight;
                    break;
                case StateArrows.Left:
                    rendArrow.material = matLeft;
                    break;
                default:
                    break;
            }

            arrow = _state;
        }
    }

    public Wall GetRelationWall(int _listNum)
    {
        return relationWalll[_listNum];
    }

    public int GetRelationCount
    {
        get
        {
            return relationWalll.Count;
        }
    }

    public void AddingRelation(Wall _wall)
    {
        if (state == StateCell.Free)
        {
            relationWalll.Add(_wall);
        }
    }

    public void ResetRelation()
    {
        for (int i = relationWalll.Count - 1; i > 0; i--)
        {
            relationWalll.RemoveAt(i);
        }
    }

    public StateCell GetState
    {
        get
        {
            return state;
        }
    }

    public int Position_X
    {
        get
        {
            return positionX;
        }
        set
        {
            positionX = value;
        }
    }

    public int Position_Y
    {
        get
        {
            return positionY;
        }
        set
        {
            positionY = value;
        }
    }

    public int Id
    {
        get
        {
            return id;
        }
        set
        {
            id = value;
            setText(id);
        }
    }

    public bool IsChecked
    {
        get
        {
            return isChecked;
        }
        set
        {
            isChecked = value;
        }
    }
    public bool IsChanged
    {
        get
        {
            return isChanged;
        }
        set
        {
            isChanged = value;
        }
    }

    public bool IsHavingArrow
    {
        get
        {
            if (arrow == StateArrows.None)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    public bool BeAbleChange
    {
        get
        {
            if (state == StateCell.Free || state == StateCell.Start)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public bool BeAbleToRoute
    {
        get
        {
            if (state == StateCell.Free || state == StateCell.Goal)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public void setText(int _id)
    {
        idText.text = "" + _id;
    }

    public void changeMaterialDefault()
    {
        rendWall.material.color = defColWall;
    }

    public void changeMaterialColor()
    {
        rendWall.material.color = new Color(0, 1.0f, 0);
    }

    public void changeArrowMaterialDefault()
    {
        rendArrow.material.color = defArrowCol;
    }

    public void changeArrowMaterialColor()
    {
        rendArrow.material.color = new Color(0.0f, 1.0f, 0.0f);
    }

    public void changeIdText()
    {
        idText.text = "R";
    }

    public void changeIdTextDef()
    {
        idText.text = "" + id;
    }

    public void addingRoutes(Route _route)
    {
        relationRoutes.Add(_route);
        //Debug.Log(name + ".AddingRoutes");
    }

    public void clearRoutes()
    {
        for (int i = relationRoutes.Count - 1; i > 0; i--)
        {
            relationRoutes.RemoveAt(i);
        }
    }
}
