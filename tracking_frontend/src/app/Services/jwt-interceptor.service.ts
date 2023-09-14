import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class JwtInterceptorService implements HttpInterceptor {

  constructor(private router:Router) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    var currentUser = {token:""};
    var getcurrentUser = localStorage.getItem('user');
    if(getcurrentUser !=null){
      currentUser.token = JSON.parse(getcurrentUser).token;
    }
    req = req.clone({
      setHeaders:{
        Authorization:"Bearer "+currentUser.token
      }
    
    });
    return next.handle(req);
    
  }
    

   
}
