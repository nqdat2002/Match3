using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearablePiece : MonoBehaviour
{

    public AnimationClip clearAnimation;
    private bool isBeingCleared = false;

    public bool IsBeingCleared { 
        get {
            return isBeingCleared;
        }
        private set {
            isBeingCleared = value;
        }
    }

    protected GamePiece piece;

    private void Awake()
    {
        piece = GetComponent<GamePiece>();
    }

    public virtual void Clear()
    {
        piece.GameGridRef.level.OnPieceCleared(piece);
        isBeingCleared = true;
        StartCoroutine(ClearCoroutine());
    }

    private IEnumerator ClearCoroutine()
    {
        var animator = GetComponent<Animator>();

        if (animator)
        {
            animator.Play(clearAnimation.name);

            yield return new WaitForSeconds(clearAnimation.length);

            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
