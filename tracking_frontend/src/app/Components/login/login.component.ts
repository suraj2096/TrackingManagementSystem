import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Login } from 'src/app/Models/login';
import { LoginRegisterService } from 'src/app/Services/login-register.service';
import {Store, select} from "@ngrx/store";
import * as loginActions from "../../Store/actions/login.action";
import { selectLogins } from 'src/app/Store/Selector/LoginRegisterSelector';
import { Appstate } from 'src/app/shared/store/appstate';
import { selectAppState } from 'src/app/shared/store/app.selector';
import { setAPIStatus } from 'src/app/shared/store/app.action';
import { Observable } from 'rxjs';
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  // declare members.
  apiStatus$:Observable<any>;
   LoginUser:Login;
    constructor(private store:Store,private appStore:Store<Appstate>,private route:Router){
      this.LoginUser = new Login();
      this.apiStatus$ = new Observable();
    }
   ngOnInit(): void {
   }
   loginClick(){

    this.store.dispatch(loginActions.LoginInitiate({login:this.LoginUser}));
    this.apiStatus$ = this.appStore.pipe(select(selectAppState));
    this.apiStatus$.subscribe((apState) => {
      if (apState.apiStatus == 'success') {
        //alert(apState.apiResponseMessage);
       this.route.navigate(['']);
      }
      this.LoginUser = {...this.LoginUser}

    });

   }
}
