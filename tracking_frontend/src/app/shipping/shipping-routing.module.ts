import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TableComponent } from 'src/app/Components/table/table.component';
import { AddTableComponent } from '../Components/add-table/add-table.component';
import { EditTableComponent } from '../Components/edit-table/edit-table.component';

const routes: Routes = [
  {path:"",component:TableComponent},
  {path:"addtable",component:AddTableComponent},
  {path:"edit/:id",component:EditTableComponent}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ShippingRoutingModule { }
