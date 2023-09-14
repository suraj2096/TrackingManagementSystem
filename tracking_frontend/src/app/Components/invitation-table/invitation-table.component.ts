import { Component, OnDestroy, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { InvitationService } from 'src/app/Services/invitation.service';
import * as  shippingActionAuth from '../../Store/actions/shipping.action';

@Component({
  selector: 'app-invitation-table',
  templateUrl: './invitation-table.component.html',
  styleUrls: ['./invitation-table.component.scss']
})
export class InvitationTableComponent implements OnInit,OnDestroy {

constructor(private invitaionService:InvitationService,private store:Store){}

invitations:any;
showTable:boolean = false;
invitationsId:string = "";
ngOnInit(): void {
  this.getInvitations();
}
  getInvitations(){
    debugger
   this.invitaionService.fetchIvitationFromUser().subscribe({
    next:(data)=>{
       this.invitations = data;
       console.log(this.invitations);
    },
    error:(err)=>{
      console.log(err);
    }
   })
  }
  ngOnDestroy(): void {
   // this.store.dispatch(shippingActionAuth.normalShipping({newState:[]}));
  }


  showTableClick(invitationId:any){
    this.showTable = false;
    if(this.invitationsId == invitationId){
     this.showTable = true;
     return;
    }
    setTimeout(() => {
      this.invitationsId = invitationId;
      this.showTable = true;
    }, 100);
    
  }
  

}
