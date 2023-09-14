import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Login } from '../Models/login';
import { Observable } from 'rxjs';
import { Register } from '../Models/register';

@Injectable({
  providedIn: 'root'
})
export class LoginRegisterService {

  constructor(private httpClient:HttpClient) { 
  }
  // here we will call login api......
  LoginApiCall(login:Login):Observable<any>{
    return this.httpClient.post(`${environment.Api_Base_URL}/login`,login);
  }
  // here we will call register api..........
  RegisterApiCall(register:Register):Observable<any>{
     return this.httpClient.post(`${environment.Api_Base_URL}/register`,register);
  }
}
