using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PuzzleController : MonoBehaviour
{
    public static PuzzleController Instance { get; private set; }
    Tile[,] grid;

    [SerializeField]
    int sizeX, sizeY;
    [SerializeField]
    Tile[] tilesPrefab;

    [SerializeField]
    List<string> types;

    [SerializeField]
    int[] totals;

    [SerializeField]
    GameObject ghostObj;
    bool canMove = false;
    bool fast = true;

    int dragY = -1;
    int dragX = -1;

    Vector3 temp;

    void Awake()
    {
        if (Instance != this && Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    //Start Puzzle
    public void StartPuzzle()
    {

        types.Add("White");
        types.Add("Green");
        types.Add("Blue");
        types.Add("Red");

        grid = new Tile[sizeX, sizeY*2];
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                CreateTile(i, j);
            }
        }
        Check();
    }

    private void Update()
    {
        temp = Input.mousePosition;
        temp.z = 12;
        ghostObj.transform.position = Camera.main.ScreenToWorldPoint(temp);
        if (Input.GetMouseButtonUp(0))
        {
            ghostObj.SetActive(false);
        }
    }

    public void DragTile(Tile tile)
    {
        if (!canMove)
            return;
        dragX = tile.xPos;
        dragY = tile.yPos;
        ghostObj.SetActive(true);
        ghostObj.GetComponent<SpriteRenderer>().color = new Color( tile.GetComponent<SpriteRenderer>().color.r,
                                                                    tile.GetComponent<SpriteRenderer>().color.g,
                                                                    tile.GetComponent<SpriteRenderer>().color.b,.75f);
       
    }

    public void DropTile(Tile tile)
    {
        
        if (!canMove)
        {
        
            return;
        }
        if (dragX == -1 || dragY == -1)
        {
           
            return;
        }
        SwapTiles(dragX, dragY, tile.xPos, tile.yPos);

        dragX = -1;
        dragY = -1;
    }

    void SwapTiles(int x1, int y1, int x2, int y2)
    {
        fast = false;
        if (x1 == x2 && y1 == y2)
            return;

        MoveTile(x1, y1, x2, y2);

        List<Tile> TilesToCheck = CheckHorizontalMatches();
        TilesToCheck.AddRange(CheckVerticalMatches());

        if (TilesToCheck.Count == 0)
        {
            MoveTile(x1, y1, x2, y2);
        }
        Check();
    }

    void Check()
    {
        List<Tile> TilesToDestroy = CheckHorizontalMatches();
        TilesToDestroy.AddRange(CheckVerticalMatches());

        TilesToDestroy = TilesToDestroy.Distinct().ToList();

        bool sw = TilesToDestroy.Count == 0;

        var query = TilesToDestroy.GroupBy(Tile => Tile.type);
        if (query != null)
        {
            foreach (var Tile in query)
            {
                if (Tile.Key.ToString() == types[0])
                    totals[0] = Tile.Count(); 
                else if (Tile.Key.ToString() == types[1])
                    totals[1] = Tile.Count();
                else if (Tile.Key.ToString() == types[2])
                    totals[2] = Tile.Count();
                else if (Tile.Key.ToString() == types[3])
                    totals[3] = Tile.Count();
            }
        }

        int red = 0;
        int blue = 0;
        int green = 0;
        int white = 0;
        for (int i = 0; i < TilesToDestroy.Count; i++)
        {
           
            
            if (TilesToDestroy[i] != null)
            {
                switch (TilesToDestroy[i].type)
                {
                    case "White":
                        white++;
                        break;
                    case "Green":
                        green++;
                        break;
                    case "Blue":
                        blue++;
                        break;
                    case "Red":
                        red++;
                        break;
                }
                
                Destroy(TilesToDestroy[i].gameObject);
                
                CreateTile(TilesToDestroy[i].xPos, TilesToDestroy[i].yPos + sizeY);
            }
        }
        GameManager.Instance.AddPoints(red, blue, green, white);
        if (!sw)
            StartCoroutine(Gravity());
        else
        {
            canMove = true;
            for (int i = 0; i < totals.Count(); i++)
            {
                totals[i] = 0;
            }
            GameManager.Instance.SetPlayerTurn(false);
        }
    }

    IEnumerator Gravity()
    {
        bool Sw = true;
        while (Sw)
        {
            canMove = false;
            Sw = false;
            for (int j = 0; j < sizeY * 2; j++)
            {
                for (int i = 0; i < sizeX; i++)
                {
                    if (Fall(i, j))
                    {
                        Sw = true;
                    }
                }

                if (j <= sizeY && !fast)
                    yield return null;
            }
        }
        yield return null;
        //canMove = true;
        Check();
    }

    bool Fall(int x, int y)
    {
        if (x < 0 || y <= 0 || x >= sizeX || y >= sizeY * 2)
            return false;
        if (grid[x, y] == null)
            return false;
        if (grid[x, y - 1] != null)
            return false;

        MoveTile(x, y, x, y - 1);
        return true;

    }

    List<Tile> CheckVerticalMatches()
    {
        List<Tile> TilesToCheck = new List<Tile>();
        List<Tile> TilesToReturn = new List<Tile>();

        string type = "";

        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                if (grid[i, j].type != type)
                {
                    if (TilesToCheck.Count >= 3)
                    {
                        TilesToReturn.AddRange(TilesToCheck);
                    }
                    TilesToCheck.Clear();
                }
                type = grid[i, j].type;
                TilesToCheck.Add(grid[i, j]);
            }
            if (TilesToCheck.Count >= 3)
            {
                TilesToReturn.AddRange(TilesToCheck);
            }
            TilesToCheck.Clear();
        }
        return TilesToReturn;
    }
    
    List<Tile> CheckHorizontalMatches()
    {
        List<Tile> TilesToCheck = new List<Tile>();
        List<Tile> TilesToReturn = new List<Tile>();

        string type = "";

        for (int j = 0; j < sizeY; j++)
        {
            for (int i = 0; i < sizeX; i++)
            {
                if (grid[i, j].type != type)
                {
                    if (TilesToCheck.Count >= 3)
                    {
                        TilesToReturn.AddRange(TilesToCheck);
                    }
                    TilesToCheck.Clear();
                }
                type = grid[i, j].type;
                TilesToCheck.Add(grid[i, j]);
            }
            if (TilesToCheck.Count >= 3)
            {
                TilesToReturn.AddRange(TilesToCheck);
            }
            TilesToCheck.Clear();
        }
        return TilesToReturn;
    }

    void MoveTile(int xPos1, int yPos1, int xPos2, int yPos2)
    {
        if (grid[xPos1, yPos1] != null)
            grid[xPos1, yPos1].transform.position = new Vector3(xPos2, yPos2);
        if (grid[xPos2, yPos2] != null)
            grid[xPos2, yPos2].transform.position = new Vector3(xPos1, yPos1);

        Tile temporal = grid[xPos1, yPos1];
        grid[xPos1, yPos1] = grid[xPos2, yPos2];
        grid[xPos2, yPos2] = temporal;
        if (grid[xPos1, yPos1] != null)
            grid[xPos1, yPos1].ChangePosition(xPos1, yPos1);
        if (grid[xPos2, yPos2] != null)
            grid[xPos2, yPos2].ChangePosition(xPos2, yPos2);
    }

    //Create New Tile
    void CreateTile(int x, int y)
    {
        Tile newTile = Instantiate(tilesPrefab[Random.Range(0, tilesPrefab.Length)], new Vector2(x, y), Quaternion.identity);
        newTile.ChangePosition( x, y);
        grid[x, y] = newTile;
    }
}
