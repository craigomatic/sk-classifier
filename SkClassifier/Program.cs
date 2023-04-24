using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Orchestration;
using SkClassifier;
using SkClassifier.Model;
using System.Reflection;


//configure your Azure OpenAI backend
var key = "";
var endpoint = "";
var model = "gpt-35-turbo";

//use text completion endpoints by uncommenting this next line and using a model such as text-davinci-003
//var sk = Kernel.Builder.Configure(c => c.AddAzureOpenAITextCompletionService(model, model, endpoint, key)).Build();

//use chat completion
var sk = Kernel.Builder.Configure(c =>
        c.AddAzureChatCompletionService(
            "chat-completion",
            model, 
            endpoint, 
            key, 
            true))
    .Build();

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


