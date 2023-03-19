using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GamePanel : MonoBehaviour
{
    public Text text_score; //得分
    public Text text_bestScore;//最高分

    private int score = 0;
    private int bestScore;

    public Transform gridParent;

    public Dictionary<int, int> grid_config = new Dictionary<int, int>() { { 4, 78 }, { 5, 60 }, { 6, 48 } };

    private int row;
    private int col;

    public GameObject gridPrefab;
    public GameObject numberPrefab;

    public GameObject winPanel;
    public GameObject losePanel;

    public Item[][] items = null;

    public List<Item> canCreateNumberGrid = new List<Item>();

    private Vector3 pointerDownPos, pointerUpPos;

    private bool IsNeedCreateNUmber = false;

    private StepData stepData;
    //上一步
    public void OnLast()
    {
        ReadData();
    }
    public void ReadData()
    {

        SetScore(stepData.score);
        SetBestScore(stepData.best_Score);

        for (int i = 0; i < row; i++)
        { 
            for(int j = 0; j < col; j++)
            {
                if (stepData.number[i][j] == 0)
                {
                    if (items[i][j].number != null)
                    {
                        GameObject.Destroy(items[i][j].GetNumber().gameObject);
                        items[i][j].SetNumber(null);
                    }
                }
                else
                {
                    if (items[i][j].number == null)
                    {
                        GameObject gameObj = GameObject.Instantiate(numberPrefab, items[i][j].transform);
                        gameObj.GetComponent<Number>().Init(items[i][j]);
                        items[i][j].number.SetNumber(stepData.number[i][j]);
                    }
                    else 
                    {
                        items[i][j].number.SetNumber(stepData.number[i][j]);
                    }
                }
            }
        }
    }
    //重新开始
    public void OnRestar()
    {
        SceneManager.LoadSceneAsync(1);
    }

    //退出
    public void OnExitToMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }

    //初始化格子
    public void InitGrid()
    {
        int gridCount = PlayerPrefs.GetInt(Const.GameMode, 4);
        items = new Item[gridCount][];
        GridLayoutGroup gridLayoutGroup = gridParent.GetComponent<GridLayoutGroup>();
        gridLayoutGroup.constraintCount = gridCount;
        gridLayoutGroup.cellSize = new Vector2(grid_config[gridCount], grid_config[gridCount]);

        row = gridCount;
        col = gridCount;

        for (int i = 0; i < row; i++)
        {
            for (int x = 0; x < col; x++)
            {
                if (items[i] == null)
                {
                    items[i] = new Item[gridCount];
                }
                items[i][x] = CreateGrid();
            }
        }


    }

    //创建格子
    public Item CreateGrid()
    {
        GameObject item = GameObject.Instantiate(gridPrefab, gridParent);
        return item.GetComponent<Item>();
    }

    //创建数字
    public void CreateNumber()
    {
        canCreateNumberGrid.Clear();

        for (int i = 0; i < row; i++)
        {
            for (int x = 0; x < col; x++)
            {
                if (items[i][x].IsHaveNumber() == false)
                {
                    canCreateNumberGrid.Add(items[i][x]);
                }
            }
        }

        if (canCreateNumberGrid.Count == 0)
        {
            return;
        }

        int index = Random.Range(0, canCreateNumberGrid.Count);

        GameObject gameObj = GameObject.Instantiate(numberPrefab, canCreateNumberGrid[index].transform);
        gameObj.GetComponent<Number>().Init(canCreateNumberGrid[index]);
    }

    private void Awake()
    {
        InitGrid();
        CreateNumber();
        bestScore = PlayerPrefs.GetInt(Const.BestScore);
        SetBestScore(bestScore);
        stepData = new StepData();
    }

    public void OnPointerDown()
    {
        pointerDownPos = Input.mousePosition;
    }

    public void OnPointerUp()
    {
        pointerUpPos = Input.mousePosition;
        if (Vector3.Distance(pointerUpPos, pointerDownPos) > 50)
        {
            MoveType moveType = CaculateMoveType();
            MoveNumber(moveType);
            if (IsNeedCreateNUmber)
            {
                CreateNumber();
            }
            IsNeedCreateNUmber = false;
        }
        ResetNumberType();
    }

    public MoveType CaculateMoveType()
    {
        if (Mathf.Abs(pointerDownPos.x - pointerUpPos.x) > Mathf.Abs(pointerDownPos.y - pointerUpPos.y))
        {
            //左右移动
            if (pointerUpPos.x - pointerDownPos.x > 0)
            {

                return MoveType.right;
            }
            else
            {

                return MoveType.left;
            }
        }
        else
        {
            //上下移动
            if (pointerUpPos.y - pointerDownPos.y > 0)
            {

                return MoveType.up;
            }
            else
            {

                return MoveType.down;
            }
        }
    }

    public void MoveNumber(MoveType moveType)
    {
        stepData.SetStepData(score, bestScore, items);
        switch (moveType)
        {
            case MoveType.up:
                for (int j = 0; j < col; j++)
                {
                    for (int i = 1; i < row; i++)
                    {
                        if (items[i][j].IsHaveNumber())
                        {
                            Number number = items[i][j].GetNumber();
                            for (int m = i - 1; m >= 0; m--)
                            {
                                if (items[m][j].IsHaveNumber())
                                {
                                    //判断能不能合并
                                    if (number.GetNumber() == items[m][j].GetNumber().GetNumber() && items[m][j].GetNumber().status == NumberType.normal)
                                    {
                                        //合并
                                        items[m][j].GetNumber().Merge();
                                        number.GetGrid().SetNumber(null);
                                        //GameObject.Destroy(number.gameObject);
                                        number.DestoryOnMoveEnd(items[m][j]);
                                        IsNeedCreateNUmber = true;
                                    }
                                    break;
                                }
                                else
                                {
                                    //没数字移动上去
                                    number.MoveToGrid(items[m][j]);
                                    IsNeedCreateNUmber = true;
                                }
                            }
                        }
                    }
                }
                break;
            case MoveType.down:
                for (int j = 0; j < col; j++)
                {
                    for (int i = row - 2; i >= 0; i--)
                    {
                        if (items[i][j].IsHaveNumber())
                        {
                            Number number = items[i][j].GetNumber();
                            for (int m = i + 1; m < row; m++)
                            {
                                if (items[m][j].IsHaveNumber())
                                {
                                    //判断能不能合并
                                    if (number.GetNumber() == items[m][j].GetNumber().GetNumber() && items[m][j].GetNumber().status == NumberType.normal)
                                    {
                                        //合并
                                        items[m][j].GetNumber().Merge();
                                        number.GetGrid().SetNumber(null);
                                        //GameObject.Destroy(number.gameObject);
                                        number.DestoryOnMoveEnd(items[m][j]);
                                        IsNeedCreateNUmber = true;
                                    }
                                    break;
                                }
                                else
                                {
                                    //没数字移动上去
                                    number.MoveToGrid(items[m][j]);
                                    IsNeedCreateNUmber = true;
                                }
                            }
                        }
                    }
                }
                break;
            case MoveType.left:
                for (int j = 0; j < col; j++)
                {
                    for (int i = 1; i < row; i++)
                    {
                        if (items[j][i].IsHaveNumber())
                        {
                            Number number = items[j][i].GetNumber();
                            for (int m = i - 1; m >= 0; m--)
                            {
                                if (items[j][m].IsHaveNumber())
                                {
                                    //判断能不能合并
                                    if (number.GetNumber() == items[j][m].GetNumber().GetNumber() && items[j][m].GetNumber().status == NumberType.normal)
                                    {
                                        //合并
                                        items[j][m].GetNumber().Merge();
                                        number.GetGrid().SetNumber(null);
                                        //GameObject.Destroy(number.gameObject);
                                        number.DestoryOnMoveEnd(items[j][m]);
                                        IsNeedCreateNUmber = true;
                                    }
                                    break;
                                }
                                else
                                {
                                    //没数字移动上去
                                    number.MoveToGrid(items[j][m]);
                                    IsNeedCreateNUmber = true;
                                }
                            }
                        }
                    }
                }
                break;
            case MoveType.right:
                for (int j = 0; j < col; j++)
                {
                    for (int i = row - 2; i >= 0; i--)
                    {
                        if (items[j][i].IsHaveNumber())
                        {
                            Number number = items[j][i].GetNumber();
                            for (int m = i + 1; m < row; m++)
                            {
                                if (items[j][m].IsHaveNumber())
                                {
                                    //判断能不能合并
                                    if (number.GetNumber() == items[j][m].GetNumber().GetNumber() && items[j][m].GetNumber().status == NumberType.normal)
                                    {
                                        //合并
                                        items[j][m].GetNumber().Merge();
                                        number.GetGrid().SetNumber(null);
                                        number.DestoryOnMoveEnd(items[j][m]);
                                        IsNeedCreateNUmber = true;
                                    }
                                    break;
                                }
                                else
                                {
                                    //没数字移动上去
                                    number.MoveToGrid(items[j][m]);
                                    IsNeedCreateNUmber = true;
                                }
                            }
                        }
                    }
                }
                break;
        }

        if (IsGameLose())
        {
            GameLose();
        }

    }

    public bool IsGameLose()
    {

        for (int i = 0; i < row; i++)
        {
            for (int x = 0; x < col; x++)
            {
                if (items[i][x].IsHaveNumber() == false)
                {
                    return false;
                }
            }
        }
        for (int i = 0; i < row; i ++)
        {
            for (int j = 0; j < col; j++)
            {
                //Item left = IsExist(i, j - 1) ? items[i][j - 1] : null;
                Item right = IsExist(i, j + 1) ? items[i][j + 1] : null;
                Item down = IsExist(i + 1, j) ? items[i + 1][j] : null;

                if (/*items[i][j].GetNumber().GetNumber() == (left == null ? 0:left.GetNumber().GetNumber()) ||*/
                    items[i][j].GetNumber().GetNumber() == (right == null ? 0 : right.GetNumber().GetNumber()) ||
                    items[i][j].GetNumber().GetNumber() == (down == null ? 0 : down.GetNumber().GetNumber())
                   )
                {
                    return false;
                }
            }
        }
        return true;
    }

    public bool IsExist(int i, int j)//判断格子是否存在
    {
        if (i >= 0 && i < row && j >= 0 && j < row)
        {
            return true;
        }
        else
            return false;
    }

    public void ResetNumberType()
    {
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                if (items[i][j].IsHaveNumber())
                {
                    items[i][j].GetNumber().status = NumberType.normal;
                }
            }
        }

    }

    public void GameWin()
    {
        winPanel.SetActive(true);
    }

    public void GameLose()
    {
        losePanel.SetActive(true);
    }

    public void ChangeScore(int mergeScore)
    {
        score += mergeScore;
        SetScore(score);
        if (score > bestScore)
        {
            SetBestScore(score);
        }
    }
    public void SetScore(int x)
    {
        text_score.GetComponent<Text>().text = x.ToString();
    }

    public void SetBestScore(int x)
    {
        PlayerPrefs.SetInt(Const.BestScore, x);
        text_bestScore.GetComponent<Text>().text = x.ToString();
    }

}
