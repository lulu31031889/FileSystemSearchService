# FileSystemSearchService

A File System Search Service that started out as a (very) rough Proof-of-Concept and is evolving into a more professional project.

## Features
- Uses ElasticSearch for search indexing.
- Fast index updates.

## Limitations
This is evolving out of a (very) rough Proof-of-Concept so there are some known bugs (and probably some unknown bugs too!) so please read the licence especially regarding liability.
- The Service works with a flat storage structure - it has not been tested with a nested folder structure.
- Files without an extension are deemed to be folders to the processing of these is currently unknown.
- The service currently monitors only one folder.

## Prerequisites
- Windows 10.
- Visual Studio 2019.
- .NET 5.0.
- ElasticSearch (it must be running and you must have read/write privileges to it).
- Kibana (at the moment for demo purposes and up until the UI has been completed).

## Tested on
This code has been tested on the following platform:
- Windows 10 Version 20HS (OS Build 19042.928)
- Visual Studio 2019 Version 16.9.4
- ElasticSearch Version 7.12.0
- Kibana Version 7.12.0

## How to use
- Download the repository and open it in Visual Studio 2019.
- Start your instance of ElasticSearch.
- When ElasticSearch is running, start Kibana and wait for it to be running.
- Take a look at the main **FileSystemSearchService** project and open up the appsettings.json file.
- Create a folder on your hard drive that you wish to be monitored for indexing.  In the appsettings.json it's called C:\FolderToBeMonitored but you can set it differently there.
- Also in the appsettings.json is the connection string to ElasticSearch - make sure the url and port matches your running instance.
- In Visual Studio, start debug, or build and run the executable.
- In Kibana "Discover" the index "artifacts" - there should be no documents in there.
- Create a file, or drag and drop a file, to the folder, and refresh the Kibana "Discover" page.  You will now see an indexed document in there.
- Rename an existing file in the folder and refresh the Kibana "Discover" page.  The index will be updated.
- Delete an existing file from the folder and refresh the Kibana "Discover" page.  The index will be updated.
