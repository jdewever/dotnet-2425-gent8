# Rise - [GENT8]

## Team Members
- [Jens Meersschaert] - [jens.meersschaert@student.hogent.be] - [JensM04]
- [Neal Debot] - [neal.debot@student.hogent.be] - [NealDebot]
- [Mathisse Snauwaert] - [mathisse.snauwaert@student.hogent.be] - [mathisse2004]
- [Niek Termote] - [niek.termote@student.hogent.be] - [NiekTermote]
- [Brecht Vanderhoydonck] - [brecht.vanderhoydonck@student.hogent.be] - [BrechtVanderhoydonck]
- [Eliott Hauteclair] - [eliott.hauteclair@student.hogent.be] - [eliottha]
- [Jonas De Wever] - [jonas.dewever@student.hogent.be] - [jdewever]


## Technologies & Packages Used
- [Blazor](https://dotnet.microsoft.com/en-us/apps/aspnet/web-apps/blazor) - Frontend
- [ASP.NET 8](https://dotnet.microsoft.com/en-us/apps/aspnet) - Backend
- [Entity Framework 8](https://learn.microsoft.com/en-us/ef/) - Database Access
- [EntityFrameworkCore Triggered](https://github.com/koenbeuk/EntityFrameworkCore.Triggered) - Database Triggers
- [User Secrets](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets) - Securely store secrets in DEV.
- [GuardClauses](https://github.com/ardalis/GuardClauses) - Validation Helper
- [bUnit](https://bunit.dev) - Blazor Component Testing
- [xUnit](https://xunit.net) - (Unit) Testing
- [nSubstitute](https://nsubstitute.github.io) - Mocking for testing
- [Shouldly](https://docs.shouldly.org) - Helper for testing

## Installation Instructions
1. Clone the repository
2. Open the `Rise.sln` file in Visual Studio or Visual Studio Code
3. Run the project using the `Rise.Server` project as the startup project
4. The project should open in your default browser on port 5001.
5. Initially the database will not exist, so you will need to run the migrations to create the database.

## Creation of the database
To create the database, run the following command in the main folder `Rise`
```
dotnet ef database update --startup-project Rise.Server --project Rise.Persistence
```
> Make sure your connection string is correct in the `Rise/Server/appsettings.json` file.

## Migrations
Adapting the database schema can be done using migrations. To create a new migration, run the following command:
```
dotnet ef migrations add [MIGRATION_NAME] --startup-project Rise.Server --project Rise.Persistence
```
And then update the database using the following command:
```
dotnet ef database update --startup-project Rise.Server --project Rise.Persistence
```

## Tailwind CSS

> **Use `dotnet watch --no-hot-reload` to directly update CSS while coding, no need ot run `npm run watch` in a separate terminal**
> 
> This is a little slower because it disabled hot reload, but it's a lot faster and easier to get updated styling. On first use, you need to run `npm install` in the Rise.Client folder.

We added Tailwind as styling framework for our Blazor client. This is done with postcss and tailwind. The compiled tailwind css is included in the Rise.Client project at `wwwroot/css/app.min.css`.

If you use new tailwind classes, you need to recompile the css or in development, watch the Pages folder for changes and recompile the css.
In the Rise.Client folder, run the following command:
```
npm install
npm run watch
```

To compile the css for production, run the following command:
```
npm install
npm run build
```