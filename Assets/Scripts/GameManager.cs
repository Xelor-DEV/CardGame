using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Sprite backcardSprite;
    private bool firstGuess, secondGuess;
    public CardInfoSO[] cardPool;
    public GameObject card;
    public GameObject cardField;
    public GameObject winPanel;
    private List<GameObject> cards = new List<GameObject>();
    private List<Button> buttons = new List<Button>();


    private int index;
    private Card firstchoise;
    private Card secondchoise;
    private bool evaluating;

    private int matches;
    private int totalMatches;

    [SerializeField] private string scene;

    public AudioSource mainAudioSource;
    public AudioSource audioSourceone;
    public AudioSource audioSourcetwo;
    public AudioClip goodAudio;
    public AudioClip wrongAudio;

   
    void Start()
    {
        totalMatches = cardPool.Length;
        for (int i = 0; i < cardPool.Length; ++i)
        {
            for (int l = 0; l < 2; ++l)
            {
                GameObject go = Instantiate(card, cardField.transform, false);
                go.GetComponent<Card>().Initialize(cardPool[i].index, cardPool[i].sprite, backcardSprite);
                go.gameObject.name = i.ToString();
                cards.Add(go);
            }
        }
        List<GameObject> cardscopy = new List<GameObject>();
        List<GameObject> displaycards = new List<GameObject>();

        for (int i = 0; i < cards.Count; ++i)
        {
            cardscopy.Add(cards[i]);
        }
        for (int i = 0; i < cards.Count; ++i)
        {
            int x = UnityEngine.Random.Range(0, cardscopy.Count);
            displaycards.Add(cardscopy[x]);
            cardscopy.RemoveAt(x);
        }
        for (int i = 0; i < cards.Count; ++i)
        {
            cards[i] = displaycards[i];
            cards[i].transform.SetSiblingIndex(i);
        }
        for (int i = 0; i < cards.Count; ++i)
        {
            Button btn = cards[i].gameObject.GetComponent<Button>();
            buttons.Add(btn);
        }
        AddListeners();

    }
    void AddListeners()
    {
        foreach (Button btn in buttons)
        {
            btn.onClick.AddListener(() => PickACard());
        }
    }

    public void PickACard()
    {
        if (evaluating)
        {
            return;
        }
        if (index < 2)
        {
            index++;
            UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Card>().Flip();
            if (!firstGuess)
            {
                firstGuess = true;
                firstchoise = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Card>();
                audioSourceone.Play();
            }
            else if (!secondGuess)
            {
                secondGuess = true;
                secondchoise = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Card>();
                audioSourcetwo.Play();
            }
        }
        if (index == 2)
        {
            evaluating = true;
            StartCoroutine(EvaluateCards());
        }
    }

    private IEnumerator EvaluateCards()
    {
        yield return new WaitForSeconds(1.5f);
        firstGuess = secondGuess = false;
        if (firstchoise.Index() == secondchoise.Index() && firstchoise.GetInstanceID() != secondchoise.GetInstanceID())
        {
            if (!firstchoise.IsPaired() && !secondchoise.IsPaired())
            {
                firstchoise.SetPair();
                secondchoise.SetPair();
                matches++;
                firstchoise.btn.interactable = false;
                secondchoise.btn.interactable = false;
                mainAudioSource.PlayOneShot(goodAudio);
            }
        }
        else
        {
            firstchoise.Flip();
            secondchoise.Flip();
            mainAudioSource.PlayOneShot(wrongAudio);
        }
        index = 0;
        evaluating = false;
        Win();
    }
    public void Win()
    {

        if (matches == totalMatches)
        {
            winPanel.gameObject.SetActive(true);
        }

    }
    public void RestartGame()
    {
        SceneManager.LoadScene(scene);
    }
}
