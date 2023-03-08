# Voice 2 GPT - voice chat with OpenAI ChatGPT model

This project is focused on voice communication with ChatGPT.

The **user speaks into the computer's microphone**, and the application **transforms the audio recording into text using the
OpenAI Whisper** service.
Then, if necessary, this text is translated into English (this functionality is not yet implemented).
**Afterwards, the OpenAI Chat service API is called**, using the 'gpt-3.5-turbo-0301' model. **The response is then transformed
back into audio using the built-in sound synthesis support in the MS Windows operating system**.

(Note: It is recommended to have this (Speech support in windows) support installed for the language you are communicating in,
otherwise the output will not work. To check if the support is installed, go to settings -> time & language -> speech ->
installed voice packages.
You can also use the `list-voices` command to list all available voices. Like this: `voice2gpt list-voices`)

**🛑 DISCLAIMER: This project was created in my spare time during 2 evenings just for fun. Do not ask me to make urgent
fixes or add new features. I may consider it, but I cannot promise anything. My time is limited. I have a job, a family,
dog, garden, etc.**

## Prerequisites

Before you begin, ensure you have met the following requirements:

1. You have installed the version of .NET Core SDK 7.0 or higher.

   To install .NET Core SDK, follow these steps:
    1. Download the .NET Core SDK installer from https://dotnet.microsoft.com/en-us/download/dotnet/7.0
    2. Run the installer and follow the instructions.
    3. Verify the installation by running the following command:
        ```
        dotnet --version
        ```

2. Windows 10 or higher
    - This application uses the Microsoft Speech Platform, which is only available on Windows 10 or higher.
      **In the future I plan to make implementations for Azure cognitive services and Eleven labs AI text to speech services.**

## Installation

To install and run this project, follow these steps:

1. Clone this repository to your local machine.
2. Navigate to the solution folder 'src'.
3. Install the required dependencies by running the following command:

    ```
    dotnet restore
    ```

4. Run the application by running the following command:

    ```
    dotnet build
    ```

## Configuration

The application is configured using the `appsettings.json` file. The following parameters can be configured:

```json
{
  "OpenAIServiceOptions": {
    "ApiKey": "",
    "OrgKey": ""
  },
  "DeepLTranslatorOptions": {
    "ApiKey": ""
  }
}
```

The **OpenAI API** key (`OpenAIServiceOptions:ApiKey`) and organization key
(`OpenAIServiceOptions:OrgKey`) can be obtained by registering at https://platform.openai.com/
and creating a new API key. The organization key is not required, but it is recommended to use
it to avoid rate limiting. You can find the organization key at https://platform.openai.com/account/org-settings.

The **DeepL API** key (`DeepLTranslatorOptions:ApiKey`) can be obtained by
registering at https://www.deepl.com/pro and creating a new API key.
**This is not required at the moment, because the translation functionality is not yet implemented**.


## Usage

### Installation

To use this project acts as dotnet global tool, follow these steps:

1. Navigate to the solution folder 'src'.
2. Install the tool by running the following command:

    ```
    dotnet pack ..\src\Voice2Gpt -c Release -o nupkg
    dotnet tool install --global --add-source ..\src\Voice2Gpt\nupkg Voice2Gpt.App.CLI
    ```

3. Run the application by running the following command for the first time:

    ```
    voice2gpt --help
    ```

### Update

1. Navigate to the solution folder 'src'.
2. Update the tool by running the following command:

    ```
    dotnet pack ..\src\Voice2Gpt -c Release -o nupkg
    dotnet tool update --global --add-source ..\src\Voice2Gpt\nupkg Voice2Gpt.App.CLI
    ```

3. Run the application by running the following command for the first time:

    ```
    voice2gpt --help
    ```

## Command-line usage

This project is designed as a console application. It accepts the following parameters as input:

- `chat`: Starts the chat with the chatbot
    - Configuration parameters:
        - `-l` or `--log`: Enables logging (default: false)
        - `-il` or `--input-language`: Input language you will speak in (default: en)
        - `-d` or `--device`: Microphone device number (default: 0)

- `list-devices`: Lists all available microphone devices
    - Configuration parameters:
        - `-l` or `--log`: Enables logging (default: false)
- `list-voices`: Lists all available voices
    - Configuration parameters:
        - `-l` or `--log`: Enables logging (default: false)
        -

The following command will transcribe speech from device 1 (the default is 0) input language 'sk'

    voice2gpt -d 1 -il sk

The following command will transcribe speech from the defaut device 0 input language 'sk'

    voice2gpt -il sk

The following command will transcribe speech from the defaut device 0 default input language 'en' and enable logging

    voice2gpt -l

Run the following to view all available options:

    voice2gpt --help

## Components

The following components are used in this project:

1. [OpenAI ChatGPT](https://beta.openai.com/docs/engines/chat-gpt) - for the main chat functionality
2. [OpenAI Whisper](https://beta.openai.com/docs/engines/whisper) - for speech transcription
3. [DeepL Translator](https://www.deepl.com/docs-api/translating-text/request/) - for text translation
4. [Microsoft Speech Platform](https://docs.microsoft.com/en-us/previous-versions/windows/desktop/ms723627(v=vs.85)) - for
   speech synthesis
6. [NAudio package](https://github.com/naudio/NAudio) - for audio recording and playback

## License

This project is released under the MIT License.
See [LICENSE](https://github.com/zemacik/Voice2Gpt/blob/main/LICENSE) for further details.
