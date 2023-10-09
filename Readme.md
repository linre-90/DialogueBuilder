# Dialogue builder

"Dialogue Builder" is a "user-friendly" tool designed for creating dialogue trees for game engines.

The program utilizes the JSON format to store project data. It requires two distinct .json files to function: one for managing "business" logic states and another (ui\_{project}.json) for the graphical node editor. The {project}.json file can be used by any engine that supports decent serializing.

Additionally, the program allows users to export data in a basic CSV format and also Unreal Engine compatible format. The exported data is written to the [project]/export directory. Unreal CSV format is specifically designed for Unreal Engine data table assets. It consists of two files, one for node data and another for node relationships. Notably, any commas found within text are transformed into ";" characters. Unlike JSON, the CSV format has this limitation.

## Info

To initiate a project, user starts by opening a folder, which becomes the project directory and project name. All project-related files are saved within this designated directory. The JSON file is named after the project (e.g., {project}.json). Export folder is located under project directory. CSV files are named with pattern {prefix}\_{project}\_{suffix}

**Prefixes**

- UE, for unreal data table assets.
- CSV, for regular csv format.

**Suffixes**

- data, saves information about node.
- relation, saves info about node relationships.

As of its current state, the program is in its **Minimum Viable Product (MVP)** phase and offers only basic functionality. Additional features and bug fixes are implemented as needed, primarily to address my specific development requirements.
