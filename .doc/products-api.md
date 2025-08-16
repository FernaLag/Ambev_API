[Back to README](../README.md)

### Products

#### GET /products
- Description: Retrieve a list of all products
- Query Parameters:
  - `_page` (optional): Page number for pagination (default: 1)
  - `_size` (optional): Number of items per page (default: 10)
  - `_order` (optional): Ordering of results (e.g., "price desc, title asc")
  - `Category` (optional): Filter products by category name
- Response: 
  ```json
  {
    "success": true,
    "message": "",
    "data": [
      {
        "id": 1,
        "title": "string",
        "price": 10.50,
        "category": "string",
        "rating": {
          "rate": 4.5,
          "count": 100
        }
      }
    ],
    "errors": null,
    "currentPage": 1,
    "totalPages": 1,
    "totalCount": 0
  }
  ```

#### POST /products
- Description: Add a new product
- Request Body:
  ```json
  {
    "title": "string",
    "price": 10.50,
    "description": "string",
    "category": "string",
    "image": "string",
    "rating": {
      "rate": 4.5,
      "count": 100
    }
  }
  ```
- Response: 
  ```json
  {
    "success": true,
    "message": "Product created successfully",
    "data": {
      "id": 1,
      "title": "string",
      "price": 10.50,
      "description": "string",
      "category": "string",
      "image": "string",
      "rating": {
        "rate": 4.5,
        "count": 100
      },
      "createdAt": "2024-01-01T00:00:00Z",
      "updatedAt": "2024-01-01T00:00:00Z"
    },
    "errors": null
  }
  ```

#### GET /products/{id}
- Description: Retrieve a specific product by ID
- Path Parameters:
  - `id`: Product ID
- Response: 
  ```json
  {
    "success": true,
    "message": "",
    "data": {
      "id": 1,
      "title": "string",
      "price": 10.50,
      "description": "string",
      "category": "string",
      "image": "string",
      "rating": {
        "rate": 4.5,
        "count": 100
      },
      "createdAt": "2024-01-01T00:00:00Z",
      "updatedAt": "2024-01-01T00:00:00Z"
    },
    "errors": null
  }
  ```

#### PUT /products/{id}
- Description: Update a specific product
- Path Parameters:
  - `id`: Product ID
- Request Body:
  ```json
  {
    "title": "string",
    "price": 10.50,
    "description": "string",
    "category": "string",
    "image": "string",
    "rating": {
      "rate": 4.5,
      "count": 100
    }
  }
  ```
- Response: 
  ```json
  {
    "success": true,
    "message": "Product updated successfully",
    "data": {
      "id": 1,
      "title": "string",
      "price": 10.50,
      "description": "string",
      "category": "string",
      "image": "string",
      "rating": {
        "rate": 4.5,
        "count": 100
      },
      "createdAt": "2024-01-01T00:00:00Z",
      "updatedAt": "2024-01-01T00:00:00Z"
    },
    "errors": null
  }
  ```

#### DELETE /products/{id}
- Description: Delete a specific product
- Path Parameters:
  - `id`: Product ID
- Response: 
  ```json
  {
    "success": true,
    "message": "Product deleted successfully",
    "data": null,
    "errors": null
  }
  ```

#### GET /products/categories
- Description: Retrieve all product categories
- Response: 
  ```json
  {
    "success": true,
    "message": "",
    "data": [
      "string"
    ],
    "errors": null
  }
  ```



<br>
<div style="display: flex; justify-content: space-between;">
  <a href="./general-api.md">Previous: General API</a>
  <a href="./carts-api.md">Next: Carts API</a>
</div>