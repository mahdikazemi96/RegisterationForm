import { Component, OnInit } from '@angular/core';
import { PersonServiceService } from '../Service/person-service.service';
import { NgForm } from '@angular/forms';
import { Gender } from '../Model/gender';
import { Status } from '../Model/status';
import { PersonalityServiceService } from '../Service/personality-service.service';


@Component({
  selector: 'app-person-registor',
  templateUrl: './person-register.component.html',
  styleUrls: ['./person-register.component.css']
})
export class PersonRegistorComponent implements OnInit {

  genders: number[] = [];
  genderEnums = Gender;

  statuses: number[] = [];
  statsEnums = Status;


  constructor(public service: PersonServiceService,public personalitySevice:PersonalityServiceService) {

  }

  ngOnInit(): void {
    this.personalitySevice.get();
    this.genders = Object.keys(this.genderEnums).filter(x => parseInt(x) >= 0).map(Number);
    this.statuses = Object.keys(this.statsEnums).filter(x => parseInt(x) >= 0).map(Number);
  }

  //CHB
  onChange(id: number, isChecked: boolean) {

    if (isChecked) {
      this.service.person.personalitiesIds.push(id);
    } else {
      const index = this.service.person.personalitiesIds.indexOf(id, 0);
      console.log(index);
      if (index > -1)
      {
        this.service.person.personalitiesIds.splice(index, 1);
        console.log(this.service.person.personalitiesIds);
      }
    }
  }

  submitPerson(form: NgForm) {
    if (this.service.person.id == 0 || this.service.person.id == null)
      this.service.post();
    else
      this.service.put();

    this.resetForm(form);
  }

  resetForm(form: NgForm) {
    form.form.reset();
  }
}
