import { Component, ElementRef, OnInit } from '@angular/core';
import { Person } from '../Model/person';
import { PersonServiceService } from '../Service/person-service.service';
import { Gender } from '../Model/gender';
import { Status } from '../Model/status';
import { PersonalityServiceService } from '../Service/personality-service.service';

@Component({
  selector: 'app-person-info',
  templateUrl: './person-info.component.html',
  styleUrls: ['./person-info.component.css']
})
export class PersonInfoComponent implements OnInit {

  gender = Gender;
  status = Status;

  constructor(public service: PersonServiceService,public personalityService:PersonalityServiceService) { }

  ngOnInit(): void {
    this.service.get().then(res => console.log(res));
    console.log(this.service.allPerson);
  }

  deletePerson(Id: number | string) {
    if (confirm("Are You Sure"))
      this.service.delete(Id);
  }

  showPerson(person: Person) {

    this.service.person = Object.assign({}, person);

    //CHB
    //برای پاک کردن همه چک باکس ها
    this.personalityService.allPersonality.forEach(element => {
      const checkbox = document.getElementById(
        'chb'+element.id,
      ) as HTMLInputElement | null;
      
      if (checkbox != null) {
        checkbox.checked = false;
      }
    });
    
    //CHB
    //برای انتخاب چک باکس های انتخاب شده هنگام 
    // Insert
    this.service.person.personalitiesIds.forEach(element => {
      const checkbox = document.getElementById(
        'chb'+element,
      ) as HTMLInputElement | null;
      console.log(checkbox);
      if (checkbox != null) {
        checkbox.checked = true;
        //this.service.person.personalitiesIds.push(element);
      }
    });
  }


}
