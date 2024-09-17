using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public Image img;
    public Button btn;
    public Sprite frontSprite;
    public Sprite backSprite;
    [SerializeField] private int _index;
    [SerializeField] private bool isFlipped = true;
    [SerializeField] private bool isPaired = false;

    public void Initialize(int index, Sprite front, Sprite back)
    {
        _index = index;
        img = GetComponent<Image>();
        btn = GetComponent<Button>();
        frontSprite = front;
        backSprite = back;

        Flip();
    }

    public void Flip()
    {
        isFlipped = !isFlipped;
        img.sprite = isFlipped ? frontSprite : backSprite;
    }

    public int Index()
    {
        return _index;
    }
    public void SetPair()
    {
        isPaired = true;
    }
    public bool IsPaired()
    {
        return isPaired;
    }
}
