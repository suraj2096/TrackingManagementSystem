import { createAction,props } from "@ngrx/store";
import { Register } from "src/app/Models/register";

export enum RegisterEnum{
    Register = '[register page] Register',
    Register_Success = '[register page ] Register Success',
    Register_Error = '[register page] Register Error'
}
export const register = createAction(RegisterEnum.Register,props<{register:Register}>());

export const registerSuccess = createAction(RegisterEnum.Register_Success,props<{data:any}>());

export const registerError = createAction(RegisterEnum.Register_Error,props<{message:string}>());
