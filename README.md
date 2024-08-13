The SampleAPI folder has the solution file.

The solution is modeled using the repository pattern. 

It's using an in-memory database, and when you run the API project in Swagger, there's an "Initialize" API that will create an initial customer (id = 5) and account (id = 17). 
From there, any of the other API calls can be made using the JSON in the documentation. 
Error checking is in place; server errors will return 500 and data errors 400, along with the error. 
Correct calls will return 200 and the required JSON.
