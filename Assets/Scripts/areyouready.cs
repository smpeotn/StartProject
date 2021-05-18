using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class areyouready : MonoBehaviour
{
    [SerializeField]
    private Image _img;

    [SerializeField]
    private Game _game;
    
    private bool _next = false;

    private Sprite[] _cntSprite = new Sprite[4];
    
    // Start is called before the first frame update
    void Start()
    {
        iTween.ScaleFrom(gameObject, iTween.Hash(
            "x", 2f,
            "easetype", "easeInOutQuart",
            "time", 3f,
            "oncompletetarget", gameObject,
            "oncomplete", "endAnimation"
        ));
        setFadeIn();
        StartCoroutine("Count");

        _cntSprite[3] = Resources.Load<Sprite>("Images/three");
        _cntSprite[2] = Resources.Load<Sprite>("Images/two");
        _cntSprite[1] = Resources.Load<Sprite>("Images/one");
        _cntSprite[0] = Resources.Load<Sprite>("Images/Go");
    }

    protected void setFadeIn()
    {
        iTween.ValueTo(gameObject, iTween.Hash(
            "from", 0f, 
            "to", 1f, 
            "time", 1f, 
            "onupdate", "_fadeIn"
        ));
    }

    protected void _fadeIn(float alpha) {
        // iTweenで呼ばれたら、受け取った値をImageのアルファ値にセット
        Color c = _img.color;
        _img.color = new Color(c.r,c.g,c.b, alpha);
    }

    private IEnumerator Count()
    {
        yield return new WaitWhile(() => _next == false);

        int cnt = 3;
        while (cnt >= 0)
        {
            _img.sprite = _cntSprite[cnt];
            --cnt;
            yield return new WaitForSeconds(1.0f);
        }
        _game.GameStart();
    }
    
    protected void endAnimation()
    {
        _next = true;
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
