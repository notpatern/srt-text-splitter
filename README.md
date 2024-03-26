# Srt text Splitter

This project provides a solution for generating Json files from Srt files and batching the subtitles content into batch of approximately \{userchoice\} words.

## Features

- **Srt to Json parser**: Parses a Srt file to a Csharp Json Object.
- **Batch text into {userchoice} words**: Edits the Json Object to group subtitle contents in group of words.

## Usage

1. **Choose an output directory**

2. **Choose .srt files to batch the content of**

3. **Run the command in your command prompt**: srt-text-splitter /<output-directory/> \<amount-of-words-per-batch\> \<any-amount-of-srt-files-paths\> \<any-amount-of-srt-files-paths\> ...

4. **Monitor Output**: The script will write the Json files into the chosen output directory.

## License

This project is licensed under the [MIT License](https://opensource.org/license/mit)
