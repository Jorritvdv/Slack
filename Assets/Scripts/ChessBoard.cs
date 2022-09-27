using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChessBoard : MonoBehaviour
{
    [SerializeField]
    private Text _turnText;
    [SerializeField]
    private GameObject _tilePreFabWhite;
    [SerializeField]
    private GameObject _tilePreFabBlack;
    [SerializeField]
    private int _boardWidth=8;
    [SerializeField]
    private int _boardHeight=8;
    public static ChessBoard Instance;
    public List<GameObject> _tiles = new List<GameObject>();
    public List<GameObject> _tilesWithPossibleMove = new List<GameObject>();
    private GameObject[] _pieces;

    public GameObject Pawn;

    public ChessTile HoveredTile;
    private ChessTile _selectedTileWithPiece;

    private bool _whiteTurn = true;
    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
    }
    
    void Start()
    {
        _pieces = GameObject.FindGameObjectsWithTag("Piece");
        for (int i = 0; i < _boardHeight; i++)
        {
            for (int j = 0; j < _boardWidth; j++)
            {
                Vector3 pos = new Vector3(i, 0, j);
                if(i % 2 == 0)
                {
                    if (j % 2 == 0)
                    {
                        GameObject tile= Instantiate(_tilePreFabWhite, pos, transform.rotation);
                        _tiles.Add(tile);
                    }
                    else
                    {
                        GameObject tile = Instantiate(_tilePreFabBlack, pos, transform.rotation);
                        _tiles.Add(tile);
                    }
                }
                else
                {
                    if (j % 2 == 0)
                    {
                        GameObject tile = Instantiate(_tilePreFabBlack, pos, transform.rotation);
                        _tiles.Add(tile);
                    }
                    else
                    {
                        GameObject tile = Instantiate(_tilePreFabWhite, pos, transform.rotation);
                        _tiles.Add(tile);
                    }
                }
                
                
            }
        }

        SetPiecesToTiles();
    }
    private void SetPiecesToTiles()
    {
        foreach (GameObject tile in _tiles)
        {
            foreach (GameObject piece in _pieces)
            {
                if (tile.transform.position == piece.transform.position)
                    tile.GetComponent<ChessTile>().SetPiece(piece);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (_selectedTileWithPiece != null)
            {
                if (_selectedTileWithPiece == HoveredTile)
                {
                    HoveredTile.UnsetAsTileToMove();
                    _selectedTileWithPiece = null;
                    UncheckTiles();
                }
                else
                {

                    if (!_tilesWithPossibleMove.Contains(HoveredTile.gameObject))
                    {
                        HoveredTile.UnsetAsTileToMove();
                        _selectedTileWithPiece.UnsetAsTileToMove();
                        _selectedTileWithPiece = null;
                        UncheckTiles();
                        return;
                    }
                        
                    if (HoveredTile._hasPiece)
                    {
                        if(_selectedTileWithPiece.GetPiece().GetComponent<ChessPiece>()._isWhitePiece==
                           HoveredTile.GetPiece().GetComponent<ChessPiece>()._isWhitePiece)
                        {
                            return;
                        }
                        HoveredTile.RemovePiece();
                    }
                    
                        
                    
                    HoveredTile.MovePieceHere(_selectedTileWithPiece.GetPiece());
                    
                    _selectedTileWithPiece.UnsetAsTileToMove();
                    _selectedTileWithPiece.RemovePiece();
                    UncheckTiles();
                    _selectedTileWithPiece = null;
                    if (_whiteTurn == true)
                    {
                        _whiteTurn = false;
                        _turnText.text = "Black Turn";
                    }

                    else
                    {
                        _whiteTurn = true;
                        _turnText.text = "White Turn";
                    }
                        
                }
                return;
            }





            if (HoveredTile._hasPiece&&HoveredTile.GetPiece().GetComponent<ChessPiece>()._isWhitePiece==_whiteTurn)
            {
                _selectedTileWithPiece = HoveredTile;
                HoveredTile.SetAsTileToMove();
                CheckPossibleTiles(_selectedTileWithPiece);
            }
        }
    }
    private void UncheckTiles()
    {
        foreach (GameObject tile in _tilesWithPossibleMove)
        {
            tile.GetComponent<ChessTile>().UnsetAsPossibleMoveTile();
        }
        _tilesWithPossibleMove.Clear();
    }
    private void CheckPossibleTiles(ChessTile tile)
    {
        ChessPiece piece = tile.GetPiece().GetComponent<ChessPiece>();
        foreach (Vector3 pos in piece._moveSet)
        {
            foreach (GameObject tileCheck in _tiles)
            {
                if (piece.transform.position + pos == tileCheck.transform.position)
                {
                    if (tileCheck.GetComponent<ChessTile>()._hasPiece)
                    {
                        if (_selectedTileWithPiece.GetPiece().GetComponent<ChessPiece>()._isWhitePiece !=
                           tileCheck.GetComponent<ChessTile>().GetPiece().GetComponent<ChessPiece>()._isWhitePiece)
                        {
                            tileCheck.GetComponent<ChessTile>().SetAsPossibleMoveTile();
                            _tilesWithPossibleMove.Add(tileCheck);
                            return;
                        }
                    }
                    else
                    {
                        tileCheck.GetComponent<ChessTile>().SetAsPossibleMoveTile();
                        _tilesWithPossibleMove.Add(tileCheck);
                    }
                    
                    
                }

            }
            
        }
    }
}
