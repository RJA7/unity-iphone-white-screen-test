using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ButtonsController : MonoBehaviour
{
    public GameObject button0;
    public GameObject button1;
    public GameObject button2;
    public GameObject button3;
    public GameObject button4;
    public GameObject button5;
    public GameObject button6;
    public GameObject button7;
    public GameObject button8;
    public GameObject button9;
    public GameObject buttonSplit;
    public GameObject buttonFrame;
    public GameObject buttonFreeLast;
    public GameObject buttonBackspace;
    public GameObject buttonNewLine;
    public GameObject buttonClear;
    public GameObject textField;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        button0.GetComponent<Button>().onClick.AddListener(CreateClickHandler("0"));
        button1.GetComponent<Button>().onClick.AddListener(CreateClickHandler("1"));
        button2.GetComponent<Button>().onClick.AddListener(CreateClickHandler("2"));
        button3.GetComponent<Button>().onClick.AddListener(CreateClickHandler("3"));
        button4.GetComponent<Button>().onClick.AddListener(CreateClickHandler("4"));
        button5.GetComponent<Button>().onClick.AddListener(CreateClickHandler("5"));
        button6.GetComponent<Button>().onClick.AddListener(CreateClickHandler("6"));
        button7.GetComponent<Button>().onClick.AddListener(CreateClickHandler("7"));
        button8.GetComponent<Button>().onClick.AddListener(CreateClickHandler("8"));
        button9.GetComponent<Button>().onClick.AddListener(CreateClickHandler("9"));
        buttonSplit.GetComponent<Button>().onClick.AddListener(CreateClickHandler("|"));
        buttonNewLine.GetComponent<Button>().onClick.AddListener(CreateClickHandler("\n"));
        buttonFrame.GetComponent<Button>().onClick.AddListener(CreateSplitClickHandler("T"));
        buttonFreeLast.GetComponent<Button>().onClick.AddListener(CreateSplitClickHandler("F"));

        buttonBackspace
            .GetComponent<Button>()
            .onClick.AddListener(() =>
            {
                var field = textField.GetComponent<TMP_InputField>();
                field.text = field.text[..^1];
            });

        buttonClear
            .GetComponent<Button>()
            .onClick.AddListener(() =>
            {
                textField.GetComponent<TMP_InputField>().text = "";
            });
    }

    UnityAction CreateClickHandler(string value)
    {
        return () =>
        {
            textField.GetComponent<TMP_InputField>().text += value;
        };
    }

    UnityAction CreateSplitClickHandler(string value)
    {
        return () =>
        {
            var field = textField.GetComponent<TMP_InputField>();

            if (field.text.Last() != '|')
            {
                field.text += "|";
            }

            field.text += value;
            field.text += "|";
        };
    }
}
