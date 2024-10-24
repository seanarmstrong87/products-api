# Setup

## Database

There is a DB migration `20241023185654_InitialCreate.cs` that creates the initial schema for the database. The migration is located in the `Migrations` folder. To run the migration, execute the following command in the ProductsWebApi folder next to the csproj file:

```bash
dotnet ef database update 
```

The connection string is located in the `appsettings.Development.json` file. The connection string is named `DefaultConnection`. Update the connection string to point to your database.

> Note: I found I had to create the DB first before running the initial migration.

The ef core update command should create user related tables and a Product table.

## Authentication

I have used ASP.NET Core Identity for authentication.

Once the db migration is run, you can register a user by navigating to the `/register` endpoint.

![img.png](Images/img.png)AddCookie

After registering, you can log in by navigating to the `/login` endpoint. Make sure to set `useCookies = true` and `useSessionCookies = true`.

![img4.png](Images/img4.png)

The authentication is done using a cookie which should be valid for 30 minutes.

## Requests

There is one controller in the project, the `ProductController`. The controller has the following three endpoints:

1. `GET /api/product` - This endpoint returns all the products in the database or a subset based on filter queries.
e.g. `api/Product?Colour=Yellow&Price.From=10&Price.To=20`
2. `POST /api/product` - This endpoint creates a new product in the database.
3. `PUT /api/product/{id}` - This endpoint updates an existing product in the database.
4. `GET api/health` - This endpoint returns a 200 status code if the application is running.

## Testing

There is a Tests project with some example tests for the ProductRepository and ProductsQueryBuilder. 
The repository tests use an in-memory database to test the repository methods.

Given more time I would add more tests for the input dto validation and integration and load tests which would 
test the endpoints using a test server or spin up a database with some seeded data in a docker container to test against.

## Example screenshots

I have included some example screenshots of the application running from swagger.

- Get all products
![img_2.png](Images/img_2.png)

- Post a product
![img_4.png](Images/img_4.png)

- Update a product
![img_5.png](Images/img_5.png)

## In the context of a larger system
Here is a simplified architecture diagram of how this API could fit into a larger microservices system. Each domain having it's
own API and database. The API Gateway would be responsible for routing requests to the correct API and the message broker service
is responsible for sending messages between the services and ensuring eventual consistency.
![Excalidraw_Export_ProductsAPI.png](Images/Excalidraw_Export_ProductsAPI.png)

## Improvements

- Flesh out the product model with more properties.
- Populate swagger dropdowns with enum strings.
- Improve error handling, e.g. wrong authentication details.
- Setup other methods of authentication.
- Extra ID column (GUID) to track entities across services.
- DB indexes on any columns that are queried on frequently.
- Paging for the get products endpoint.
- Migrations and DB code into separate project
- Proper specification pattern for DB queries
- More xml comments for the API and generate docs from the comments.
- Separate the identity DB from the application DB for use across services