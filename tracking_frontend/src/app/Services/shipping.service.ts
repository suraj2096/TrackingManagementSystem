import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Shipping } from '../Models/shipping';

@Injectable({
  providedIn: 'root'
})
export class ShippingService {

  constructor(private httpClient:HttpClient) { 
  }
  getAllShipping():Observable<any>{
     return this.httpClient.get(`${environment.Api_Base_URL}/shippings`);
  }
 
  createShipping(shipping:Shipping):Observable<any>{
    return this.httpClient.post(`${environment.Api_Base_URL}/createShipping`,shipping);
  }
  updateShipping(shipping:Shipping):Observable<any>{
    return this.httpClient.put(`${environment.Api_Base_URL}/updateShipping`,shipping);
  }
  deleteShipping(id:any):Observable<any>{
    return this.httpClient.delete(`${environment.Api_Base_URL}/deleteShipping/${id}`);
  }
}
