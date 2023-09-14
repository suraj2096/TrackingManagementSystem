import { Action,createReducer,on } from "@ngrx/store";
import * as LoginActions from '../actions/login.action';
import * as RegisterActions from '../actions/register.action';

export interface State {
    result: any;
    logout:boolean;
  }
  export const initalState:State ={
    result:null,
    logout:false
  };

 export const loginRegisterReducer = createReducer(initalState,
    on(LoginActions.LoginSuccess,(state,{data})=>{
     var newState = {...state};
     newState.result = data;
     newState.logout = data.logout;
     return newState;
    }),
    on(RegisterActions.registerSuccess,(state,{data})=>{
        return data;
    }),
    on(LoginActions.Logout,(state,{data})=>{
       var newState = {...state};
        newState = {...data};
        return newState;
    }),
    on(LoginActions.LoginFailure,(state,{message})=>{
        console.log(message);
        return state;

    })
    );

    export const LogoutReducer = createReducer(initalState,
        on(LoginActions.Logout,(state,{data})=>{
            console.log("comessss");
           var newState = {...state};
            newState = {...data};
            return newState;
        }));
   



  