![ASP.NET Core logo](https://miro.medium.com/max/450/1*jgpD8XH0jBOgcUnM_3UxWw.png "ASP.NET Core")

---

# DocNet - An ASP.NET Core REST API, powered by Entity Framework

## Get Started

### 1. Clone Repository

`git clone https://rendu-git.etna-alternance.net/module-6749/activity-38021/group-779220.git`

or

`git@rendu-git.etna-alternance.net:module-6749/activity-38021/group-779220.git`

**All necessary Nuget dependencies will automatically be installed on first build.**

### 2. Open API Solution in Visual Studio

API Solution is located at the root of the Service/ directory.

### 3. Create SQL Server Database

- In Visual Studio, go to View -> **Server Explorer**.
- Expand SQL Server, then expand **(localdb)\\mssqllocaldb**.
- Right-click on "Databases" -> Add New Database.
- Name your Database **"docnet"** (without quotes).

### 4. Init your DocNet Database

- Right-click on your newly-created "docnet" database. Select "New Query".
- Paste the contents of the DocNet SQL file **docnet.sql** located in the Data/ directory.
- Run **"Execute"** (or Ctrl + Shift + E).

Your database is now up and running. **Some data has already been inserted : Roles (roles_rle) & Genders (genders_gdr), and an active ADMIN user (email : string / password : string)**.

### 5. Build Project

- Hit **F5** to build the Solution. A browser window will appear, taking you directly to the Swagger Documentation.

---

## Use Swagger

- Use POST /auth to **retrieve a JWT** for existing ADMIN user string:string (see section 4.).
- Click on the **"Authorize"** button (top right) and type in **"Bearer " + your Jwt**.

You may now use authenticated routes.

---
