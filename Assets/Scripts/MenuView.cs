using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuView : MonoBehaviour
{
    public TMPro.TMP_Text names, scores;

    private int page;

    // Start is called before the first frame update
    void Start()
    {
        ScoreManager.Instance.LoadLeaderBoards(0);
    }

    public void LoadMore()
    {
        page++;
        ScoreManager.Instance.LoadLeaderBoards(page);
    }

    private void Update()
    {
        names.text = ScoreManager.Instance.leaderBoardPositionsString;
        scores.text = ScoreManager.Instance.leaderBoardScoresString;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
