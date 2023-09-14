import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { InvitationService } from 'src/app/Services/invitation.service';

@Component({
  selector: 'app-invitation-confirmation',
  templateUrl: './invitation-confirmation.component.html',
  styleUrls: ['./invitation-confirmation.component.scss']
})
export class InvitationConfirmationComponent implements OnInit {
  status:any;
constructor(private router:ActivatedRoute,private invitationService:InvitationService){}
ngOnInit(): void {
  let getStatus = this.router.snapshot.paramMap.get('status')??"";
  let recevierId = this.router.snapshot.paramMap.get('receiverId');
  let status = Number.parseInt(getStatus.toString().replace(':',''));
  recevierId= recevierId?.toString().replace(':','')??"";

    this.status = status==1?"Approved":"Rejected";
  
  // now we will call the api and we will change the invitation as appropiately ............
   this.invitationService.changeInvitationStatus(recevierId,getStatus).subscribe({
    next:(data)=>{
       console.log(data);
    },
    error:(err)=>{
        console.log(err);
    }
   })
}

}
