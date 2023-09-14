import {
  AfterContentChecked,
  AfterContentInit,
  Component,
  DoCheck,
  Input,
  OnChanges,
  OnInit,
  SimpleChanges,
} from '@angular/core';
import { Store, select } from '@ngrx/store';
import * as shippingAction from '../../Store/actions/shipping.action';
import { selectShipping } from 'src/app/Store/Selector/ShippingSelectore';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';
import { SwUpdate } from '@angular/service-worker';
@Component({
  selector: 'app-table',
  templateUrl: './table.component.html',
  styleUrls: ['./table.component.scss'],
})
export class TableComponent implements OnInit, OnChanges {
  @Input() invitaionerPersonId: string = '';
  shippingData$: Observable<any>;
  constructor(private store: Store, private router: Router,private swUpdate: SwUpdate) {
    debugger;
    this.shippingData$ = this.store.pipe(select(selectShipping));
  }
  ngOnChanges(): void {
    debugger;
    console.log(this.invitaionerPersonId);
    if (this.invitaionerPersonId != '') {
      this.store.dispatch(
        shippingAction.getInvitationerShipping({
          invitationerShippingId: this.invitaionerPersonId,
        })
      );
    } else {
      this.store.dispatch(shippingAction.getShipping());
    }
  }

  ngOnInit(): void {
    debugger
    if (this.swUpdate.isEnabled) {
      this.swUpdate.available.subscribe(() => {
        if(confirm("You're using an old version of the control panel. Want to update?")) {
          window.location.reload();
        }
      });
    this.store.dispatch(shippingAction.getShipping());
  }
}

  addTable() {
    this.store.dispatch(
      shippingAction.sendSenderId({ id: this.invitaionerPersonId })
    );
  }
  editClick(id: any) {
    this.router.navigateByUrl(`edit/:${id}`);
  }
  deleteClick(id: any) {
    this.store.dispatch(shippingAction.deleteShippingInitial({ id }));
  }
}
