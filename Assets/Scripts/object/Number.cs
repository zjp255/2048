using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Number : MonoBehaviour
{
    private Image bcImage;
    private Text numberText;

    private Item inItem;

    public NumberType status;

    private float spawnScaleTime = 1;
    private float mergeScaleTime = 2;

    private float movePosTime = 1;
    private Vector3 moveStart;

    private bool isDestoryOnMoveEnd = false;
    public Color[] bg_Color;
    public List<int> number_Index;


    private void Awake()
    {
        bcImage = GetComponent<Image>();
        numberText = transform.Find("Text").GetComponent<Text>();
    }

    public void Init(Item myGrid)
    {
        myGrid.SetNumber(this);
        SetGrid(myGrid);
        SetNumber(2);
        status = NumberType.normal;
        PlaySpawnAnim();
    }

    public void SetGrid(Item item)
    {
        this.inItem = item;
    }

    public Item GetGrid()
    {
        return inItem;
    }

    public void SetNumber(int x)
    {
       
        numberText.text = x.ToString();
        this.GetComponent<Image>().color = bg_Color[number_Index.IndexOf(x)];
        if (x > 100)
        {
            this.transform.Find("Text").GetComponent<Text>().fontSize = 20;
        }
        if (x > 1000)
        {
            this.transform.Find("Text").GetComponent<Text>().fontSize = 15;
        }
        if (x == 2048)
        {
            GameObject.Find("Canvas/GamePanel").GetComponent<GamePanel>().GameWin();
        }
    }

    public int GetNumber()
    {
        return int.Parse(this.numberText.text);
        
    }

    public void DestoryOnMoveEnd(Item grid)
    {
        //moveStart = transform.localPosition;
        //moveEnd = grid.transform.position;
        moveStart = transform.position - grid.transform.position;
        movePosTime = 0;
        isDestoryOnMoveEnd = true;
    }

    public void MoveToGrid(Item grid)
    {
        transform.SetParent(grid.transform);
        //transform.localPosition = Vector3.zero;
        moveStart = transform.position - grid.transform.position;
        //moveEnd = grid.transform.position;

        movePosTime = 0;

        GetGrid().SetNumber(null);
        grid.SetNumber(this);
        SetGrid(grid);
    }


    public void Merge()
    {
        GameObject.Find("Canvas/GamePanel").GetComponent<GamePanel>().ChangeScore(this.GetNumber());
        this.SetNumber(this.GetNumber() * 2);
        status = NumberType.CanNotMerge;
        PlayMergeAnim();
        AudioManager.instance.PlaySound();
    }

    public void PlaySpawnAnim()
    {
        spawnScaleTime = 0;
    }
    public void PlayMergeAnim()
    {
        mergeScaleTime = 0;
    }
    public void Update()
    {
        if (spawnScaleTime <= 1)
        {
            spawnScaleTime += Time.deltaTime * 4;
            transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, spawnScaleTime);
        }
        if (mergeScaleTime <= 1)
        {
            mergeScaleTime += Time.deltaTime * 4;
            transform.localScale = Vector3.Lerp(Vector3.one, new Vector3(1.2f, 1.2f, 1.2f), mergeScaleTime);
        }
        if (1 < mergeScaleTime && mergeScaleTime <= 2)
        {
            mergeScaleTime += Time.deltaTime * 4;
            transform.localScale = Vector3.Lerp(new Vector3(1.2f, 1.2f, 1.2f), Vector3.one, mergeScaleTime);
        }
        if (movePosTime <= 1)
        {
            movePosTime += Time.deltaTime * 4;
            transform.localPosition = Vector3.Lerp(moveStart, Vector3.zero, movePosTime);
            if (movePosTime >= 1 && isDestoryOnMoveEnd == true)
            {
                GameObject.Destroy(gameObject);
            }
        }

    }
}
