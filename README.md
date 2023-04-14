# sk-classifier
This small sample project shows how to perform basic classification on a series on prompts, against a pre-defined series of classifications using [Semantic Kernel](https://github.com/microsoft/semantic-kernel)

## Usage
1. Get your keys from Azure OpenAI and paste into [Program.cs](SkClassifier/Program.cs) - if you are using Open AI, you'll need to edit line 19.
2. Run the sample and observe the output:

<img width="940" alt="image" src="https://user-images.githubusercontent.com/146438/231605776-fd0bb543-3f27-4d15-9560-feb0aefe9264.png">

Modify the classifications array and the toClassify array to match your scenario. Some adjustment to the [skprompt.txt](SkClassifier/skprompt.txt) may be required, depending on your scenario.
