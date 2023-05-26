import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
////#if (IndividualLocalAuth)
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
////#else
import { HttpClientModule } from '@angular/common/http';
////#endif

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
////#if (IndividualLocalAuth)
import { AuthorizeInterceptor } from 'src/api-authorization/authorize.interceptor';
////#endif

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'counter', component: CounterComponent },
      { path: 'fetch-data', component: FetchDataComponent },
    ])
  ],
  ////#if (IndividualLocalAuth)
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthorizeInterceptor, multi: true }
  ],
  ////#else
  providers: [],
  ////#endif
  bootstrap: [AppComponent]
})
export class AppModule { }
