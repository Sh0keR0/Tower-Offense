using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class InputFieldNumberOnly : MonoBehaviour
{

    private TMP_InputField currentInputField;

    private void Start()
    {
        currentInputField = GetComponent<TMP_InputField>();
        currentInputField.onValidateInput += delegate (string input, int charIndex, char addedChar) { return MyValidate(addedChar); };
    }

    private char MyValidate(char charToValidate)
    {
        //Checks if a dollar sign is entered....
        if (char.IsNumber(charToValidate) == false)
        {
            // ... if it is change it to an empty character.
            charToValidate = '\0';
        }
        return charToValidate;
    }
}
