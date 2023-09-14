import { Component, OnDestroy } from '@angular/core';
import { ShippingService } from 'src/app/Services/shipping.service';
import { Store, select } from '@ngrx/store';
import { Shipping } from 'src/app/Models/shipping';
import * as shippingAction from '../../Store/actions/shipping.action';
import { Appstate } from 'src/app/shared/store/appstate';
import { selectAppState } from 'src/app/shared/store/app.selector';
import { Router } from '@angular/router';
import { senderId } from 'src/app/Store/Selector/ShippingSelectore';
import { map, switchMap } from 'rxjs';
@Component({
  selector: 'app-add-table',
  templateUrl: './add-table.component.html',
  styleUrls: ['./add-table.component.scss'],
})
export class AddTableComponent implements OnDestroy {
  ngOnDestroy(): void {
    this.store.dispatch(shippingAction.sendSenderId({ id: '' }));
  }
  shippingPost: Shipping;
  constructor(
    private store: Store,
    private appstore: Store<Appstate>,
    private route: Router
  ) {
    this.shippingPost = new Shipping();
  }

  createShipping() {
    this.shippingPost.UserId = '';
    this.store.pipe(select(senderId)).subscribe({
      next: (data) => {
        this.shippingPost.UserId = data;
      },
    });
    // this.store.pipe(select(senderId)).pipe(map(data=>{
    //   console.log(data);
    //   this.shippingPost.UserId = data}));

    //console.log(this.shippingPost);
    //this.shippingPost.UserId = "d66e9813-4a7e-4b46-b49a-6e68d4d0a065";

    this.store.dispatch(
      shippingAction.addShippingInitial({ shipping: this.shippingPost })
    );
    let apiStatus$ = this.appstore.pipe(select(selectAppState));
    apiStatus$.subscribe((apState) => {
      if (apState.apiStatus == '1' && this.shippingPost.UserId == '') {
        this.route.navigateByUrl('');
      }

      this.route.navigateByUrl('/invitationTable');
    });
  }
}

//}
