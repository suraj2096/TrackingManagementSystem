import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { Observable, Subject,catchError,debounceTime, map, switchMap } from 'rxjs';
import { InvitationerDetail } from 'src/app/Models/invitationer-detail';
import { InvitationService } from 'src/app/Services/invitation.service';

@Component({
  selector: 'app-invitaion',
  templateUrl: './invitaion.component.html',
  styleUrls: ['./invitaion.component.scss']
})
export class InvitaionComponent implements OnInit {
  invitaionerList!:any[];
  displayInvitation:any;
  displayNameorNot:boolean=true;
  displayFindName:InvitationerDetail;
  results$: Observable<any>;
  subject = new Subject()
  constructor(private invitationService:InvitationService){
    this.displayInvitation = {
      display:'none'
    }
    this.results$ = new Observable<any>();
    this.displayFindName = new InvitationerDetail();
  }
  ngOnInit(): void {

    this.fetchInvitions();
    this.results$ = this.subject.pipe(
  
      debounceTime(2000),
      switchMap((searchKey)=>this.invitationService.fetchUsers(searchKey).pipe(map(response=>response.length==0?[{name:"No Such Person Available"}]:response)))
  )
  }
  // here we will take desigion 
  showInvitation(){
    if(this.displayInvitation.display == 'none'){
      this.displayInvitation = {
        display:'block'
      }
    }
    else{
      this.displayInvitation = {
        display:'none'
      }
    }
  }
  // now  we will implement debounce in angular........... and in this function we will get the value that user enter in the find user input .
  search(event:any) {
    if(!this.displayNameorNot){
      this.displayNameorNot = true;
    }
    const searchText = event.target.value;

    // emits the `searchText` into the stream. This will cause the operators in its pipe function (defined in the ngOnInit method) to be run. `debounce` runs and then `switchmap`. If the time interval of 1 sec in debounce hasn't elapsed, switchMap will not be called, thereby saving the server from being called.
    if(searchText.length == 0){
      this.displayNameorNot = false;
      return;
    }
    this.subject.next(searchText);
}
// here we will get the user from input and display it in the find input textbox.
displayNameClick(event:any,userId:string){
  debugger
 if(event.target.value == "No Such Person Available"){
  return ;
 }
 this.displayFindName.Id = userId;
  this.displayFindName.Name = event.target.value;

  this.displayNameorNot = false;
}

// here we will send invitation to the user ..................................
sendeInvitation(){
  let invitationUser = this.displayFindName;
    this.invitationService.createInvitation(invitationUser).subscribe({
      next:(data)=>{
        console.log(data);
        this.fetchInvitions();
      },
      error:(err)=>{
        console.log(err);
      }
    })
}



// display the invitation here ............
fetchInvitions(){
  this.invitationService.displayInvitaion().subscribe({
    next:(data)=>{
      this.invitaionerList = data;
      console.log(this.invitaionerList);
    },
    error:(err)=>{
      console.log(err);
    }
  })
}


// we will update the action based on  the user =---------------------------
updateAction(receiverId:string,action:any){
 this.invitationService.takeActionOnInvitaion(receiverId,action).subscribe({
  next:(data)=>{
    console.log(data);
    this.fetchInvitions();
  },
  error:(err)=>{
  console.log(err);
  }
 })
}

}
