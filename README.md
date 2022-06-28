+ BikesCo

*********************************************************************************
*  For the easiest interraction with the project, Visual Studio 2019 is preffered
*
*  Everything should be configured and installed in the repository
*
*  The App uses a SqlServer database by the name of "BicyclesTest1.4"
*
*  You have 2 options:
*  either create a database on your localhost port by the name 
*  "BicyclesTest1.4", 
*  or create a new database by any name and change the connection string 
*  located in the "appsettings.json" file to the connection string of your 
*  new database (just change "Initial Catalog=BicyclesTest1.4" 
*  to "Initial Catalog=*new name*")
*
*  After creating the database a first migration is needed to populate it
*  with the seeder data
*
*  for this, you should open the "Package Management Console" in visual studio
*  and enter the following command "add-migration <migration name ex: first>"
*  
*  after the success of the migration enter the next command "update-database"
*  and everything should be setup
*
*  you can check the database in any SqlServer management tool (a default manager
*  is included in visual studio 2019 in the Server Explorer tab)
*
*  the seeder data includes 
*  - 1 admin with username: admin, password: admin
*  - 1 customer with username: test1, password: test1
*
*  the application is ready to be tested
***********************************************************************************
