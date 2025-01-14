# Semantic Kernel Demo

> January 16, 2024 .net Guild Meeting

Based on [Microsoft Learn Documentation](https://learn.microsoft.com/en-us/semantic-kernel/overview/)

## Prerequisites

- [Ollama](https://ollama.com/)
- [Visual Studio Code](https://code.visualstudio.com/)

Optional prerequisites (not needed if using the dev container)
- [.net SDK 8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Polyglot Notebooks Extension (VS Code)](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.dotnet-interactive-vscode)

## Using the Dev Container

- If you have the recommended [Dev Containers](https://marketplace.visualstudio.com/items?itemName=ms-vscode-remote.remote-containers) extension installed then it is possible to use a dev container for the best experience
- You will still need to install and start [Ollama](https://ollama.com/)

## Notebooks

- `01_GettingStarted.ipynb`
  - A gentle start using Ollama to host a local LLM instance and a small amount of .net code to perform a simple request response to the LLM.
  - We start by installing the NuGet packages, downloading the appropraite models, and finish by learning the answer to life, the universe, and everything.
- `02_ChatHistory.ipynb`
  - Learn about the `ChatHistory` class and how the LLM understands the back and forth nature of your interactions.
