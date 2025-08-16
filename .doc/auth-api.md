[Back to README](../README.md)


### Authentication

#### POST /auth
- Description: Authenticate a user
- Request Body:
  ```json
  {
    "email": "string",
    "password": "string"
  }
  ```
- Response: 
  ```json
  {
    "success": true,
    "message": "",
    "data": {
      "id": 1,
      "name": "string",
      "email": "string",
      "phone": "string",
      "role": "string",
      "token": "string"
    },
    "errors": null
  }
  ```

<br/>
<div style="display: flex; justify-content: space-between;">
  <a href="./users-api.md">Previous: Users API</a>
  <a href="./project-structure.md">Next: Project Structure</a>
</div>
