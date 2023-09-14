import { NgModule, isDevMode } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './Components/login/login.component';
import { RegisterComponent } from './Components/register/register.component';
import { HeaderComponent } from './Components/header/header.component';
import { FormsModule } from '@angular/forms';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
// ngrx related file
import {StoreModule} from '@ngrx/store';
import {EffectsModule} from '@ngrx/effects';
import { loginRegisterReducer } from './Store/reducers/LoginRegisterReducer';
import { UserEffects } from './Store/effects/userLoginSignupEffect';
import {StoreDevtoolsModule} from '@ngrx/store-devtools';
import { TableComponent } from './Components/table/table.component';
import { appReducer } from './shared/store/app.reducer';
import { AddTableComponent } from './Components/add-table/add-table.component';
import { EditTableComponent } from './Components/edit-table/edit-table.component';
import { InvitaionComponent } from './Components/invitaion/invitaion.component';
import { JwtInterceptorService } from './Services/jwt-interceptor.service';
import { InvitationConfirmationComponent } from './Components/invitation-confirmation/invitation-confirmation.component';
import { InvitationTableComponent } from './Components/invitation-table/invitation-table.component';
import { ServiceWorkerModule } from '@angular/service-worker';
@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    RegisterComponent,
    HeaderComponent,
    TableComponent,
    AddTableComponent,
    EditTableComponent,
    InvitaionComponent,
    InvitationConfirmationComponent,
    InvitationTableComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule,
    StoreModule.forRoot({}),
    StoreModule.forRoot({ appState: appReducer }),
    StoreModule.forFeature("login",loginRegisterReducer ),
    StoreDevtoolsModule.instrument({
      maxAge: 25, // Retains last 25 states
    }),
    EffectsModule.forRoot({}),
    EffectsModule.forFeature(UserEffects),
    ServiceWorkerModule.register('ngsw-worker.js', {
      enabled: !isDevMode(),
      // Register the ServiceWorker as soon as the application is stable
      // or after 30 seconds (whichever comes first).
      registrationStrategy: 'registerWhenStable:30000'
    })
  ],
  providers: [
    {
      provide:HTTP_INTERCEPTORS,
      useClass:JwtInterceptorService,
      multi:true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
