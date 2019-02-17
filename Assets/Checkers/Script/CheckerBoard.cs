using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckerBoard : MonoBehaviour {
    [SerializeField] private GameObject whitePiecePref;
    [SerializeField] private GameObject blackPiecePref;
    private GameObject _blacks;
    private GameObject _whites;

    private Vector3 boardOffset;

    private Piece selectedPiece;

    private Vector2 mouseOver;
    private Vector2 startDrag;

    float force = 20;

    void Start() {
        boardOffset = new Vector3(-0.1f, 0, -0.1f);
    }

    private void Update()
    {
        createPieces();
        updateMouseOver();
        //if it's my turn
        {
            int x = (int)mouseOver.x;
            int y = (int)mouseOver.y;
            if (Input.GetMouseButtonDown(0))
            {
                choosePiece();
            }

            if (Input.GetMouseButtonUp(0))
            {
                TryMove((int)startDrag.x, (int)startDrag.y);
            }
        }
    }

    private void choosePiece()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            GameObject hitObject = hit.transform.gameObject; //now work with hitObject
            Rigidbody moveObject = hit.collider.attachedRigidbody;
            if (moveObject != null & !moveObject.isKinematic)
            {
                Vector3 dir = moveObject.transform.position - transform.position; //calculate the direction of hitting
                Debug.Log(hit.point + " and " + transform.position);
                dir = -dir.normalized;
                moveObject.velocity = dir * force;
            }
            //Debug.Log(hitObject);
        }
    }
    private void TryMove(int x1, int y1)
    {
        //Multi Support
        startDrag = new Vector2(x1, y1);

        //Check if out of bound
    }

    private void updateMouseOver()
    {
        //if its my turn

        if (!Camera.main)
        {
            Debug.Log("Unable to find main camera");
            return;
        }

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 25.0f, LayerMask.GetMask("Board")))
            {
            mouseOver.x = (int)(hit.point.x - boardOffset.x);
            mouseOver.y = (int)(hit.point.z - boardOffset.z);
            //Debug.Log("Hit" + hit.point);
        }
        else
        {
            mouseOver.x = -1;
            mouseOver.y = -1;
           // Debug.Log("Hit" + hit.point);
        }
    }

    private void createPieces()
    {
        if (_blacks == null || _whites == null)
        {

            for (int b = 0; b < 8; b++)
            {
                _blacks = Instantiate(blackPiecePref) as GameObject;
                _whites = Instantiate(whitePiecePref) as GameObject;
                _blacks.transform.position = new Vector3(-3.5f, 0.5f, -3.4f) + (Vector3.right * b) + boardOffset;
                _whites.transform.position = new Vector3(-3.5f, 0.5f, 3.4f) + (Vector3.right * b) + boardOffset;
            }
        }
    }
}
