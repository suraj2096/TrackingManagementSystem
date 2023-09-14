import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './Components/login/login.component';
import { RegisterComponent } from './Components/register/register.component';
import { TableComponent } from './Components/table/table.component';
import { InvitaionComponent } from './Components/invitaion/invitaion.component';
import { InvitationConfirmationComponent } from './Components/invitation-confirmation/invitation-confirmation.component';
import { InvitationTableComponent } from './Components/invitation-table/invitation-table.component';
import { AddTableComponent } from './Components/add-table/add-table.component';

const routes: Routes = [

  {path:"login",component:LoginComponent},
  {path:"register",component:RegisterComponent},
  {path:"",
  loadChildren:()=>
    import('./shipping/shipping.module').then((co)=>co.ShippingModule)
},
{
  path:"invitation",component:InvitaionComponent
},
{
  path:"invitationConfirmation/:receiverId/:status",component:InvitationConfirmationComponent
},
{
  path:"invitationTable",component:InvitationTableComponent
}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
