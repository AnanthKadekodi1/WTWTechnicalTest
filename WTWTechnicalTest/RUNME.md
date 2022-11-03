# Instructions for running the application

#### Main Application

The Application has been created as a console App (.NET 6) and has been tested to be working. The output file generated matches the example output data present in README.md. 

To run the project, setup ClaimsReservationConsoleApp as the startup project. Running build will download the nuget packages. 

#### Input and Output Files

The Application will pickup the input files (sample input file provided present in the InputFile Folder) and Output a csv file with calculated data. 

The paths for the input and output file location are present in app.config file. 

#### Unit Tests

Unit tests have been written to test various scenarios such as - Calculating with two triangle; calculating with three triangles, calculating with missing columns etc. 

The units tests are working and there should be no failing tests.

The csv files that the units tests are using are present in the TestFiles folder. 

#### NUGET Packages Required

The following NUGET packages are required for the successful running of the project:
1) Coverlet.collector
2) Microsoft.NET.Test.Sdk
3) NUnit
4) NUnit.Analyzers
5) System.Configuration.ConfigurationManager
6) TinyCSVParser (open source for CSV Parsing)
7) Unity
8) UnityAutoMoq



