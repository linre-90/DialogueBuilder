# Dialogue builder

"Dialogue Builder" is a "user-friendly" tool designed for creating dialogue trees in game engines.

The program utilizes the JSON format to store project data. It requires two distinct .json files to function: one for managing "business" logic states and another for the graphical node editor.

Additionally, the program allows users to export data in a CSV format compatible with Unreal Engine. The exported data is written to the [project]/export directory. This CSV format is specifically designed for Unreal Engine data table assets. It consists of two files, one for node data and another for node relationships. Notably, any commas found within text are transformed into ";" characters. Unlike JSON, the CSV format has this limitation.

## Info

To initiate a project, users start by opening a folder, which becomes the project directory. All project-related files are saved within this designated directory. The JSON file is named after the project (e.g., {project}.json) and can be used in any program that supports recursive JSON structures.

As of its current state, the program is in its **Minimum Viable Product (MVP)** phase and offers only basic functionality. Additional features and bug fixes are implemented as needed, primarily to address my specific development requirements.

**This tool primarily serves my own development purposes.**
