import { Component, OnInit } from '@angular/core';
import { Store, select } from '@ngrx/store';
import { selectLogins } from 'src/app/Store/Selector/LoginRegisterSelector';
import * as  userActionsAuth from '../../Store/actions/login.action';
import * as  shippingActionAuth from '../../Store/actions/shipping.action';
import {initalState} from '../../Store/reducers/LoginRegisterReducer';
import { setAPIStatus } from 'src/app/shared/store/app.action';
@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {
  selectData:any;
  constructor(private store:Store){}
 login$ =this.store.pipe(select(selectLogins));
 ngOnInit(): void {
  debugger
  this.login$.subscribe((data)=>{
    if(data.result == null){
      var localData = localStorage.getItem("user")
      if(localData!=null){
        this.store.dispatch(userActionsAuth.LoginSuccess({data:JSON.parse(localData),logout:false}));
    }
    }
  })
    

  }
  LogoutClick(){
    // here we will clear the local storage..
    localStorage.clear();
    this.store.dispatch(setAPIStatus({apiStatus:{apiResponseMessage:'',apiStatus:''}}));
  this.store.dispatch(userActionsAuth.Logout({data:{result:null,logout:true}}));
  this.store.dispatch(shippingActionAuth.normalShipping({newState:[]}));
  }
}
