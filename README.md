## How to run

To run this project, you need to clone this repository. Start LibraryAPI application. Then Swagger page should appear. If it isn't - open https://localhost:7002/swagger/index.html.

To create requests (get, post, put, delete) you need to authorize. You should make a "get" request to the user, copy the user's data, then insert this data into the "post" auth method and get a token for authorization. Then you should to insert this token in request headers like "Authorize": '{your token without brackets}'
Or click on button with "lock" icon in swagger page and insert there your token like: '{your token without this brackets}'.

The lifetime of the Jwt token is 5 minutes. You can change it in the "LibraryApi/appsettings.json"
