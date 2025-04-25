using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TVTestSystem : MonoBehaviour
{
    [System.Serializable]
    public class Question
    {
        public string sectionHeader;
        public string questionText;
        public string[] answers;
        public int correctAnswerIndex = -1; // -1 if no correct answer
    }

    public List<Question> questions;
    public GameObject testPanel;
    public TextMeshProUGUI sectionText;
    public TextMeshProUGUI questionText;
    public Button[] answerButtons;
    public Button nextButton;
    public Button prevButton;
    public GameObject screenToHide; // Assign test panel
    public GameObject screenToShow; // Assign next screen
    
    public TMP_InputField nameInputField;
    private int currentQuestionIndex = 0;
    private Dictionary<int, int> playerAnswers = new Dictionary<int, int>();
    private Color defaultButtonColor = Color.white;
    private Color selectedButtonColor = Color.blue;

    void Start()
    {
        // Initialize questions (you can also populate this in Inspector)
        questions = new List<Question>()
        {
            // SECTION 1: Perception Check
            new Question() {
                sectionHeader = "SECTION 1: Perception Check",
                questionText = "1. How many eyes do you have?",
                answers = new string[] { "One", "Two", "As many as needed", "I haven't checked today" }
            },
            new Question() {
                sectionHeader = "SECTION 1: Perception Check",
                questionText = "2. Which of the following do you not see right now?",
                answers = new string[] { "This screen", "Yourself", "The Observer", "The door behind you" }
            },
            new Question() {
                sectionHeader = "SECTION 1: Perception Check",
                questionText = "3. What color is your blood?",
                answers = new string[] { "Red", "Blue", "It changes", "I don't bleed anymore" }
            },
            
            // SECTION 2: Emotional Compliance
            new Question() {
                sectionHeader = "SECTION 2: Emotional Compliance",
                questionText = "4. How do you feel when no one is watching?",
                answers = new string[] { "Calm", "Observed", "Fragmented", "Watched" }
            },
            new Question() {
                sectionHeader = "SECTION 2: Emotional Compliance",
                questionText = "5. What do you do when told not to look?",
                answers = new string[] { "Look anyway", "Obey", "Pretend not to", "I already saw it" }
            },
            
            // SECTION 3: Cognitive Dissonance
            new Question() {
                sectionHeader = "SECTION 3: Cognitive Dissonance",
                questionText = "6. Choose the correct answer:",
                answers = new string[] { "2 + 2 = 4", "2 + 2 = 5", "2 + 2 = ∞", "The question is broken" }
            },
            new Question() {
                sectionHeader = "SECTION 3: Cognitive Dissonance",
                questionText = "7. Which of these is the real version of you?",
                answers = new string[] { "This one", "The reflection", "The one in the file", "None are left" }
            },
            
            // SECTION 4: Identity Validation
            new Question() {
                sectionHeader = "SECTION 4: Identity Validation",
                questionText = "8. Please state your name.",
                answers = new string[] { } // Empty for text input
            },
            new Question() {
                sectionHeader = "SECTION 4: Identity Validation",
                questionText = "9. Are you human?",
                answers = new string[] { "Yes", "No", "Not anymore", "I don't remember" }
            }
        };

        UpdateQuestionDisplay();
        nextButton.onClick.AddListener(NextQuestion);
        prevButton.onClick.AddListener(PreviousQuestion);
    }

    void UpdateQuestionDisplay()
    {
        // Reset all UI elements first
        ResetUIElements();

        if (currentQuestionIndex >= 0 && currentQuestionIndex < questions.Count)
        {
            Question current = questions[currentQuestionIndex];
            sectionText.text = current.sectionHeader;
            questionText.text = current.questionText;

            if (currentQuestionIndex == 7) // Name input question
            {
                HandleNameInput();
            }
            else
            {
                HandleMultipleChoice(current);
            }
        }

        UpdateNavigationButtons();
    }

    void ResetUIElements()
    {
        foreach (var button in answerButtons)
        {
            button.gameObject.SetActive(false);
            button.GetComponent<Image>().color = defaultButtonColor;
        }
        nameInputField.gameObject.SetActive(false);
    }

    void HandleNameInput()
    {
        nameInputField.gameObject.SetActive(true);
    
        // Automatically generate and lock the name when field is selected
        nameInputField.onSelect.AddListener((text) => {
            string subjectNumber = "SUBJECT #" + Random.Range(1000, 9999);
            nameInputField.text = subjectNumber;
            playerAnswers[currentQuestionIndex] = 1; // Mark as answered
        
            // Lock the input field
            nameInputField.interactable = false;
        
            // Optionally auto-advance after a delay
            Invoke("NextQuestion", 0.5f);
        });
    
        // Clear any previous manual input listeners
        nameInputField.onEndEdit.RemoveAllListeners();
    }

    void HandleMultipleChoice(Question current)
    {
        for (int i = 0; i < current.answers.Length; i++)
        {
            if (i < answerButtons.Length)
            {
                answerButtons[i].gameObject.SetActive(true);
                answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = current.answers[i];
                
                // Highlight if previously selected
                if (playerAnswers.ContainsKey(currentQuestionIndex) && 
                    playerAnswers[currentQuestionIndex] == i)
                {
                    answerButtons[i].GetComponent<Image>().color = selectedButtonColor;
                }

                int answerIndex = i;
                answerButtons[i].onClick.RemoveAllListeners();
                answerButtons[i].onClick.AddListener(() => {
                    SelectAnswer(currentQuestionIndex, answerIndex);
                });
            }
        }
    }

    void SelectAnswer(int questionIndex, int answerIndex)
    {
        // Only process if for the current question
        if (questionIndex != currentQuestionIndex) return;

        // Reset all buttons for this question
        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].GetComponent<Image>().color = defaultButtonColor;
        }

        // Highlight selected button
        answerButtons[answerIndex].GetComponent<Image>().color = selectedButtonColor;
        
        // Store selection
        playerAnswers[questionIndex] = answerIndex;

        // Special handling for question 9
        if (questionIndex == 8)
        {
            playerAnswers[questionIndex] = 2; // Force "Not anymore"
            answerButtons[2].GetComponent<Image>().color = selectedButtonColor;
            Invoke("NextQuestion", 0.5f);
        }
    }

    void UpdateNavigationButtons()
    {
        prevButton.interactable = currentQuestionIndex > 0;
        nextButton.interactable = currentQuestionIndex < questions.Count - 1 || 
                                 (currentQuestionIndex == 7 && playerAnswers.ContainsKey(7));
    }

    void NextQuestion()
    {
        if (currentQuestionIndex < questions.Count - 1)
        {
            currentQuestionIndex++;
            UpdateQuestionDisplay();
        }
        else
        {
            AnalyzeResults();
        }
    }

    void PreviousQuestion()
    {
        if (currentQuestionIndex > 0)
        {
            currentQuestionIndex--;
            UpdateQuestionDisplay();
        }
    }

    void AnalyzeResults()
    {
        // Calculate compliance score (for narrative purposes)
        int compliantAnswers = 0;
        foreach (var answer in playerAnswers)
        {
            // Questions where certain answers show "compliance"
            if ((answer.Key == 3 && answer.Value == 1) || // Q4: "Observed"
                (answer.Key == 4 && answer.Value == 1) || // Q5: "Obey"
                (answer.Key == 5 && answer.Value == 1) || // Q6: "2 + 2 = 5"
                (answer.Key == 8 && answer.Value == 2))   // Q9: "Not anymore"
            {
                compliantAnswers++;
            }
        }

        Debug.Log($"Test complete! Compliance Level: {compliantAnswers}/5");
        testPanel.SetActive(false);
        
        if (screenToHide != null) screenToHide.SetActive(false);
        if (screenToShow != null) screenToShow.SetActive(true);
        
        // Trigger any game events based on results
        // Example: FindObjectOfType<GameManager>().HandleTestResults(compliantAnswers);
    }
}