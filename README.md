Before starting application, 
  user should download DB Browser, to be able to connect with the database. Download the latest 7 version.
  our program does not support net 8.0
  In DB Browser, 
    press 'Open database' or 'Database openen',
    then select 'database.db' under the 'net7.0' files.
    Once opened,
      select 'Create table', 
      name it 'Feedbacks',
      add primary key INTEGER 'ID',
      add INTEGER 'ReservationNumber',
      add TEXT 'Email',
      add INT 'Rating',
      add TEXT 'Message'.
  Install these NuGet Packages and correct versions:
    Colorful.Console | Version 1.2.15
    coverlet.collector by tonerdo | Version 3.2.0
    Microsoft.EntityFrameworkCore | Version 7.0.14
    Microsoft.EntityFrameworkCore.Design | Version 7.0.14
    Microsoft.EntityFrameworkCore.Sqlite | Version 7.0.14
    Microsoft.EntityFrameworkCore.Tools  | Version 7.0.14
    Microsoft.NET.Test.Sdk | Version 17.7.1
    MSTest.TestAdapter   | Version 3.1.1
    MSTest.TestFramework | Version 3.1.1
    Newtonsoft.Json | Version 13.0.3
  Open the solution in visual studio and delete the 3 test projects that could cause an error. 
    The projects are: SpecialDishTest, DishTest, DrinksMenuTest
      
When starting application, 
  make sure the window size of cmd is maximum. (Not fullscreen! [f11])


(When text is bigger than the console screen, sometimes the console.clear() isn't working properly) you'll see this as you scroll to much upwards.
