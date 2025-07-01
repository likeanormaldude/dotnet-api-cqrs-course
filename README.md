# .NET 8 API Course

### Description

Clean Architecture, user Identity system and Azure deployment using CI/CD + practical exercises.

## Common Nuget Console commands

Commands and generation and very useful when using Entity Framework to interact with the database. <br>
In the package manager console on Visual Studio, make sure "Default project" is set correctly, that is, the incoming commands will take effect on the right project inside the solution.

#### Add migration command

```cmd
Add-Migration "migration_name"
```

#### Apply migration and update the DB

```cmd
Update-Database
```

#### Csharpier extension
It's recommended to install [csharpier] on visual studio to mantain code pattern accross the project. The rules can be found at `.editorconfig`


#### Push an existing folder

```sh
cd existing_folder
git init
// HTTPS
git remote add origin https://gitlab.com/likeanormaldude/lottus.git
// SSH
git remote add origin git@gitlab.com:likeanormaldude/mock-web.git
git add .
git commit -m "Initial commit"
git push -u origin master
```

### Command line instructions

#### Install NodeJS dependencies (yarn version)

```sh
yarn install
```




[//]: # (Links)

   [csharpier]: <https://csharpier.com/>