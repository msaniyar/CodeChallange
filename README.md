# CodeChallange
This solution reads any new .xml file from configured input location, calculates output according to code challange requirements and creates .xml file to configured output location.

Solution consists two .net5 projects. 
## FileWatcherService: 
It is a console application and responsible for listening input location and fire event in case of new .xml file is copied. After event fired backend service is called via package called MediatR.
## Processor
It is a class library which is responsible for calculating generator values and creating output file to given directory.

## Configuration
Configurations are located in the appsettings.json.

Input file location under Input:Location
Output file location under Output:Location

Reference File name and directory are also referred in the appsettings.json file.

Input and Output directories are created by FileWatcherService if they are not exist.

## Solution Run
Solution can be run directly from Visual Studio (via F5 or Ctrl+F5) after pulling the code and building it. Configuration as mentioned above should be changed before running. 

Alternativly project can be run via dotnet commands.
Go To Solution Folder 

#### dotnet build

Go to \FileWatcherService Project Folder

#### dotnet run

## Testing Functionality
Related .xml file should be copied to the input location then result can be observed in the output location.

## Logging
FileWatcherService logs to console output and shows output file name and location and if anything goes wrong it shows error as well.

## Unit Testing
Tests are under UnitTests project. Tests are not covering all functionality entirely. It can be extended.

