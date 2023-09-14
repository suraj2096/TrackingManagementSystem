import { Injectable } from '@angular/core';
import { Actions, ofType, createEffect } from '@ngrx/effects';
import { exhaustMap, map, catchError, of, switchMap, EMPTY } from 'rxjs';
import { LoginRegisterService } from 'src/app/Services/login-register.service';
import * as LoginActions from '../actions/login.action';
import * as RegisterAction from '../actions/register.action';
import { Appstate } from 'src/app/shared/store/appstate';
import { Store } from '@ngrx/store';
import { setAPIStatus } from 'src/app/shared/store/app.action';

@Injectable()
export class UserEffects {
  constructor(
    private actions$: Actions,
    private loginRegisterService: LoginRegisterService,
    private appstore: Store<Appstate>
  ) {}
  //login effects,,,,,,,,,, and we will store the data to the localstorage........
  userLogin$ = createEffect(() =>
    this.actions$.pipe(
      ofType(LoginActions.LoginInitiate),
      switchMap((action) =>
        this.loginRegisterService.LoginApiCall(action.login).pipe(
          map(
            (response) => {
              console.log("hello");
              localStorage.setItem('user', JSON.stringify(response));
              this.appstore.dispatch(
                setAPIStatus({
                  apiStatus: {
                    apiResponseMessage: response.message,
                    apiStatus: 'success',
                  },
                })
              );
              return LoginActions.LoginSuccess({
                data: response,
                logout: false,
              });
            }),
            catchError((error: any) =>{ 
              this.appstore.dispatch(
                setAPIStatus({
                  apiStatus: {
                    apiResponseMessage: error.error.message,
                    apiStatus: 'failure',
                  },
                }));
                return EMPTY;
              return of(LoginActions.LoginFailure(error))
            })
          
        )
      )
    )
  );

  // register effects........
  registerLogin$ = createEffect(() =>
    this.actions$.pipe(
      ofType(RegisterAction.register),
      exhaustMap((action) =>
        this.loginRegisterService.RegisterApiCall(action.register).pipe(
          map((response) => RegisterAction.registerSuccess(response)),
          catchError((error: any) => of(RegisterAction.registerError(error)))
        )
      )
    )
  );
}
