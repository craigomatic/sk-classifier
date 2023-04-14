// Copyright (c) Microsoft. All rights reserved.

using Azure.AI.OpenAI;
using Microsoft.SemanticKernel.AI.TextCompletion;

namespace SkClassifier;

public class AzureOpenAIChatCompletionShim : ITextCompletion
{
    private class UserRoles
    {
        internal static string System = "system";
        internal static string User = "user";
    }

    private readonly string _key;
    private readonly string _endpoint;
    private readonly string _deploymentOrModelId;
    private readonly string _label;

    public AzureOpenAIChatCompletionShim(string label, string deploymentOrModelId, string endpoint, string key)
    {
        this._key = key;
        this._endpoint = endpoint;
        this._deploymentOrModelId = deploymentOrModelId;
        this._label = label;
    }

    public async Task<string> CompleteAsync(string text, CompleteRequestSettings requestSettings, CancellationToken cancellationToken = default)
    {
        var client = new OpenAIClient(new Uri(this._endpoint), new Azure.AzureKeyCredential(this._key));
        var chatCompletionsOptions = new ChatCompletionsOptions
        {
            Messages = { new ChatMessage(ChatRole.User, text) }
        };
        
        var result = await client.GetChatCompletionsAsync(this._deploymentOrModelId, chatCompletionsOptions, cancellationToken);

        return result.Value.Choices.First().Message.Content;
    }
}

