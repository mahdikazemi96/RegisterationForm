import { TestBed } from '@angular/core/testing';

import { PersonServiceService } from './person-service.service';

describe('PersonServiceService', () => {
  let service: PersonServiceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PersonServiceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
