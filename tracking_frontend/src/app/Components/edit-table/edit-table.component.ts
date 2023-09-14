import { Component, DoCheck, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Store, select } from '@ngrx/store';
import { filter, mergeMap, switchMap } from 'rxjs';
import { Shipping } from 'src/app/Models/shipping';
import { selectShippingById } from 'src/app/Store/Selector/ShippingSelectore';
import { selectAppState } from 'src/app/shared/store/app.selector';
import * as shippingAction from '../../Store/actions/shipping.action';

@Component({
  selector: 'app-edit-table',
  templateUrl: './edit-table.component.html',
  styleUrls: ['./edit-table.component.scss']
})
export class EditTableComponent implements OnInit {
  id:any;
  shipppingUpdatePost:any = {
    shippingId:0,
    name:"",
    senderAddress:"",
    receiverAddress:"",
    shippingMethod:"",
    shippingStatus :"",
    shippingCost:"",
    userId:""
  };
  


  constructor(private store:Store,private router:ActivatedRoute,private route:Router){
   
  }
  ngOnInit(): void {
    this.id = this.router.snapshot.paramMap.get('id');
    this.id = Number.parseInt(this.id.toString().replace(':',''));
    console.log(this.id);
    let fetchData$ =  this.store.pipe(select(selectShippingById(this.id)));
    fetchData$.subscribe((data) => {
      if (data) {
        this.shipppingUpdatePost = {...data[0]};
        console.log(this.shipppingUpdatePost);
      } 
    });
  
  }

  editShipping(){
    console.log(this.shipppingUpdatePost);
    this.store.dispatch(shippingAction.updateShippingInitial({shipping:this.shipppingUpdatePost}));
    let apiStatus$ = this.store.pipe(select(selectAppState));
    apiStatus$.subscribe((apState) => {
      if (apState.apiStatus == '1') {
        this.route.navigateByUrl('');
      }
    }); 
  }
}
