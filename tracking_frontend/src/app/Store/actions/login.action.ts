import { createAction,props } from "@ngrx/store";
import { Login } from "src/app/Models/login";
export enum LoginStatus {
    USER_LOGIN = '[Login Page]Login',
    USER_LOGIN_SUCCESS="[Login Page] Login Success",
    USER_LOGIN_FAILURE = "[Login Page] Login Failure",
    USER_LOGOUT = "Logout user "
};
export const LoginInitiate = createAction(LoginStatus.USER_LOGIN,props<{login:any}>());

export const LoginSuccess = createAction(LoginStatus.USER_LOGIN_SUCCESS,props<{data:any,logout:boolean}>());

export const LoginFailure = createAction(LoginStatus.USER_LOGIN_FAILURE,props<{message:string}>());

export const Logout = createAction(LoginStatus.USER_LOGOUT,props<{data:{result:any,logout:boolean}}>());