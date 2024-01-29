using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class TextTypingEffect : MonoBehaviour
{
    public float typingSpeed = 0.1f; // Adjust the typing speed as needed
    public string fullText;
    private string currentText = "";
    private int currentCharIndex = 0;
    private bool isTyping = false;

    public TMP_Text textComponent; // Use TMP_Text for TextMeshPro support, otherwise use Text

    void OnEnable()
    {
        StartTyping();
    }

    void Update()
    {
        if (isTyping)
        {
            currentText = fullText.Substring(0, currentCharIndex);
            textComponent.text = currentText;
            currentCharIndex++;

            if (currentCharIndex > fullText.Length)
            {
                isTyping = false;
            }
        }
    }

    void StartTyping()
    {
        currentCharIndex = 0;
        isTyping = true;
        StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        yield return new WaitForSeconds(typingSpeed);

        while (currentCharIndex <= fullText.Length)
        {
            currentText = fullText.Substring(0, currentCharIndex);
            textComponent.text = currentText;
            currentCharIndex++;

            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }
}
