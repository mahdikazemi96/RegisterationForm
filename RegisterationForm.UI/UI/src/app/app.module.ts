import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { AppComponent } from './app.component';
import { PersonRegistorComponent } from './Person/person-register/person-register.component';
import { PersonInfoComponent } from './Person/person-info/person-info.component';

@NgModule({
  declarations: [
    AppComponent,
    PersonRegistorComponent,
    PersonInfoComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
