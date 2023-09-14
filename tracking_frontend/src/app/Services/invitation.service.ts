import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { InvitationerDetail } from '../Models/invitationer-detail';

@Injectable({
  providedIn: 'root'
})
export class InvitationService {

  constructor(private httpClient:HttpClient) { 
  }
  fetchUsers(userName:any):Observable<any>{
    return this.httpClient.get(`${environment.Api_Base_URL}/findPerson/${userName}`);
  }
  createInvitation(userInvitation:InvitationerDetail):Observable<any>{
    return this.httpClient.post(`${environment.Api_Base_URL}/invitationCreator`,userInvitation);
  }

   changeInvitationStatus(receiverId:string,status:any):Observable<any>{
    return this.httpClient.get(`${environment.Api_Base_URL}/changeInvitationTable/${receiverId}/${status}`);
   }
   displayInvitaion():Observable<any>{
    return this.httpClient.get(`${environment.Api_Base_URL}/getInvitedPerson`);
   }
   takeActionOnInvitaion(receiverId:string,action:number):Observable<any>{
    return this.httpClient.get(`${environment.Api_Base_URL}/changeInvitationTableAction/${receiverId}/${action}`);
   }
   fetchIvitationFromUser():Observable<any>{
    return this.httpClient.get(`${environment.Api_Base_URL}/getInvitationComesFromUser`);
   }
  
  getInvitationerShipping(invitationerId:string):Observable<any>{
    return this.httpClient.get(`${environment.Api_Base_URL}/getInvitationerShipping/${invitationerId}`);
 }
}
