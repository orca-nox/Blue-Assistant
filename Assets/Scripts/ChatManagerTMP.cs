using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenAI;

public class ChatGPTManager : MonoBehaviour {
    [SerializeField]
    private string apiKey = "";
    [SerializeField]
    private string orgKey = "";
    private OpenAIApi openAI = new OpenAIApi();
    //private OpenAIApi openAI = new OpenAIApi();
    private List<ChatMessage> messages = new List<ChatMessage>();

    [SerializeField]
    private string chatGPTModel; //gpt-3.5-turbo, gpt-4, gpt-4-turbo
    [SerializeField]
    private bool debugMode = true;

    [TextArea(15, 20)]
    public string instructions;
    public string pose;
    public int accuracy;

    private void Awake() {
        openAI = new OpenAIApi(apiKey, orgKey);
    }

    public async void RequestChatGPT(string newText) {
        ChatMessage newMessage = new ChatMessage();
        newMessage.Content = newText;
        newMessage.Role = "user";

        messages.Add(newMessage);

        CreateChatCompletionRequest request = new CreateChatCompletionRequest();
        request.Messages = messages;
        request.Model = chatGPTModel;

        var response = await openAI.CreateChatCompletion(request);

        if (response.Choices != null && response.Choices.Count > 0) {
            var chatResponse = response.Choices[0].Message;
            messages.Add(chatResponse);
            // GameManager.Instance.SetResultTextField(chatResponse.Content);

            Debug.Log(chatResponse.Content);
        }
    }

    string NewPrompt(string instructions, string pose, int accuracy) {

        string str = $"{instructions}";

        //string str = $"{instructions} \n The pose is {pose}. \n The accuracy is {accuracy.ToString()}";

        return str;

    }

    // Start is called before the first frame update
    void Start() {

        // Test
        if (debugMode) RequestChatGPT(NewPrompt(instructions, pose, accuracy));
    }

    // Update is called once per frame
    void Update() {

    }
}
