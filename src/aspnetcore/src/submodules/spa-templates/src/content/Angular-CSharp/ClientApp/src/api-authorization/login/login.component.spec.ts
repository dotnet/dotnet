import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { LoginComponent } from './login.component';
import { ActivatedRoute, UrlSegment, convertToParamMap, Params, Router } from '@angular/router';
import { LoginActions } from '../api-authorization.constants';
import { AuthorizeService } from '../authorize.service';
import { HomeComponent } from 'src/app/home/home.component';

describe('LoginComponent', () => {
  let component: LoginComponent;
  let fixture: ComponentFixture<LoginComponent>;
  let router: Router;

  beforeEach(() => {
    let tempParams: Params = { id: '1234' };

    let segment0: UrlSegment = new UrlSegment('segment0', {});
    let segment1: UrlSegment = new UrlSegment(LoginActions.Login, {});

    let urlSegments: UrlSegment[] = [segment0, segment1];

    TestBed.configureTestingModule({
      imports: [
        RouterTestingModule.withRoutes([
        { path: 'authentication/login-failed', component: HomeComponent }
      ])],
      declarations: [LoginComponent, HomeComponent],
      providers: [{
        provide: ActivatedRoute, useValue: {
          snapshot: {
            paramMap: convertToParamMap(tempParams),
            url: urlSegments,
            queryParams: tempParams
          }
        }
      }]
    }).compileComponents();

    router = TestBed.inject(Router);
    spyOn(router, 'navigate').and.returnValue(Promise.resolve(true));
  });

  beforeEach(() => {
    let authService = TestBed.inject(AuthorizeService);

    spyOn(authService as any, 'createUserManager').and.callFake(() => {
      const userManager = jasmine.createSpyObj([]);
      return Promise.resolve(userManager);
    })

    fixture = TestBed.createComponent(LoginComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
