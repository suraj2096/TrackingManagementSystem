import { Component } from '@angular/core';
import { Register } from 'src/app/Models/register';
import { LoginRegisterService } from 'src/app/Services/login-register.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent {

  RegisterUser:Register;
  constructor(private loginRegisterService:LoginRegisterService){
    this.RegisterUser = new Register();
  }
  RegisterUserClick(){
    this.loginRegisterService.RegisterApiCall(this.RegisterUser).subscribe({
      next:(data)=>{
        console.log(data);
      },
      error:(err)=>{
        console.log(err);
      }
    })
  }
}
