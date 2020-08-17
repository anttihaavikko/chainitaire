using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Board : MonoBehaviour
{
    public GameObject blockPrefab;
    public Dude dude;

    private List<GameObject> cells;

    // Start is called before the first frame update
    void Start()
    {
        cells = new List<GameObject>();

        for(var i = 0; i < 11; i++)
        {
            var c = Instantiate(blockPrefab, transform);
            cells.Add(c);
        }

        for (var i = 0; i < 52; i++)
        {
            //var c = Instantiate(blockPrefab, transform);
            cells.Add(null);
        }

        cells = cells.OrderBy(c => Random.value).ToList();

        var index = 0;

        for (var i = -4; i < 5; i++)
        {
            for (var j = -3; j < 4; j++)
            {
                if(cells[index])
                    cells[index].transform.position = transform.position + new Vector3(i, j, 0);

                index++;
            }
        }

        dude.transform.position = cells.Where(c => c != null).First().transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
