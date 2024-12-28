using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenAI;
using TMPro;
using System.IO;




public class ChatGPTManager : MonoBehaviour {
    [SerializeField]
    public string apiKey = "";
    [SerializeField]
    public string orgKey = "";
    private OpenAIApi openAI = new OpenAIApi();
    //private OpenAIApi openAI = new OpenAIApi();
    private List<ChatMessage> messages = new List<ChatMessage>();

    [SerializeField]
    public string chatGPTModel; //gpt-4o

    [SerializeField]
    private TextMeshProUGUI outputUI;

    [SerializeField]
    string systemPrompt;
    [SerializeField]
    string assistantPrompt;
    [SerializeField]
    string userPrompt;

    [SerializeField] 
    private PlanaOutputText planaOutput;

    [SerializeField]
    private PlanaController planaController;

    [SerializeField]
    SettingsParser settingsParser;

    [SerializeField]
    private GameObject planaLoading;

    public static ChatGPTManager Instance; 

    private void Awake() {
        //apiKey = settingsParser.GetApiKey();
        //orgKey = settingsParser.GetOrgId();
        //chatGPTModel = settingsParser.GetModel();


        if (Instance != null && Instance != this) {
            Destroy(this);
        } else {
            Instance = this;
        }
    }

    public void Start() {
        SetInitSystemPrompt();
        SetInitAssistantPrompt();
        SetInitUserPrompt();
        
    }

    public async void RequestChatGPT(string userPrompt) {
        openAI = new OpenAIApi(apiKey, orgKey);
        messages.Clear();
        ChatMessage newMessage = new ChatMessage();

        newMessage.Role = "system";
        newMessage.Content = systemPrompt;

        messages.Add(newMessage);

        newMessage.Role = "assistant";
        newMessage.Content = assistantPrompt;


        messages.Add(newMessage);

        newMessage.Role = "user";
        newMessage.Content = userPrompt;
        assistantPrompt += "user: " + userPrompt + "\n";

        messages.Add(newMessage);

        CreateChatCompletionRequest request = new CreateChatCompletionRequest();
        request.Messages = messages;
        request.Model = chatGPTModel;

        Debug.Log("Sent request: " + userPrompt);

        planaLoading.SetActive(true);
        var response = await openAI.CreateChatCompletion(request);

        planaLoading.SetActive(false);

        if (response.Choices != null && response.Choices.Count > 0) {
            var chatResponse = response.Choices[0].Message;
            Debug.Log("Received: " + chatResponse.Content);

            assistantPrompt += "plana: " + chatResponse.Content + "\n";
            messages.Add(chatResponse);

            planaController.ProcessLLMResponse(chatResponse.Content);

        }
    }

    string NewPrompt(string instructions, string pose, int accuracy) {

        string str = $"{instructions}";

        return str;

    }

    private void SetInitSystemPrompt() {
        string path = Path.Combine(Application.streamingAssetsPath, "systemPrompt.txt");
        systemPrompt = File.ReadAllText(path);
    }

    private void SetInitAssistantPrompt() {
        assistantPrompt = "";
    }

    private void SetInitUserPrompt() {
        userPrompt = "";

    }


}
