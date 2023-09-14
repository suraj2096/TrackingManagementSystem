import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { ShippingService } from 'src/app/Services/shipping.service';
import * as shippingAction from '../actions/shipping.action';
import { Store, select } from '@ngrx/store';
import {
  EMPTY,
  exhaustAll,
  exhaustMap,
  map,
  mergeMap,
  switchMap,
  withLatestFrom,
} from 'rxjs';
import { setAPIStatus } from 'src/app/shared/store/app.action';
import { selectShipping } from '../Selector/ShippingSelectore';
import { InvitationService } from 'src/app/Services/invitation.service';

@Injectable()
export class ShippingEffects {
  constructor(
    private actions$: Actions,
    private shippingService: ShippingService,
    private invitationService: InvitationService,
    private appstore: Store
  ) {}

  getAll$ = createEffect(() =>
    this.actions$.pipe(
      ofType(shippingAction.getShipping),
      withLatestFrom(this.appstore.pipe(select(selectShipping))),
      exhaustMap(([, ShippingFromStore]) => {
        debugger;
        console.log(ShippingFromStore);
        if (ShippingFromStore.length > 0) {
          return EMPTY;
        }
        return this.shippingService
          .getAllShipping()
          .pipe(
            map((datas) => shippingAction.getShippingSuccess({ data: datas }))
          );
      })
    )
  );

  getInvitaionerShipping$ = createEffect(() =>
    this.actions$.pipe(
      ofType(shippingAction.getInvitationerShipping),
      switchMap((actions) => {
        return this.invitationService
          .getInvitationerShipping(actions.invitationerShippingId)
          .pipe(
            map((datas) => shippingAction.getShippingSuccess({ data: datas }))
          );
      })
    )
  );

  create$ = createEffect(() =>
    this.actions$.pipe(
      ofType(shippingAction.addShippingInitial),
      switchMap((actions) => {
        return this.shippingService.createShipping(actions.shipping).pipe(
          map((response) => {
            if (response.status == 1) {
              this.appstore.dispatch(
                setAPIStatus({
                  apiStatus: {
                    apiResponseMessage: response.message,
                    apiStatus: '1',
                  },
                })
              );
            }
            return shippingAction.addShipping({ shipping: response.data });
          })
        );
      })
    )
  );

  update$ = createEffect(() =>
    this.actions$.pipe(
      ofType(shippingAction.updateShippingInitial),
      switchMap((actions) => {
        return this.shippingService.updateShipping(actions.shipping).pipe(
          map((response) => {
            if (response.status == 1) {
              this.appstore.dispatch(
                setAPIStatus({
                  apiStatus: {
                    apiResponseMessage: response.message,
                    apiStatus: '1',
                  },
                })
              );
            }
            return shippingAction.updateShipping({
              shipping: actions.shipping,
            });
          })
        );
      })
    )
  );

  delete$ = createEffect(() =>
    this.actions$.pipe(
      ofType(shippingAction.deleteShippingInitial),
      switchMap((actions) => {
        return this.shippingService.deleteShipping(actions.id).pipe(
          map((response) => {
            if (response.status == 1) {
              this.appstore.dispatch(
                setAPIStatus({
                  apiStatus: {
                    apiResponseMessage: response.message,
                    apiStatus: '1',
                  },
                })
              );
            }
            return shippingAction.deleteShipping({ id: actions.id });
          })
        );
      })
    )
  );
}
