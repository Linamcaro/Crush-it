using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIPoints : MonoBehaviour
{
    private int displayedPoints = 0;

    [SerializeField] private TextMeshProUGUI pointsText;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.OnPointsUpdated.AddListener(UpdatePoints);
    }

    private void UpdatePoints()
    {
        StartCoroutine(UpdatePointsCoroutine());
    }

    IEnumerator UpdatePointsCoroutine()
    {
        while(displayedPoints < GameManager.Instance.Points)
        {
            displayedPoints++;
            pointsText.text = displayedPoints.ToString();
            yield return new WaitForSeconds(0.1f);    
        }

        yield return null;
    }
}
