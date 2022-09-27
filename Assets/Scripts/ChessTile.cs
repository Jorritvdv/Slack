using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChessTile : MonoBehaviour
{
    [SerializeField]
    private Material _selectMaterial;
    [SerializeField]
    private Material _tileSelectedForMoveMaterial;
    [SerializeField]
    private Material _caneMoveHereMaterial;
    private MeshRenderer _renderer;
    private Material _startMaterial;
    private GameObject _pieceOnTile;
    public bool _hasPiece;
    private bool _selectedForMove;
    public bool _isPossibleMoveTile;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
        _renderer = this.gameObject.GetComponent<MeshRenderer>();
        _startMaterial = _renderer.material;
        
    }
    public void SetPiece(GameObject piece)
    {
        _pieceOnTile = piece;
        _hasPiece = true;
    }
    public void MovePieceHere(GameObject piece)
    {
        
       GameObject spawnedPiece= Instantiate(piece, transform);
        _pieceOnTile = spawnedPiece;
        _hasPiece = true;
        _pieceOnTile.transform.localPosition = Vector3.zero;
        
    }
    public void SetAsPossibleMoveTile()
    {
        _renderer.material = _caneMoveHereMaterial;
        _isPossibleMoveTile = true;
    }
    public void UnsetAsPossibleMoveTile()
    {
        _renderer.material = _startMaterial;
        _isPossibleMoveTile = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetAsTileToMove()
    {
        _selectedForMove = true;
        _renderer.material = _tileSelectedForMoveMaterial;
    }
    public void UnsetAsTileToMove()
    {
        _selectedForMove = false;
        _renderer.material = _startMaterial;
    }
    public GameObject GetPiece()
    {
        return _pieceOnTile;
    }
    public void RemovePiece()
    {
        Destroy(_pieceOnTile);
        _hasPiece = false;
        _renderer.material = _startMaterial;
    }
    
    private void OnMouseEnter()
    {
        ChessBoard.Instance.HoveredTile = this;
        if (_selectedForMove)
            return;
        if (_isPossibleMoveTile)
            return;
        _renderer.material = _selectMaterial;


        
           
        
            
    }
    private void OnMouseExit()
    {
        if (_isPossibleMoveTile)
            return;
        if (_selectedForMove)
            return;
        _renderer.material = _startMaterial;
    }

    
}
