All projects in this solution were developed using VSCode with the C# extension.
Unit tests were implemented using the XUnit tool.

The BackEnd project contains the .Net Core Web API implementation. This project has the following layers:
    ViewModel: models responsible for the communication between FrontEnd and the API
    Model: models responsible for the communication between the DB and the API
    Controller: layer responsible for performing operation on the database and returning models compatible with the FrontEnd
The ORM Entity Framework tool was used to manipulate database objects.
The FrontEnd project was implemented using HTML 5 + Bootstrap 4 + JQuery 3 + Datatables.net 1.10.

An http server is needed in order to run the FrontEnd.
To install the same http-server used during the implementation of this project, use the command "npm install http-server -g" and to execute it navigate to the folder were the solution file is located and use the command "http-server Fronted". The default port is 8080.

FrontEnd description:
    The website contains a navbar with three menu items: Log list, Upload log files and Add new log.
    In the right corner, there's also two buttons that play the same role as the menu items.

    Log list:
        this page lists in a table the logs returned from the database, according to the search criteria. The table has a pagination feature as well as the number of records to be displayed.
        When clicking on "View details", the system redirects the user to the edit / delete page of the selected record

    Upload log Files
        in this page, the user informs one or more log files to be written to the database.
        The files must respect the pattern shown in the image example that is located inside the PDF file of the Ben Fatto's technical challenge.
        A file named "logs.log" was commited to be used for testing purposes

    Add new log:
        in this page, the user manually informs the data to be inserted in the database
