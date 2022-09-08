import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Personality } from '../Model/personality';

@Injectable({
  providedIn: 'root'
})
export class PersonalityServiceService {

  public personality: Personality = new Personality();
  public allPersonality: Personality[] = [];
  private baseUrl:string='http://localhost:5003/api/personality';

  constructor(private http:HttpClient) { }

  get(){
    return this.http.get(this.baseUrl).toPromise().then(res=>this.allPersonality=res as Personality[]);
  }
}
