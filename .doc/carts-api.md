[Back to README](../README.md)

### Carts

#### GET /carts
- Description: Retrieve a list of all carts
- Query Parameters:
  - `_page` (optional): Page number for pagination (default: 1)
  - `_size` (optional): Number of items per page (default: 10)
  - `_order` (optional): Ordering of results (e.g., "id desc, userId asc")
- Response: 
  ```json
  {
    "success": true,
    "message": "",
    "data": [
      {
        "id": 1,
        "userId": 1,
        "date": "2024-01-01T00:00:00Z"
      }
    ],
    "errors": null,
    "currentPage": 1,
    "totalPages": 1,
    "totalCount": 0
  }
  ```

#### POST /carts
- Description: Add a new cart
- Request Body:
  ```json
  {
    "userId": 1,
    "date": "2024-01-01T00:00:00Z",
    "products": [
      {
        "productId": 1,
        "quantity": 2
      }
    ]
  }
  ```
- Response: 
  ```json
  {
    "success": true,
    "message": "Cart created successfully",
    "data": {
      "id": 1,
      "userId": 1,
      "date": "2024-01-01T00:00:00Z",
      "cartItems": [
        {
          "id": 1,
          "productId": 1,
          "productTitle": "string",
          "productPrice": 10.50,
          "productImage": "string",
          "quantity": 2
        }
      ]
    },
    "errors": null
  }
  ```

#### GET /carts/{id}
- Description: Retrieve a specific cart by ID
- Path Parameters:
  - `id`: Cart ID
- Response: 
  ```json
  {
    "success": true,
    "message": "",
    "data": {
      "id": 1,
      "userId": 1,
      "date": "2024-01-01T00:00:00Z",
      "cartItems": [
        {
          "id": 1,
          "productId": 1,
          "productTitle": "string",
          "productPrice": 10.50,
          "productImage": "string",
          "quantity": 2
        }
      ]
    },
    "errors": null
  }
  ```

#### PUT /carts/{id}
- Description: Update a specific cart
- Path Parameters:
  - `id`: Cart ID
- Request Body:
  ```json
  {
    "userId": 1,
    "date": "2024-01-01T00:00:00Z",
    "products": [
      {
        "productId": 1,
        "quantity": 3
      }
    ]
  }
  ```
- Response: 
  ```json
  {
    "success": true,
    "message": "Cart updated successfully",
    "data": {
      "id": 1,
      "userId": 1,
      "date": "2024-01-01T00:00:00Z",
      "cartItems": [
        {
          "id": 1,
          "productId": 1,
          "productTitle": "string",
          "productPrice": 10.50,
          "productImage": "string",
          "quantity": 3
        }
      ]
    },
    "errors": null
  }
  ```

#### DELETE /carts/{id}
- Description: Delete a specific cart
- Path Parameters:
  - `id`: Cart ID
- Response: 
  ```json
  {
    "success": true,
    "message": "Cart deleted successfully",
    "data": null,
    "errors": null
  }
  ```


<br>
<div style="display: flex; justify-content: space-between;">
  <a href="./products-api.md">Previous: Products API</a>
  <a href="./users-api.md">Next: Users API</a>
</div>