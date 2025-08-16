[Back to README](../README.md)

### Users

#### GET /users
- Description: Retrieve a list of all users
- Query Parameters:
  - `_page` (optional): Page number for pagination (default: 1)
  - `_size` (optional): Number of items per page (default: 10)
  - `_order` (optional): Ordering of results (e.g., "username asc, email desc")
- Response: 
  ```json
  {
    "data": [
      {
        "id": "integer",
        "email": "string",
        "username": "string",
        "password": "string",
        "name": {
          "firstname": "string",
          "lastname": "string"
        },
        "address": {
          "city": "string",
          "street": "string",
          "number": "integer",
          "zipcode": "string",
          "geolocation": {
            "lat": "string",
            "long": "string"
          }
        },
        "phone": "string",
        "status": "string (enum: Active, Inactive, Suspended)",
        "role": "string (enum: Customer, Manager, Admin)"
      }
    ],
    "totalItems": "integer",
    "currentPage": "integer",
    "totalPages": "integer"
  }
  ```

#### POST /users
- Description: Add a new user
- Request Body:
  ```json
  {
    "username": "string",
    "email": "string",
    "password": "string",
    "phone": "string",
    "status": "Active",
    "role": "Customer"
  }
  ```
- Response: 
  ```json
  {
    "success": true,
    "message": "User created successfully",
    "data": {
      "id": 1,
      "username": "string",
      "email": "string",
      "phone": "string",
      "status": "Active",
      "role": "Customer",
      "address": null,
      "createdAt": "2024-01-01T00:00:00Z",
      "updatedAt": null
    },
    "errors": null
  }
  ```

#### GET /users/{id}
- Description: Retrieve a specific user by ID
- Path Parameters:
  - `id`: User ID
- Response: 
  ```json
  {
    "success": true,
    "message": "",
    "data": {
      "id": 1,
      "username": "string",
      "email": "string",
      "phone": "string",
      "status": "Active",
      "role": "Customer",
      "address": {
        "city": "string",
        "street": "string",
        "number": 1,
        "zipCode": "string",
        "geolocation": {
          "lat": "string",
          "long": "string"
        }
      },
      "createdAt": "2024-01-01T00:00:00Z",
      "updatedAt": null
    },
    "errors": null
  }
  ```

#### PUT /users/{id}
- Description: Update a specific user
- Path Parameters:
  - `id`: User ID
- Request Body:
  ```json
  {
    "email": "string",
    "username": "string",
    "password": "string",
    "name": {
      "firstname": "string",
      "lastname": "string"
    },
    "address": {
      "city": "string",
      "street": "string",
      "number": "integer",
      "zipcode": "string",
      "geolocation": {
        "lat": "string",
        "long": "string"
      }
    },
    "phone": "string",
    "status": "string (enum: Active, Inactive, Suspended)",
    "role": "string (enum: Customer, Manager, Admin)"
  }
  ```
- Response: 
  ```json
  {
    "id": "integer",
    "email": "string",
    "username": "string",
    "password": "string",
    "name": {
      "firstname": "string",
      "lastname": "string"
    },
    "address": {
      "city": "string",
      "street": "string",
      "number": "integer",
      "zipcode": "string",
      "geolocation": {
        "lat": "string",
        "long": "string"
      }
    },
    "phone": "string",
    "status": "string (enum: Active, Inactive, Suspended)",
    "role": "string (enum: Customer, Manager, Admin)"
  }
  ```

#### DELETE /users/{id}
- Description: Delete a specific user
- Path Parameters:
  - `id`: User ID
- Response: 
  ```json
  {
    "success": true,
    "message": "User deleted successfully",
    "data": null,
    "errors": null
  }
  ```
<br/>
<div style="display: flex; justify-content: space-between;">
  <a href="./carts-api.md">Previous: Carts API</a>
  <a href="./auth-api.md">Next: Auth API</a>
</div>
