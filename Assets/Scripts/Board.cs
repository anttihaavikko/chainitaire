using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Board : MonoBehaviour
{
    public GameObject blockPrefab, markerPrefab;
    public Dude dude;
    public LayerMask markerLayer, bonusLayer, cardLayer;
    public Sprite[] bonusSprites;
    public Bonus bonus;

    private List<GameObject> cells, markers;
    private bool firstMove = true;

    // Start is called before the first frame update
    void Start()
    {
        cells = new List<GameObject>();
        markers = new List<GameObject>();

        for (var i = 0; i < 11; i++)
        {
            var c = Instantiate(blockPrefab, transform);
            cells.Add(c);
        }

        for (var i = 0; i < 52; i++)
        {
            var m = Instantiate(markerPrefab, transform);
            m.SetActive(false);
            cells.Add(m);
            markers.Add(m);
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

        var possibleStarts = cells.Where(CanBeStart);
        var start = possibleStarts.OrderBy(c => Random.value).First(c => c.tag == "Island");
        start.tag = "Start";
        dude.transform.position = start.transform.position;

        DoBonus(true);
    }

    public void DoBonus(bool silent = false)
    {
        if(!silent)
        {
             // TODO: do spawn sound here
        }

        var spot = markers.OrderBy(m => Random.value).First();
        bonus.transform.position = spot.transform.position;

        bonus.SetSprite(bonusSprites[Random.Range(0, bonusSprites.Length)]);
    }

    private bool CanBeStart(GameObject g)
    {
        if (g.tag != "Island") return false;

        var neighbors = GetNeighbors(g.transform.position).Count(go => go != null);
        return g.tag == "Island" && neighbors <= 1;
    }

    public void Moved()
    {
        firstMove = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowMarkers()
    {
        var activeMarkers = markers.Where(m => GetNeighbors(m.transform.position).Any(IsOk)).ToList();
        activeMarkers.ForEach(m => m.SetActive(true));
    }

    public void HideMarkers()
    {
        markers.ForEach(m => m.SetActive(false));
    }

    private bool IsOk(GameObject g)
    {
        if (!g) return false;
        if(firstMove) return g.tag == "Start";
        return g.tag == "Island" || g.tag == "Card" || g.tag == "Wall" || g.tag == "Start";
    }

    private List<GameObject> GetNeighbors(Vector3 pos)
    {
        var all = new List<GameObject>
        {
            GetNeighbor(pos + Vector3.up),
            GetNeighbor(pos + Vector3.down),
            GetNeighbor(pos + Vector3.right),
            GetNeighbor(pos + Vector3.left)
        };

        return all;
    }

    private GameObject GetNeighbor(Vector3 pos)
    {
        var hit = Physics2D.OverlapCircle(pos, 0.1f);

        if (hit)
        {
            return hit.gameObject;
        }

        return null;
    }

    public void DeactivateMarker(GameObject marker)
    {
        markers.Remove(marker);
    }
}
