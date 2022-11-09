import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { LoginMenuComponent } from './login-menu.component';
import { AuthorizeService } from '../authorize.service';
import { of } from 'rxjs';

describe('LoginMenuComponent', () => {
  let component: LoginMenuComponent;
  let fixture: ComponentFixture<LoginMenuComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [RouterTestingModule],
      declarations: [ LoginMenuComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    let authService = TestBed.inject(AuthorizeService);

    spyOn(authService as any, 'createUserManager').and.callFake(() => {
      const userManager = jasmine.createSpyObj([]);
      return Promise.resolve(userManager);
    });
    spyOn(authService as any, 'getUserFromStorage').and.returnValue(
      of(null));

    fixture = TestBed.createComponent(LoginMenuComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
