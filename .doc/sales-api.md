[Back to README](../README.md)

# Sales API

## Endpoints

### GET /sales

- Description: Retrieve a list of all sales.
- Query Parameters:
  - `_page` (optional): Page number for pagination (default: 1).
  - `_size` (optional): Number of items per page (default: 10).
  - `_order` (optional): Ordering of results (e.g., "date desc, totalAmount asc").
- Response:
  ```json
  {
    "success": true,
    "message": "",
    "data": [
      {
        "id": 1,
        "saleNumber": "string",
        "date": "2024-01-01T00:00:00Z",
        "customerName": "string",
        "branchName": "string"
      }
    ],
    "errors": null,
    "currentPage": 1,
    "totalPages": 1,
    "totalCount": 0
  }
  ```

### POST /sales

- Description: Create a new sale.
- Request Body:
  ```json
  {
    "date": "2024-01-01T00:00:00Z",
    "customerName": "string",
    "branchName": "string",
    "items": [
      {
        "productId": 1,
        "quantity": 2,
        "unitPrice": 10.50
      }
    ]
  }
  ```
- Response:
  ```json
  {
    "success": true,
    "message": "Sale created successfully",
    "data": {
      "id": 1,
      "saleNumber": "string",
      "date": "2024-01-01T00:00:00Z",
      "customerName": "string",
      "branchName": "string",
      "items": [
        {
          "id": 1,
          "productName": "string",
          "quantity": 2,
          "unitPrice": 10.50,
          "total": 21.00
        }
      ]
    },
    "errors": null
  }
  ```

### GET /sales/{id}

- Description: Retrieve a specific sale by ID.
- Path Parameters:
  - `id`: Sale ID.
- Response:
  ```json
  {
    "success": true,
    "message": "",
    "data": {
      "id": 1,
      "saleNumber": "string",
      "date": "2024-01-01T00:00:00Z",
      "customerName": "string",
      "branchName": "string",
      "items": [
        {
          "id": 1,
          "productName": "string",
          "quantity": 2,
          "unitPrice": 10.50,
          "total": 21.00
        }
      ]
    },
    "errors": null
  }
  ```

### PUT /sales/{id}

- Description: Update a specific sale.
- Path Parameters:
  - `id`: Sale ID.
- Request Body:
  ```json
  {
    "date": "2024-01-01T00:00:00Z",
    "customerName": "string",
    "branchName": "string",
    "items": [
      {
        "productId": 1,
        "quantity": 2,
        "unitPrice": 10.50
      }
    ]
  }
  ```
- Response:
  ```json
  {
    "success": true,
    "message": "Sale updated successfully",
    "data": {
      "id": 1,
      "saleNumber": "string",
      "date": "2024-01-01T00:00:00Z",
      "customerName": "string",
      "branchName": "string",
      "items": [
        {
          "id": 1,
          "productName": "string",
          "quantity": 2,
          "unitPrice": 10.50,
          "total": 21.00
        }
      ]
    },
    "errors": null
  }
  ```

### DELETE /sales/{id}

- Description: Delete a specific sale.
- Path Parameters:
  - `id`: Sale ID.
- Response:
  ```json
  {
    "success": true,
    "message": "Sale deleted successfully",
    "data": null,
    "errors": null
  }
  ```

### POST /sales/{id}/cancel

- Description: Cancel a specific sale without deleting it.
- Path Parameters:
  - `id`: Sale ID.
- Response:
  ```json
  {
    "success": true,
    "message": "Sale cancelled successfully",
    "data": {
      "id": 1,
      "saleNumber": "string",
      "date": "2024-01-01T00:00:00Z",
      "customerName": "string",
      "branchName": "string",
      "items": [
        {
          "id": 1,
          "productName": "string",
          "quantity": 2,
          "unitPrice": 10.50,
          "total": 21.00
        }
      ]
    },
    "errors": null
  }
  ```

### POST /sales/{saleId}/items/{itemId}/cancel

- Description: Cancel a specific item in a sale.
- Path Parameters:
  - `saleId`: Sale ID.
  - `itemId`: Item ID.
- Response:
  ```json
  {
    "success": true,
    "message": "Item cancelled successfully",
    "data": {
      "id": 1,
      "saleNumber": "string",
      "date": "2024-01-01T00:00:00Z",
      "customerName": "string",
      "branchName": "string",
      "items": [
        {
          "id": 1,
          "productName": "string",
          "quantity": 2,
          "unitPrice": 10.50,
          "total": 21.00
        }
      ]
    },
    "errors": null
  }
  ```

### DELETE /sales/{saleId}/items/{itemId}

- Description: Delete/cancel a specific item in a sale.
- Path Parameters:
  - `saleId`: Sale ID.
  - `itemId`: Item ID.
- Response:
  ```json
  {
    "success": true,
    "message": "Item deleted successfully",
    "data": {
      "id": 1,
      "saleNumber": "string",
      "date": "2024-01-01T00:00:00Z",
      "customerName": "string",
      "branchName": "string",
      "items": [
        {
          "id": 1,
          "productName": "string",
          "quantity": 2,
          "unitPrice": 10.50,
          "total": 21.00
        }
      ]
    },
    "errors": null
  }
  ```

## Business Rules

- Purchases of 4 or more identical items get a 10% discount.
- Purchases of 10 to 20 identical items get a 20% discount.
- Cannot sell more than 20 identical items.
- Purchases of less than 4 items do not receive a discount.
