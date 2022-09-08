
# RegisterationForm 
> Clean Architecture + ASP.NET Core 6 Web API + .NET Core 6 Class Library + Angular 13

## Table of Contents
* [General Info](#general-information)
* [Technologies Used](#technologies-used)
* [Features](#features)
* [Who is it suitable for?](#Who-is-it-suitable-for)
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
  * [Implement Person Registration Form (html file)](#Implement-Person-Registeration-Form-html-file)
  * [Implement Person Registration Form (typescript file)](#Implement-Person-Registeration-Form-typescript-file)
* [Final Step](#Final-Step)
<!-- * [License](#license) -->


## General Information
> This is a beginner-level project to teach you:
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

## Who is it suitable for?
for developers who are familiar with Angular, Asp.Net Core, REST Apis, C#, and Database.

## What do you learn in this project?
- How to design a multi-layers project with clean architecture
- Build web api application with ASP.NET Core 6
- Build the web application UI base on Angular 13 
- Connect the UI to the Backend With Angular + TypeScript 
- How to use RadiButton, Checkbox, DropDown, Input, and feed them by api in Angular


## Screenshots
![image](https://user-images.githubusercontent.com/30793006/181934783-d6d901a7-102c-4907-9377-7558753a6c51.png)

## What does this application do?
this is a simple single-page application that contains a table and a form. the form has a multi-type of html inputs and allows you to register the person's data into a database or edit previous data. by the table you can see data or delete them.

## Create Projects
In the first step we Create a blank solution in the visual studio the add the projects as follows:
> a class library with the name *Domain* for our models, dto, ...

> a class library with the name *Infrastructure* for interfaces such as unit of work and other common general operations that are gonna use in other layers

> a class library with the name *DAL* to communicate with the database

> a class library with the name *IoC* to handle our dependency injections in one layer

> a class library with the name *BL* to implement our business rules 

> a web api .net core to implement our apis

![image](https://user-images.githubusercontent.com/30793006/182139351-cc2cc3dc-ec16-4483-bbf5-b91d503200ca.png)


## Develop Domain Layer
This is our data diagram:
![image](https://user-images.githubusercontent.com/30793006/182148220-c85e0e71-6a47-4ad0-937d-7be609547f6b.png)
We have 3 tables:
> *Person* is the table that keeps person data.

> *Personality* is the table that keeps the personalities types. (We don't have any form for crud operations on this table and it gets fed directly from Sql Server Management Studio)

> *PersonPersonality* is the table that keeps the data that each person has on how many types of personalities.

we want to create these tables by entity framework code first and also we need a model per table to use in our project so we create a class per table just like this picture:

![image](https://user-images.githubusercontent.com/30793006/182153818-9278e716-1f06-4bee-a85c-194a7000891a.png)

> in the *Person* model I put two properties with names: **personalities** and **personalitiesIds**. the **personalities** is to combine all person personality in one string and show it in the table and the **personalitiesIds** is used because we need each personality id in the client side application and when we want to edit one person info. as we do not save these properties in the database so I used the *[NotMapped]* for these properties.

> as we don't have any base table for *gender* and *status* in the *Person* table so we create two enums for these fields.

## Connect to Database
Now we want to connect to the database. the *DAL* Layer should be responsible for this one.
before starting we should install the necessary packages. install these packages on DAL Project:
> Microsoft.EntityFrameworkCore

> Microsoft.EntityFrameworkCore.Design

> Microsoft.EntityFrameworkCore.SqlServer

> Microsoft.EntityFrameworkCore.Tools

> as you know the database context gets the connection string from the end layer. in this project, our context gets the database connection string from the *Api* layer. so we need to define the database connection string in the file *appsettings.json* in the *Api* layer. like this:
  `"ConnectionStrings": {
    "RegisterationFormDbContext": "Server=.;Database=RegisterationForm;Trusted_Connection=True;MultipleActiveResultSets=true"
  }`

> now we should connect our context to the database. for this one, we should do it in the layer that injects the services(here, the IoC layer). for this, you need to install the package

> *Microsoft.Extensions.DependencyInjection.Abstractions* on *IoC Project*
 
 then we create a class with the name DbConfig in the *IoC Project* and use the EF core service to connect our database context to the database. like this:

![image](https://user-images.githubusercontent.com/30793006/182185009-37b606e0-adfe-43fe-8a95-f8696e17bc46.png)


> at the next step you should inject this config in the end layer with the path: *Api/Program.cs* like this:

`builder.Services.AddDatabaseConfig(configuration);`

> now we need to create a database and tables by EF. before using migration you should install 

> *Microsoft.EntityFrameworkCore.Design* on the Api layer because it's needed.

> now you can go to *Package Manager Console* and write the first add migration code like this: *Add-Migrations InitializeDb*

> Congratulations your project is connected to the database.

## Implement UnitOfWork
We use the UnitOfWork pattern to work with data such as (get, write, update or delete) so since the UnitOfWork has 2 parts, the first part is *Interface* and the second part is *Implementation* we put the *Interface* in the Infrastructure layer and implement them in the DAL layer.

> In the *Infrastructure* layer create a folder with the name *IUnitOfWork* then create 2 interfaces one with the name IGenericRepository and the other with the name IUnitOfWork

> In IGenericRepository you should define all crud methods that you want to use in the project such as (getById, getAll, Insert, ...) and this class is the generic type of *T* that *T* is the name of our table. it means that the *IGenericRepository* can be a polymorphism of any model and your methods can operate for any table of database.

> In IUnitOfWork you should declare an instance of IGenericRepository per table of the database just like this:

![image](https://user-images.githubusercontent.com/30793006/182314749-6c76c23e-f58c-4553-8e15-053a2819da0a.png)

> Ok now it's time for implementation. go to the *DAL* layer and create a folder with the name *UnitOfWork* then create 2 classes with the name GenericRepository and UnitOfWork then implement the interfaces that you made in the *Infrastructure* layer.

>>don't forget that the IUnitOfWork interface and UnitOfWork class both should inherit from *IDisposable* so when the UnitOfWork is finished with the database it can free unmanaged resources.

> at the last step you should inject the UnitOfWork in the *IoC* layer and then in the *Api* layer. so Create a class with the name UnitOfWorkConfig in the *IoC* layer and inject the UnitOfWork like this:

![image](https://user-images.githubusercontent.com/30793006/182325601-b35054f1-cec3-4f18-818b-d86c83130bed.png)

then inject this class in the *Api* layer like this:

![image](https://user-images.githubusercontent.com/30793006/182325716-d78f1127-c798-4c93-9755-fb489a3a23df.png)


> You are done with *UnitOfWork*.

## Implement BL Layer
Now it's time to implement the *BL* layer according to our business rules and our scenarios for *CRUD* operations.
*Note:* it's not good to return the database model to api, it's better to convert your database model to dto (data transfer object).
it's better to have one or more dtos per scenario.

> the first step is installing *Automapper*. why? to convert database model to dto. since the working with data is *DAL* layer duty so we put it in *DAL* layer then install these packages in *DAL* layer: `AutoMapper` and `AutoMapper.Extensions.Microsoft.DependencyInjection` by the nuget package manager.

> after installing *Automaooer* You need to set some things. 
- first, create a folder with the name *AutomapperProfile* in the *DAL* layer then create a class with the name *MappingProfile*. in this class you must declare what model map to what dto or reverse.
- second you should inject *Automapper* and your *MappingProfile* class in the *IoC* layer as usual. so create a class with the name *AutomapperConfig* in the *IoC* layer and inject *Automapper* like this:

![image](https://user-images.githubusercontent.com/30793006/182592286-e77ffa56-9145-4b7f-bab7-5466e965b401.png)
and finally, inject this method in the *Api* layer as we did in previous steps.

> Now it's time to declare our scenarios and then implement the suitable methods in the *BL* layer. 

- we have a grid in our application that shows every data of the person, so we need a method that returns all person's data and for each person also shows the all personalities for that person. so in the *PersonBusiness* class, create a method with the name *GetAllPersonInfo* then in this method first get all data from the *Person* table then map this list to dto then for each record in this list get the titles of this person personalities from *PersonalityBusiness*.

- we have a form that must show all personalities from the database. so we need a method that gets all personalities from the database. so we have a method with the name GetPersonalitiesTitle in *PersonalityBusiness* that does this.

- the user should register the person's info by the form. so we need a method that gets the data and saves it in the database. so we create a method with the name *InsertPerson* in *PersonBusiness* to register data into the database and we should have the other method to insert this person's personalities in the *PersonPersonality* table so we create another method with the name *InsertRange* in *PersonPeronalityBusiness*  and call it after we inserted person data.

- the user should be able to update person data. so we need 2 methods here one to get the target person's data and put it in the form for edit and the other one for getting the new data and updating the previous one. 

    **first** create a method with name *GetPersonById* in *PersonBusiness* to get person data.
    
    **second** create a method with the name *UpdatePerson* in *PersonBusiness* to update the person data.
    
    **third** we need a method with the name *DeleteRange* to delete all previous personalities for this person in *PersonPersonalityBusiness*.
    
    **fourth** we need a method with the name *InsertRange* to insert newly selected personalities for this person in *PersonPersonalityBusiness*.
    
    **fifth** call the *DeleteRange* and *InsertRange* in method *UpdatePerson*.

- the user should be able to delete the person data. so we want a method to delete the person from the database.

before deleting a person you should delete his/her personalities from the *PersonPersonality* table, so we call the *DeleteRange* method from *PersonPersonalityBusiness* before deleting the person, then to delete the person create a method with the name *DeletePerson* in *PersonBusiness*.

after all these steps don't forget to inject your Businesses class in the *IoC* layer so create a class with the name *BusinessConfig* in the *IoC* layer and inject your Businesses here and finally inject it in the *Api* layer.


## Implement Api Layer
The api layer is a communication bridge to out and it is the layer that serves data for other applications which communicate with your project.
base on our scenarios we need 2 controllers one with the name *PersonController* and the other with the name *PersonalityController*.

we have 5 methods in *PersonController* and 1 method in *PersonalityController* that it's clear what they do.

> we used 4 types of http requests in these 2 controllers:

  - `[HttpGet]`: use it when the method is going to deliver data in the output.
  - `[HttpPost]`: use it when the method is going to get some data from input and save it in the database.
  - `[HttpPut]`: use it when the method is going to get some data and update an already saved data.
  - `[HttpDelete]`: use it when the method is going to delete some data from the database.


## Initialize UI Layer
We are completely done about the backend so now it's our turn to implement the UI, let's go.

I chose bootstrap 5 to design UI and Angular 13 to connect the UI to the Backend. 

as you know *Angular*  is based on *Typescript*. so we use *Typescript* language to develop this angular app and also most of our file types are *Typescript*.

So follow these steps:

- to run the angular app you need to install node.js so go to https://nodejs.org/en/download/ and download then install it.
- after installing node.js you should install angular CLI to be able to compile angular code. you can install it with this command `npm install -g @angular/cli`.
- create a folder with the name *RegisterationForm.UI*.
- run powershell with *administrator* mode and route to the *RegisterationForm.UI* path then execute the command `ng new UI`.
- after executing the command it starts to download the necessary files and asks some questions as follows:
  - would you like to add angular routing? *press y*
  - which stylesheet format would you like to use? *select css*
- I use **Visual Studio Code** to develop my angular app, so open the VS Code go to *File >>> Open Folder* then route to *PersonRegisterationForm.UI/UI* and click the *Select Folder* button, then you can see your angular app in the VS Code.
- go to the path `/src` in VS Code and open the file *index.html* then put the bootstrap stylesheets link in the header:

  ```
  <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/css/bootstrap.min.css" rel="stylesheet">
  ```
  
  ```
  <link rel="preload" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.14.0/css/all.min.css" data-rocket-async="style" as="style" onload="this.onload=null;this.rel='stylesheet'" integrity="sha512-1PKOgIY59xJ8Co8+NE6FZ+LOAZKjy+KY8iq0G4B3CyeY6wYHN3yt9PW0XpSriVlkMXe40PTKnXrLnZ9+fkDaog==" crossorigin="anonymous" />
  ```
  
  and put the js file in the body:
  
  ```
  <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/js/bootstrap.bundle.min.js"></script>
  ```
  
#### now we are done with initializing the UI. the next step is implementing this layer.

## Implement Angular App Infrustructure
when you want to connect an angular app to a server-side application you need an infrastructure to do it for you, so we create some infrastructures to connect our angular app to our api application in this part.

all of my application business is about *Person* stuff so I Create a folder with the name **Person** in my project and put all of the stuff in this folder.
right click on the */src/app* folder and then select **New Folder** and rename it to **Person**.
#### remember that all of your stuff should be in the */src/app* folder.

one of the advantages of angular is modularity. I mean you can divide your application into some separate parts so that each one of them does a certain job, we call each one of these parts a **Component** in angular.

for any command that you want to execute in VS Code, you should execute it in the terminal window, for this go to **Terminal >>> New Terminal** in VS Code.

route to *Person* path: `cd src/app/person`

- we need to have some models to work with data in our angular app so create a new folder with the name **model** in the path: `src/app/person`

- also we need a folder with the name: **service**. we will create some services to connect to our backend and do *CRUD* operations. so create it in this path: `src/app/person`


### Create Models
as you remember in <a href="#Develop-Domain-Layer">Develop Domain Layer Section</a> we had a model for *Person* another model for *Personality* and we had two *Enums* for *Gender* and *Status* so we create these four models in our angular app.

- right click on the *model* folder in the route *src/app/person* and select *New File* then rename it to *Gender.ts*. the *.ts* extension shows that this file type is *Typescript*. and write an *Enum* like the *Enum* you created in the backend.

- create a model for *Status*, *Person*, *Personality* like the previous step. (look at the source code)

### Implement Service Layer
We create services in the angular app to communicate to the server and send *CRUD* requests such as *to add new person*, *read all added person*, ...

in this project, we need two services one for the person and the other one for personality.

ok so execute these commands to create your services:

`ng g s person service and `ng g s personality-service`

> first we implement the person-service
- add HttpClientModule to the angular app, go to the path `src/app` and open the file `app.module.ts` and add HttpClientModule just like this:

  ![image](https://user-images.githubusercontent.com/30793006/188385377-b621769a-cfa1-4d64-98a3-31a16eaf1515.png)

- add HttpClient library to this service: `import { HttpClient } from '@angular/common/http'` this library lets you to send http request to server.
- inject this library in the *person-service* constructor to can use it.
- in this service we work with an object of *Person* or an object of a *List of Person* so we create an object for each one of them. just like this:

`public person: Person = new Person();` and `public allPerson: Person[] = [];` we keep our data in these two objects.

- create an object to declare server url: `private readonly baseUrl: string = 'http://localhost:18359/api/people'` this is the address of my api application.

- create 4 methods to do the *CRUD* operation.

- please note that the *person* object in this service will be initialized in our components such as the *person-info component* or *person-register component*, but the *allPerson* object initializes in this service by the *get()* method. so we send the *person* object to the server which is initialized in the *person-register component* and initialize the *allPerson* in this service and show it in the table by the *person-info component*.(please see the code)

> second we implement the personality-service:
- this is just as like as *person-service* but we have only a *get()* method to initialize a list of *all personalities* to show in the *registration form*.(please see the code)
## Implement UI Layer
Ok, now it's time to implement the UI layer. first, we should know what we want in our application UI. I want to have a single-page application and do all of my work on one page, so I want to have a table on the left hand of my application and a form on the right hand of the page just like what you saw on the <a href="#screenshots">Screenshots section</a>.

### Implement Person Info Table
we need a table to show all of our person registered in the database so we need a part in our angular app that can show a table with all person data and can use the infrastructure layer to connect to the server-side app. for this we create a component with this command and name it *person-info* in the path **src/app/person**:

`ng g c person-info`

now you have a folder with the name *person-info* in your angular app. let's start with the *person-info.component.ts* file in this folder:

- first import the *person, gender, status models*, *PersonServiceService* and *PersonalityServiceService* we are gonna work with them in this part.
- inject the *PersonService* and *PersonalityService* in the constructor to can call them in your app.
- create an object from the *Gender* model and the other from the *Status* model to can map enum to proper text in the *UI*.
![image](https://user-images.githubusercontent.com/30793006/187890564-76640b90-4940-4714-ad97-e7ed66cd66d4.png)

> in this class you see a built-in method with the name *ngOnInit()* method, this method is something like *constructor* but we use the constructor to inject the dependencies and use the *ngOnInit()* for all the initialization/declaration.

- so in the *ngOnInit()* we just call the *to get()* method to get our table feed. (please see the code)

- now go to the path: `src/app/person/person-info` and open the file *person-info.component.html* and write the below code:

![image](https://user-images.githubusercontent.com/30793006/187896167-a1f270f0-8d12-4357-854d-7d51cc601354.png)

> as you see I used *ngFor* in the *<tbody>*, we use *ngFor* to create **loops** and here we have an object of a list of *person* in *person-service* and we want to create a loop on this object so we use *ngFor*. 
 
> as I said in previous steps the person's gender and status are the types of enum and when you get all person from api the server side sends gender and status as an enum to your angular application and you should map this enum to text, for this one I used the objects that I created in *person-info.component.ts* and map the enum to text in the *UI* just like this:
 
![image](https://user-images.githubusercontent.com/30793006/187903315-c8749e49-0361-40b2-b0b7-b432917f7102.png)
 
> in the last row we have two operational buttons to delete and edit the person and we have two methods to do these jobs and assign these methods to these buttons via *(click)* syntax which means whenever the button was clicked fire this method. just like this: `(click)="deletePerson(person.id)"`

**now we should implement the delete person method**

- again go to *person-info.component.ts* declare a method with the name: *deletePerson* and gets an *Id* parameter and send it to the *person-service* delete method. the *Id* parameter is a type of *number* or *string* and this is one of the *Angular* abilities that you can have a parameter or an object with multiple types. you can consider just one type.
 
- we will implement the *showPerson()* method after *person-register* implementation.
 
 ### Implement Person Registration Form (html file)
 in this section, we need a form with multi types of html inputs to add a new person or edit the previous person added. so create a new component with the name: `person-register` with this command: `ng g c person-register`.
 
 in this section, I will explain the html file and in the next section, I will explain about typescript part.
 
 - first, we complete the html file. go to the path: `src/app/person/person-register` and open the `person-registor.component.html` file and complete it like the below code:
 ![image](https://user-images.githubusercontent.com/30793006/188192828-5fe86916-dcd4-4441-8f6a-d65ff3dd58bb.png)
**now I'm gonna explain what I did in this file:**
 
- the form: `<form novalidate autocomplete="off" #form="ngForm" (submit)="submitPerson(form)"> `
  > we have a form and because I want to access the form in all of the other parts of the form so I declare a `#form` as the angular form for access in the html file just like this: `#form="ngForm"`

  > I want to send all form inputs data to the server so I used the angular `(submit)` event and defined a *submitPerson()* method to send form data to the *person-registor.component.ts*.
 
- `<input type="hidden" name="id" [value]="service.person.id">`
  > as where the *person id* is auto-generated so we do not need to generate it but we need to send the *id* when we want to use this form to edit a person so we need to this input to keep *id* and send it to backend and because we fill this input by the own application and the *User* doesn't fill it so we make it hidden.
 
  > to fill this input I used `[value]` angular property binding  to bind the person id initialized in the *person-service*

- `<input type="text" class="form-control" id="name" placeholder="Enter Name" name="name" #name="ngModel" [(ngModel)]="service.person.name" required minlength="3" maxlength="10" [class.invalid]="name.invalid &#038;&#038; name.touched">`

  > I used ` #name="ngModel"` to declare an *ngModel* to access this input in all other parts.
 
  > the `[(ngModel)]="service.person.name"` is used to bind the value of this input to the person that is already generated in *person-service*, with this method, you can access the value inserted by user in the *person-register.component.ts* and reverse.
 
  > the `[class.invalid]` is an Angular validator that indicates the considered input changes to *red* color whenever it was invalid, you should declare the invalidity situation in this validator and I declared it as `name.invalid &#038;&#038; name.touched` and it means whenever the input with '#name' ngModel was empty or the min length was less than 3 or more than 10 characters make this input red.

- for gender I used a dropdown list to show you how can you use the dropdown in angular:
 
  `<select [(ngModel)]="service.person.gender" name="gender" id="gender" #gender="ngModel"  class="form-select" required [class.invalid]="gender.invalid &#038;&#038; gender.touched"><option *ngFor="let key of genders" [ngValue]="key">{{enums[key]}}</option></select>`
 
  > in *option tag* we should send the gender id to the backend but show the gender title to the user so we create an *object* of array of numbers and initialize it with gender ids and create an *object* of *gender* model in `person-registor.component.ts` to map the enum to text in the *UI*. just like this:
 
   ![image](https://user-images.githubusercontent.com/30793006/188318962-6b6381f0-21eb-4bcb-a23a-4445a2689ce3.png)

  > we will explain `person-registor.component.ts` in the next section.
 
  > so we create a loop with `ngFor` on the genders object to generate dropdown list items and use `{{enums[key]}}` to map enum to text and show it to the user.
 
- for status, we use the radio buttons and it's almost like the dropdown list you can understand it by reading the code.
 
- to show the personalities and the user can select each personality we use the *checkboxes*:
 
  `                <div class="form-check form-check-inline" *ngFor="let personality of personalitySevice.allPersonality">
                    <input class="form-check-input" type="checkbox"  [value]=""  required  (change)="onChange(personality.id,$any($event.target)?.checked)" id="chb{{personality.id}}">
                    <label class="form-check-label" for="inlineRadio1">{{personality.title}}</label>
                </div>`
   > we generate the checkboxes by the given data in *personality-service* from the server. the code: `personalitySevice.allPersonality` 
 
   > we use `(change)="onChange(personality.id,$any($event.target)?.checked)"` to send the personality id and the checkbox status to `person-registor.component.ts` and define a method with the name: `onChange` in the `person-registor.component.ts` file.
 
   > we should have a mechanism to can clear all checkboxes so we need to give a unique id to each checkbox and because of that I used this pattern `id="chb{{personality.id}}"` so all of my checkboxes ids start with **chb** for example: `chb1`, `chb2`, `chb3`, ...
 
- the submit button: `<button type="submit" class="btn btn-primary btn-block" [disabled]="form.invalid">Submit</button>`
   > we want to prevent submitting the form if our inputs were invalid. so I used `[disabled]="form.invalid"`>>> *form* point to ngModel of form which we initialized in the first line of `person-registor.component.html`.
   > when the user clicks on this button the *submitPerson(form)* will be hit.
- in all components you can find a *.css* file this file is for styling to the component html file. in this component we want to make my form left aligned so open the file `person-register.component.cs` file and write blow code:
  ```
 .text-left{
    text-align: left !important;
}
  ```
 ### Implement Person Registration Form (typescript file)
 now we want to complete the `person-registor.component.ts`, so open this file.
 
- first of all, we import the necessary references:
 
  `import { PersonServiceService } from '../service/person-service.service';`
 
  `import { NgForm } from '@angular/forms';`
 
  `import { Gender } from '../model/Gender';`
 
  `import { Status } from '../model/Status';`
 
  `import { PersonalityServiceService } from '../service/personality-service.service';`
 
  because we want to use ngForm so we should add the FormsModule to the project, go to the path `src/app` and open the file `app.module.ts` then add the module just like this:
 
   ![image](https://user-images.githubusercontent.com/30793006/188390422-d7f5bd35-7df7-49e0-9407-fb96c2d9c37d.png)
 
  we import *NgForm* because we pass the form html to the typescript file by *Angular* and then we can clear the form inputs:
 
  `  resetForm(form: NgForm) {
     form.form.reset();
   }`
- initializing *Gender* dropdown list :
 we need to define two objects:
 
  `genders: number[] = [];`
 
  `enums = Gender;`
 
  the **genders** object is needed to keep all gender ids because we need the gender id to send the selected genders to the server-side application.
  the **enums** object is used to fill **genders** object just like this: <br/>
 `this.genders = Object.keys(this.enums).filter(x => parseInt(x) >= 0).map(Number);`
  and of course, we use the **enums** object to map the gender enum to text in html as you saw in the previous section.
 
- the constructor: `constructor(public service: PersonServiceService,public personalitySevice:PersonalityServiceService)`
 
  inject the *PersonServiceService* and *PersonalityServiceService* to can use them.
 
- the *ngOnInit()* method: use it to initialize the objects and initializing *personalities* by calling the *get* method from *personality-service*:   `this.personalitySevice.get();`
 
- the *onChange()* method for checkboxes:
 
  ![image](https://user-images.githubusercontent.com/30793006/188320403-fbe0e96a-b853-4d3a-a54b-5678e434515e.png)
 
  this method gets a *personality id* and *isChecked* as entry parameters. then we check if that personality item is checked we push it to the person personalities that are initialized in the *person-service*. and if the *isChecked* was false it means the user unselected that personality so we should remove it from the person personalities for this first we find the index of that personality in person personalities and then remove it by the index number.
 
- the *submitPerson()* and *resetForm()* methods:
 
  ![image](https://user-images.githubusercontent.com/30793006/188321049-2eabd596-870f-4ac0-a032-a3417628bf2c.png)
 
  we know that we should use this form as *Registre Form* and as *Update Form* so we should recognize when we should *Register* and when should *Update*. we will recognize it by the person id, if the person id had value it means the user before clicking on the *Submit* button he/she clicked on the edit button and because of that the app filled the id field and if the id didn't have value it means the user is registering new person.
 
  finally, when the form is submitted we reset the form to clear all inputs.
 
## Final Step
now you are done with *person-register* and *person-info* components and now you need to add these components to the application first page for this one you should go to path `src/app/app.component.html` and write blow code:
```
 <div class="container-fluid p-5 bg-primary text-white text-center">
  <h1>Person Registeration</h1>
  <p>You Can Register Person Info in This App!</p>
</div>

<div class="container mt-5 text-center">
  <div class="row">
    <div class="col-sm-6">
      <app-person-info></app-person-info>
    </div>
    <div class="col-sm-6">
      <app-person-registor></app-person-registor>
    </div>
  </div>
</div>
 ```
 
 > the `<app-person-info></app-person-info>` points to the `person-info` component and if you look at the `person-info.component.ts` you see that **app-person-info** is declared in this file like this:
 
 ![image](https://user-images.githubusercontent.com/30793006/189117584-42e16d17-3f94-4ccb-a6a5-aa875a9be2a4.png)

 
