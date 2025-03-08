# geoloctest
Utility Installation and Running:
To install the utility locally follow these steps:
1) Open Command Prompt
2) In Command Prompt navigate to the repository folder .../FetchTest2
3) Enter this command: dotnet tool install --add-source ./geoloctest/nupkg geoloctest

You can now call the utility by command line call: dotnet tool run geoloc-util "abc" "xyz" "etc"...
The utility is setup to receive a series of strings, and processes each of them as a name or zip code.  
Note1: The utility isn't setup to use the argument "--locations".  This was excluded for 2 reasons: (1) the argument wasn't declared as a requirement in the test prompt, only as a *possible* example; (2) the utility only performs one action, so the argument would have been redundent. 
Note2: I timeboxed my implementation of this utility, but it took longer than expected.  Since I was running low on time, I also didn't take the time to implement any other arguments, even "standard" ones like --help.

Once you are done using the utility in the command prompt, you can uninstall it by calling this command: dotnet tool uninstall geoloctest


Running Tests:
This is relatively straight forward:
1) Navigate to the repository folder .../FetchTest2/geoloctest
2) Open the project file geoloctest
3) Build the project
4) Open the Test menu and select Run All Tests
Note that the tests can be found in the file .../FetchTest2/TestUtility/GeoLocUtilityTests.cs
