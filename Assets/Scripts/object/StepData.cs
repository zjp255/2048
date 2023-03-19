using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepData 
{
    public int score;
    public int best_Score;

    public int[][] number;

    public void SetStepData(int score,int best_Score,Item[][] items)
    {
        this.score = score;
        this.best_Score = best_Score;

        if (number == null)
        {
            number = new int[items.Length][];
        }

        for (int i = 0; i < items.Length; i++)
        {
            for (int j = 0; j < items.Length; j++)
            {
                if (number[i] == null)
                {
                    number[i] = new int[items.Length];
                }

                number[i][j] = items[i][j].GetNumber() == null ? 0 : items[i][j].GetNumber().GetNumber();

            }
        }
    }
   
}
