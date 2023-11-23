# Puppy Paw
 There is a project, made by Anton Kaplin (@tedecti) and Artyom Karmykov (@AgaspherGames) on C# + ASP.NET Core and Entity Framework Core with PostgreSQL database

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
Takes email and username, checks it for unique values and returns bool: true for unique, false for non-unique 

```
POST: /login
```
Takes email and password for login user
Returns necessary information to frontend developer, including JWT Token and 200 Code

## User (api/User)
```
GET: /{id}
```
Returns data about user by their id:
- Firstname
- Lastname
- Username
- Avatar
- Friends and followers info
- User's pets
- User's posts
Returns 200 Code
```
GET: /me
```
Returns data about logged in user (required authorize):
- Firstname
- Lastname
- Username
- Avatar
- Friends and followers info
- User's pets
- User's posts
Returns 200 Code

```
PUT: /edit
```
Allows user edit theit firstname or lastname (required authorize)
Returns 200 Code

```
POST: /upload
```
Takes images in .jpg, .jpeg, .png, .webp, saves it in local files, returns uuid of the image in response, and set path as profile picture of user
Returns 200 Code
## Search (api/Search)
```
GET: ?query={value}
```
Inside the query parameters, what the user wants to find is written in, a search is carried out by users and posts
Returns 200 Code

## Post (api/Post)
```
POST: /api/Post
```
Accepts data for further uploading information in the form of a post on the network(required authorize):
- Title
- Description
- Images (limit of 10)
Returns 201 Code

```
GET: api/Post
```
Returns array of all of the posts by relevance:
- Title
- Description
- Imgs
- UploadDate
- Sender's info

// Other info soon
