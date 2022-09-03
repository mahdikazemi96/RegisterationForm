
# RegisterationForm 
> Clean Architecture + ASP.NET Core 6 Web API + .NET Core 6 Class Library + Angular 13

## Table of Contents
* [General Info](#general-information)
* [Technologies Used](#technologies-used)
* [Features](#features)
* [What do you learn in this project?](#what-do-you-learn-in-this-project)
* [Screenshots](#screenshots)
* [What does this application do?](#What-does-this-application-do)
* [Create Projects](#create-projects)
* [Develop Domain Layer](#Develop-Domain-Layer)
* [Connect To Database](#Connect-To-Database)
* [Implement UnitOfWork](#Implement-UnitOfWork)
* [Implement BL Layer](#Implement-BL-Layer)
* [Implement Api Layer](#Implement-Api-Layer)
* [Initialize UI Layer](#Initialize-UI-Layer)
* [Implement Angular App Infrustructure](#Implement-Angular-App-Infrustructure)
  * [Create Models](#Create-Models)
  * [Implement Service Layer](#Implement-Service-Layer)
* [Implement UI Layer](#Implement-UI-Layer)
  * [Implement Person Info Table](#Implement-Person-Info-Table)
  * [Implement Person Registeration Form](#Implement-Person-Registeration-Form)
<!-- * [License](#license) -->


## General Information
> This is a beginner level project to teach you:
- Clean Architecture
- ASP.NET Core Web Api
- .NET Core Library
- Angular
- Automapper


## Technologies Used
- .NET Core - version 6
- Angular - version 13
- EF Core - version 6



## Features
- CRUD Operation


## What do you learn in this project?
- How to design a multi layers project with clean architecture
- Build web api application with ASP.NET Core 6
- Build the web application UI base on Angular 13 
- Connect the UI to Backend With Angular + TypeScript 
- How to use RadiButton, Checkbox, DropDown, Input and feed them by api in Angular


## Screenshots
![image](https://user-images.githubusercontent.com/30793006/181934783-d6d901a7-102c-4907-9377-7558753a6c51.png)

## What does this application do?
this is a simple singel page application that contains a table and a form. the form has multi type of html inputs and allow you register the person data into database or edit previous data. by the table you can see data or delete them.

## Create Projects
At the first step we Create a blank solution in visual studio the add the projects as follow:
> a class library with the name *Domain* for our models, dto, ...

> a class library with the name *Infrastructure* for interfaces such as unitofwork and other common general operations that are gonna use in other layers

> a class library with the name *DAL* to communicate with database

> a class library with the name *IoC* to handle our dependency injections in one layer

> a class library with the name *BL* to implement our business roles 

> a web api .net core to implement our apis

![image](https://user-images.githubusercontent.com/30793006/182139351-cc2cc3dc-ec16-4483-bbf5-b91d503200ca.png)


## Develop Domain Layer
This is our data diagram:
![image](https://user-images.githubusercontent.com/30793006/182148220-c85e0e71-6a47-4ad0-937d-7be609547f6b.png)
We have 3 tables:
> *Person* is the table that keeps person data.

> *Personality* is the table that keeps the personalities types.(We don't have any form for crud operations on this table and it get feed direct from Sql Server Management Studio)

> *PersonPersonality* is the table that keep the data that each person has how many type of personalities.

we want to create these tables by entity framework code first and also we need a model per table to use in our project so we create a class per table just like this picture:

![image](https://user-images.githubusercontent.com/30793006/182153818-9278e716-1f06-4bee-a85c-194a7000891a.png)

> in the *Person* model I put two properties with names: **personalities** and **personalitiesIds**. the **personalities** is to combine all person personality in one string and show it in table and the **personalitiesIds** is use to when we want edit one person info. as we do not save these properties in database so I used the *[NotMapped]* for these propeties.

> as we don't have any base table for *gender* and *status* in *Person* table so we create two enums for these fileds.

## Connect to Database
Now we want to connect to database. the *DAL* Layer should be responsible for this one.
before start we should install necessory packages. install these packages on DAL Project:
> Microsoft.EntityFrameworkCore

> Microsoft.EntityFrameworkCore.Design

> Microsoft.EntityFrameworkCore.SqlServer

> Microsoft.EntityFrameworkCore.Tools

> as you know the database context get the connectin string from the end layer. in this project our context get the database connection string from *Api* layer. so we need to define database connection string in the file *appsettings.json* in *Api* layer. like this:
  `"ConnectionStrings": {
    "RegisterationFormDbContext": "Server=.;Database=RegisterationForm;Trusted_Connection=True;MultipleActiveResultSets=true"
  }`

> now we should connect our context to databse. for this one we should do it in the layer that injects the services(here, IoC layer). for this you need install the package *Microsoft.Extensions.DependencyInjection.Abstractions* then we create a class with name DbConfig and use the ef core service to connect our database context to database. like this:

![image](https://user-images.githubusercontent.com/30793006/182185009-37b606e0-adfe-43fe-8a95-f8696e17bc46.png)


> at the next step you should inject this config in the end layer with the path: *Api/Proram.cs* like this:

`builder.Services.AddDatabaseConfig(configuration);`

> now we need create database and tables by ef. before use migration you should install *Microsoft.EntityFrameworkCore.Design* on Api layer because it's needed.

> now you can go to *Package Manager Console* and write the first add migration code like this: *Add-Migrations InitializeDb*

> Congratulations your project is connected to database.

## Implement UnitOfWork
We use UnitOfWork pattern to work with data such as (get, write, update or delete) so since the UnitOfWork has 2 parts, the first part is *Interface* and the second part is *Implementation* we put the *Interface* in Infrastructure layer and implement them in the DAL layer.

> In the layer *Infrastructure* create a folder with name *IUnitOfWork* then create 2 interfaces one with name IGenericRepository and the other with name IUnitOfWork

> In IGenericRepository you should define all crud methods that you want use in project such as (getById, getAll, Insert, ...) and this class is generic type of *T* that *T* is the name of our table. it means that the *IGenericRepository* can be polymorphism of any model and your methods can operate for any table of database.

> In IUnitOfWork you should declare instance of IGenericRepository per table of database just like this:

![image](https://user-images.githubusercontent.com/30793006/182314749-6c76c23e-f58c-4553-8e15-053a2819da0a.png)

> Ok now it's time to implementation. go to *DAL* layer and create a folder with name *UnitOfWork* the create 2 classes with name GenericRepository and UnitOfWork the implement your interfaces that you made in *Infrastructure* layer.

>>don't forget that IUnitOfWork interface and UnitOfWork class both should inherit from *IDisposable* so when the UnitOfWork finished  with database it can free unmanaged resources.

> at the last ste you should inject the UnitOfWork in *IoC* layer and then in *Api* layer. so Create a class with name UnitOfWorkConfig in *IoC* layer and inject the unitofwork like this:

![image](https://user-images.githubusercontent.com/30793006/182325601-b35054f1-cec3-4f18-818b-d86c83130bed.png)

then inject this class in *Api* layer like this:

![image](https://user-images.githubusercontent.com/30793006/182325716-d78f1127-c798-4c93-9755-fb489a3a23df.png)


> You are done with *UnitOfWork*.

## Implement BL Layer
Now it's time to implement *BL* layer according to our business roles and our senarios for *CRUD* operations.
*Note:* it's not good to return database model to api, it's better to convert your database model to dto (data transfer object).
it's better to have one or more dtos per senario.

> the first step is installing *Automapper*. why? to convert database model to dto. since the working with data is *DAL* layer duty so we put it in *DAL* layer then install these packages in *DAL* layer: `AutoMapper` and `AutoMapper.Extensions.Microsoft.DependencyInjection` by the nuget package manager.

> after installing *Automaooer* You need to set somthings. 
- first create a folder with name *AutomapperProfile* in *DAL* layer then create a class with name *MappingProfile*. in this class you must declare what model map to what dto or reverse.
- second you should inject *Automapper* and your *MappingProfile* class in *IoC* layer as usual. so create a class with name *AutomapperConfig* in *IoC* layer and inject *Automapper* like this:

![image](https://user-images.githubusercontent.com/30793006/182592286-e77ffa56-9145-4b7f-bab7-5466e965b401.png)
and finally inject this mthod in *Api* layer as we did in previous steps.

> Now it's time to declare our senarios and then implement the suitable methods in *BL* layer. 

- we have a grid in our application that shows every data of person, so we need a mehod that return all persons data and for each person also show the all personalities for that person. so in the *PersonBusiness* class create a method with name *GetAllPersonInfo* then in this method first get all data from *Person* table then map this list to dto then for each record in this list get the titles of this person personalities from *PersonalityBusiness*.

- we have a form that must show all personalities from database. so we need a method that get all personalities from database. so we have a methd with name GetPersonalitiesTitle in *PersonalityBusiness* that do this.

- the user should register person info by the form. so we need a method that gets the data and save it in database. so we create a method with name *InsertPerson* in *PersonBusiness* to register data into database and we should have an other method to insert this person personalities in *PersonPersonality* table so we create an other method with name *InsertRange* in *PersonPeronalityBusiness*  and call it after we inserted person data.

- the user should be able to update person data. so we need 2 methods here one for get the target person data and put it in the form for edit and the other one for get the new data and update previous one. 

    **first** create a method with name *GetPersonById* in *PersonBusiness* to get person data.
    
    **second** create a method with name *UpdatePerson* in *PersonBusiness* to update the person data.
    
    **third** we need a method with name *DeleteRange* to delete all previous personalities for this person in *PersonPersonalityBusiness*.
    
    **fourth** we need a method with name *InsertRange* to insert new selected personalities for this person in *PersonPersonalityBusiness*.
    
    **fifth** call the *DeleteRange* and *InsertRange* in method *UpdatePerson*.

- the user should be able to delete the person data. so we want a method to delete the person from database.

before deleting person you should delete his/her personalities from *PersonPersonality* table, so we call *DeleteRange* method from *PersonPersonalityBusiness* before deleting person, then to delete the person create a method with name *DeletePerson* in *PersonBusiness*.

after all these steps don't forget inject your Businesses class in *IoC* layer so create a class with name *BusinessConfig* in *IoC* layer and inject your Businesses here and finally inject it in *Api* layer.


## Implement Api Layer
The api layer is communication bridge to out and tis the layer that serve data for other applications that communicate with your project.
base on our senarios we need 2 controllers one with name *PersonController* and the other with name *PersonalityController*.

we have 5 methods in *PersonController* and 1 method in *PersonalityController* that it's clear what they do.

> we used 4 type of http request in these 2 controllers:

  - `[HttpGet]` : use it when the method is going to deliver data in the output.
  - `[HttpPost]` : use it when the method is going to get some data from input and save it in database.
  - `[HttpPut]` : use it when the method is going to get some data and update an already saved data.
  - `[HttpDelete]` : use it when the method is going to delete some data from database.


## Initialize UI Layer
We are completely done about the backend so now it's turn to implement the UI, let's go.

I choosed bootstrap 5 for design UI and Angular 13 to connect the UI to Backend. 

as you know *Angular*  is based on *Typescript*. so we use *Typescript* language to develop this angular app and also most of or files type are *Typescript*.

So follow these steps:

- to run angular app you need to install nodejs so go to https://nodejs.org/en/download/ and download then install it.
- after installing nodejs you should install angular cli to be able to complie angular code. you can install it with this command `npm install -g @angular/cli`.
- create a folder with name *RegisterationForm.UI*.
- run powershell with *administrator* mode and route to the *RegisterationForm.UI* path then execute the command `ng new UI`.
- after executing the command it starts to download necessory files and asks some questions as follow:
  - would you like to add angular routing? *press y*
  - which stylesheet format would you like to use? *select css*
- I use **Visual Studio Code** to develop my angular app, so open the VS Code go to *File >>> Open Folder* then route to *PersonRegisterationForm.UI/UI* and click the *Select Folder* button, then you can see your angular app in the VS Code.
- go to the path `/src` in VS Code and open the file *index.html* then put the bootstrap stylesheets link in the header:

  `<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/css/bootstrap.min.css" rel="stylesheet">`
  
  `<link rel="preload" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.14.0/css/all.min.css" data-rocket-async="style" as="style" onload="this.onload=null;this.rel='stylesheet'" integrity="sha512-1PKOgIY59xJ8Co8+NE6FZ+LOAZKjy+KY8iq0G4B3CyeY6wYHN3yt9PW0XpSriVlkMXe40PTKnXrLnZ9+fkDaog==" crossorigin="anonymous" />`
  
  and put the js file in the body:
  
  `<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/js/bootstrap.bundle.min.js"></script>`
  
#### now we are done with initializing the UI. the next step is implementing this layer.

## Implement Angular App Infrustructure
when you want to connect an angular app to a server side application you need an infrustructure to do it for you, so we create some infrustructures to connect our angular app to our api application in this part.

all of my application business is arroud of *Person* stuffs so I Create a folder with name **Person** in my project and put all of stuffs in this folder.
right click on the */src/app* folder and then select **New Folder** and rename to **Person**.
#### remember that all of your stuffs should be in */src/app* folder.

one of the advantages of angular is modularity. I mean you can divide your application to some seprate parts that each one of them do their certain job, we call each one of these parts a **Component** in angular.

for any command that you want execute in VS Code you should execute it in terminal window, for this go to **Terminal >>> New Terminal** in VS Code.

route to *Person* path: `cd src/app/person`

- we need to have some models to work with data in our angular app so create a new folder with name **model** in the path: `src/app/person`

- also we need  folder with the name: **service**. we will create some services to connect to our backend and do *CRUD* operations. so create it in this path: `src/app/person`

- create a component for the table with this command: `ng g c person-info` in this component we will implement our table

- create a component for the form with this command: `ng g c person-register` in this component we will implement our form to register and update the registerd person

### Create Models
as you remember in <a href="#Develop-Domain-Layer">Develop Domain Layer Section</a> we had a model for *Person* an other model for *Perssonality* and we had two *Enums* for *Gender* and *Status* so we create these four models in our angular app.

- right click on the *model* folder in the route *src/app/person* and select *New File* then rename it to *Gender.ts*. the *.ts* extension show that this file type is *Typescript*. and write an *Enum* like the *Enum* you created in the backend.

- create a model for *Status*, *Person*, *Personality* like previous step. (look at the source code)

### Implement Service Layer
We create services in angular app to comunicate to server and send *CRUD* request such as *add new person* , *read all added person* , ...

in this project we need two services one for person and the other one for personality.

ok so execute these command to create your services:

`ng g s person-service` and `ng g s personality-service`

> first we implement the person-service
- add HttpClient library to this service: `import { HttpClient } from '@angular/common/http'` this library lets you to send http request to server.
- inject this library in the *person-service* constructor to can use it.
- in this service we work with an object of *Person* or an object of a *List of Person* so we create an object for each one of them. just like this:

`public person: Person = new Person();` and `public allPerson: Person[] = [];` we keep our data in these two objects.

- create an object to declare server url: `private readonly baseUrl: string = 'http://localhost:18359/api/people'` this is the address of my api application.

- create 4 method to do the *CRUD* operation.

- please note that the *person* object in this service will be initialize in our components such as *person-info component* or *person-registor component*, but the *allPerson* object initializes in this service by the *get()* method. so we send the *person* object to the server wich initialized in the *person-registor component* and initialize the *allPerson* in this service and show it in the table by *person-info component*.(please see the code)

> second we implement the personality-service:
- this is just as like as *person-service* but we have only a *get()* method to initialize a list of *all personalities* to show in the *registeration form*.(please see the code)
## Implement UI Layer
Ok now it's time to implement the UI layer. first we should know what do we want in our application UI. I want to have a single page application and do all of my work in one page, so I want to have a table on the left hand of my application and a form on the right hand of the page just like what you saw in the <a href="#screenshots">Screenshots section</a>.

### Implement Person Info Table
we need a table to show all of our person registerd in database so we need a part in our angular app that can show a table with all person data an can use the infrustructure layer to connect to server side app. for this we create a component with this command and name it *person-info* in the path **src/app/person**:

`ng g c person-info`

now you have a folder with name *person-info* in your angular app. let's start with *person-info.component.ts* file in this folder:

- first import the *person, gender, status models* , *PersonServiceService* and *PersonalityServiceService* we are gonna work with them in this part.
- inject the *PersonService* and *PersonalityService* in the constructor to can call them in your app.
- create an object from *Gender* model and the other from *Status* model to can map enum to proper text in the *UI*.
![image](https://user-images.githubusercontent.com/30793006/187890564-76640b90-4940-4714-ad97-e7ed66cd66d4.png)

> in this class you see a built in method with name *ngOnInit()* method, this method is some thing like *constructor* but we use the constructor to inject the dependencies and use the *ngOnInit()* for all the initialization/declaration.

- so in the *ngOnInit()* we just call the *get()* method to get our table feed.(please see the code)

- now go to path: `src/app/person/person-info` and open the file *person-info.component.html* and write blow code:

![image](https://user-images.githubusercontent.com/30793006/187896167-a1f270f0-8d12-4357-854d-7d51cc601354.png)

> as you see I used *ngFor* in the *<tbody>*, we use *ngFor* to create **loops** and here we have an object of a list of *person* in *person-service* and we want to create a loop on this object so we use *ngFor*. 
 
> as I said in previous steps the person gender and status are type of enum and when you get all person from api the server side send gender and status as enum to your angular application and you should map this enum to text, for this one I used the objects that I created in *person-info.component.ts* and map the enum to text in the *UI* just like this:
 
![image](https://user-images.githubusercontent.com/30793006/187903315-c8749e49-0361-40b2-b0b7-b432917f7102.png)
 
> in the last row we have two operational button to delete and edit person and we have two methods to do these jobs and assign these methods to these buttons via *(click)* syntax it means when ever the button was clicked fire this method. just like this: `(click)="deletePerson(person.id)"`

**now we should implement the delete person method**

- again go to *person-info.component.ts* declare a method with name: *deletePerson* and gets an *Id* parameter and send it to *person-service* delete method. the *Id* parameter is type of *number* or *string* and this is one of *Angular* ability that you can have a parameter or an object with multiple types. you can consider just one type.
 
- we will implement the *showPerson()* method after *person-register* implementation.
 
 ### Implement Person Registeration Form
 in this section we need a form with multi types of html inputs to add new person or edit previous person added. so create a new component with name: `person-register` with this command: `ng g c person-register`.
 
 - first we complete the html file. go to path: `src/app/person/person-register` and open `person-registor.component.html` file and complete it like blow code:
 ![image](https://user-images.githubusercontent.com/30793006/188192828-5fe86916-dcd4-4441-8f6a-d65ff3dd58bb.png)
**now I'm gonna explain what I did in this file:**
 
- the form: `<form novalidate autocomplete="off" #form="ngForm" (submit)="submitPerson(form)"> `
  > we have a form and because I want to access the form in all of other parts of the form so I declare a `#form` as angular model for access in the html file just like this: `#form="ngForm"`

  > I want to send all form inputs data to the server so I used angular `(submit)` event and defined a *submitPerson()* method to send form data to the *person-registor.component.ts*.
 
- `<input type="hidden" name="id" [value]="service.person.id">`
  > as where as the *person id* is auto generated so we do not need to generate it but we need send the *id* when we want use this form to edit person so we need to this input to keep *id* and send it to backend and because we fill this input by the own application and the *User* doesn't fill it so we make it hidden.
 
  > to fill this input I used `[value]` angular property binding  to bind the person id initialized in the *person-service*

- `<input type="text" class="form-control" id="name" placeholder="Enter Name" name="name" #name="ngModel" [(ngModel)]="service.person.name" required minlength="3" maxlength="10" [class.invalid]="name.invalid &#038;&#038; name.touched">`

  > I used ` #name="ngModel"` to declare an *ngModel* to access this input in all other parts.
 
  > the `[(ngModel)]="service.person.name"` is used to bind the value of this input to the person that is already generated in *person-service*, with this method you can access the value inserted by user in the *person-register.component.ts* and reverse.
 
  > the `[class.invalid]` is an Angular validtor that indicates the considered input changes to *red* coller when ever it was invalid, you should declare the invalidity situation in this validator and I declaed as `name.invalid &#038;&#038; name.touched` and it means when ever the input with '#name' ngModel was empty or the min length was less than 3 or more than 10 character make this input red.

- for gender I used dropdown list to show you how can you use dropdown in angular:
 
  `<select [(ngModel)]="service.person.gender" name="gender" id="gender" #gender="ngModel"  class="form-select" required [class.invalid]="gender.invalid &#038;&#038; gender.touched"><option *ngFor="let key of genders" [ngValue]="key">{{enums[key]}}</option></select>`
 
  > in *option tag* we should send the gender id to backend but show the gender title to user so we create an *object* of array of numbers and initialize it with gender ids and create an *object* of *gender* model in `person-registor.component.ts` to map the enum to text in the *UI*. just like this:
 
   ![image](https://user-images.githubusercontent.com/30793006/188270476-a33d9dae-b67b-475a-a258-86b62d1144d6.png)

  > we will explain about `person-registor.component.ts` in the next section.
 
  > so we create a loop with `ngFor` on genders object to generate dropdown list items and use `{{enums[key]}}` to map enum to text and show it to user.
 
