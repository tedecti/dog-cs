# Puppy Paw
 There is a social network for people with their pets, made by Anton Kaplin (@tedecti) and Artyom Karmykov (@AgaspherGames) on C# + ASP.NET Core and Entity Framework Core with PostgreSQL database

 Original frontend was made in React Native. 
 
 This works was made for Course Work in ASTANA Polytechnic College.

 REST.API Docs:

## Auth (api/UserAuth)
```
 POST: /register
```
 Register user by their:
 - Firstname
 - Lastname
 - Email
 - Password
 - Username

For this endpoint used AuthController and UserRepository with username and email unique check.
Return 201 Code

```
POST: /unique
```
Takes:
- Email
- Username
Checks it for unique values and returns bool: true for unique, false for non-unique 

```
POST: /login
```
Takes:
- Email
- Password
Returns necessary information to frontend developer, including JWT Token and 200 Code

## User (api/User)
```
GET: /{id}
```
Returns data about user by their id:
1. Firstname
2. Lastname
3. Username
4. Avatar
5. Friends and followers info
6. User's pets
7. User's posts
Returns 200 Code
```
GET: /me
```
Returns data about logged in user (required authorize):
1. Firstname
2. Lastname
3. Username
4. Avatar
5. Friends and followers info
6. User's pets
7. User's posts
Returns 200 Code

```
PUT: /edit
```
Allows user edit theit firstname or lastname (required authorize)
Returns 200 Code

```
POST: /upload
```
Takes:
- Images in .jpg, .jpeg, .png, .webp (required  authorize)
Endpoint saves image in local files, returns uuid of the image in response, and set path as profile picture of user 
Returns 200 Code

## Friends (api/Friends)
```
GET: /{id}
```
Returns data about friends of this user in array:
1. Avatar
2. Username
3. Firstname
4. Lastname
Returns 200 Code

```
GET: /{id}/check
```
Returns bool value for check, if authorized user and user by id is friends (required  authorize) 
Return 200 Code
```
POST: /{id}
```
Set authorized user and user by id as friends in db table (required authorize)
Return 201 Code

```
DELETE: /{id}
```
Unfriend authorized user and user by id and deleting row in db table (required authorize)
Returns 204 Code

## Post (api/Post)
```
POST: /
```
Accepts data for further uploading information in the form of a post on the network (required authorize):
- Title
- Description
- Images (limit of 10)
Returns 201 Code

```
GET: /
```
Returns array of all of the posts by relevance:
1. Title
2. Description
3. Imgs
4. UploadDate
5. Sender's info

```
GET: /{id}
```
Returns data about post by id: 
1. Title
2. Description
3. Imgs
4. UploadDate
5. Sender's info
Returns 200 Code

## Like (api/Like)
```
POST: /{id}
```
Set like on post by id and add info about it in db table (required authorize)
Returns 201 Code

```
DELETE: /{id}
```
Unlike post by id and delete row in db table (required authorize)
Returns 204 Code

## Commentaries (api/Commentary)
```
GET: /{id}
```
Returns all of the commentaries for one post by id:
1. Total count of commentaries
2. Comments with post info, user info and text
Returns 200 Code

```
POSt: /{id}
```
Adds commentary to a post by id (required authorize):
- Text
Returns 201 Code

## Pets (api/Pets)
```
POST: /
```
Adds a new pet to the user by entered data (required authorize):
- Name
- Passport number
- Images
Returns 201 Code

```
GET: /{id}
```
Returns pet by id:
1. Name
2. Passport number
3. Images
4. Array of documents for pet
Returns 200 Code

### Documents (api/Document)
```
POST: /{id}
```
Adds documents for pet by id (required authorize): 
- Title
- Description
- Images of documents
Returns 201 Code

```
GET: /{id}
```
Returns data about document by document id:
1. Title
2. Description
3. Images
4. Upload date
5. Info about pet
Returns 200 Code

```
GET: /Pet/{id}
```
Returns all documents by id of the pet:
1. Title
2. Description
3. Images
4. Upload date
Returns 200 Code

## Files (api/Files)
```
GET: /{uuid}
```
Returns image by its uuid, images saves in local folder /Images with uuid in name and searched by uuid\
Returns 200 Code
## Search (api/Search)
```
GET: ?query={value}
```
Inside the query parameters, what the user wants to find is written in, a search is carried out by users and posts
Returns 200 Code
