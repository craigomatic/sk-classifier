using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.KernelExtensions;
using Microsoft.SemanticKernel.Orchestration;
using SkClassifier;
using SkClassifier.Model;
using System;
using System.Reflection;
using System.Security.Cryptography;


//note that currently this is configured to work only with text-davinci-003 (gpt-35-turbo/gpt-4 support will be added also)

//configure your Azure OpenAI backend
var key = "";
var endpoint = "";
var model = "text-davinci-003";


var sk = Kernel.Builder.Configure(c => c.AddAzureOpenAITextCompletionService(model, model, endpoint, key)).Build();
const string skillName = "Classifier";
const string functionName = "ClassifyPrompt";

sk.CreateSemanticFunction(Assembly.GetEntryAssembly().LoadEmbeddedResource("SkClassifier.skprompt.txt"),
    functionName,
    skillName,
    maxTokens: 2048);

//TODO: pull from storage
var classifications = new[]
{
   "Food",
   "Animals",
   "Places"
};

//TODO: pull from storage also
var toClassify = new[]
{
    new PromptHistory
    {
        Id = Guid.NewGuid().ToString(),
        Message = "I ate some Pizza yesterday that was out of this world!"
    },
    new PromptHistory
    {
        Id = Guid.NewGuid().ToString(),
        Message = "can you help me plan a trip to hawaii?"
    },
    new PromptHistory
    {
        Id = Guid.NewGuid().ToString(),
        Message = "Why do racoons only come out at night?"
    }
};


foreach (var item in toClassify)
{
    var contextVariables = new ContextVariables(item.Message);
    contextVariables.Set("classifications", string.Join("\n", classifications));
    contextVariables.Set("default", "No match");
    
    var classificationResult = await sk.RunAsync(contextVariables, sk.Skills.GetSemanticFunction(skillName, functionName));

    Console.WriteLine($"[{classificationResult.Result.Trim()}] {item.Message}");
}


