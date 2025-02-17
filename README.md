# RQL2Expression
**RQL2Expressions** is an ASP.NET application that demonstrates how to transform **RQL** (Resource Query Language) queries into **SQL** queries using **Entity Framework Core** (EF Core). This project is an example and can be used as a foundation for implementing data filtering in your applications.

### What is RQL?
RQL (Resource Query Language) is a query language that allows clients to form complex filters for data retrieval. It supports operations such as eq, in, and, or, limit, and others. RQL is often used in RESTful APIs for server-side data filtering.

### Key Features
RQL to Expression Tree Conversion: The application accepts an RQL query and converts it into an expression tree that can be used in EF Core.

Data Filtering: Based on the RQL query, the application forms an SQL query and returns filtered data.

Basic Operations Support: Supports operations such as *eq, in, and, or, and* wildcard search (e.g., *test*).

### How to Use
Run the application.

Send a GET request to the /search endpoint with the rql parameter. For example:

```
GET /search?rql=eq(dn,1)
```
The application will return filtered data in JSON format.

#### Example Queries
Simple query: ```eq(dn,1)``` — returns records where Id is equal to 1.

Query with and: ```and(eq(dn,1),eq(FirstName,John))``` — returns records where Id is equal to 1 and FirstName is "John".

Query with in: ```in(dn,1,2,3)``` — returns records where Id is equal to 1, 2, or 3.

Installation and Running
Clone the repository:

```
git clone https://github.com/samsonovsa/RQL2Expression.git
```

Install dependencies:

```
dotnet restore
```
Build project
```
docker-compose build
```

Run the application:
```
docker-compose up
```

### Dependencies
- .NET 8
- Entity Framework Core
- PostgreSQL 
- Docker Desktop

### License
This project is licensed under the MIT License. See the LICENSE file for details.
