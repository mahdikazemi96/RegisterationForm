import { TestBed } from '@angular/core/testing';

import { PersonalityServiceService } from './personality-service.service';

describe('PersonalityServiceService', () => {
  let service: PersonalityServiceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PersonalityServiceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
