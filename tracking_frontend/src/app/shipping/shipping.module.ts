import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ShippingRoutingModule } from './shipping-routing.module';

import {StoreModule} from '@ngrx/store';
import {EffectsModule} from '@ngrx/effects';
import {shippingReducer,invitainSenderIdReducer} from  '../Store/reducers/shipping.reducer';
import {ShippingEffects} from  '../Store/effects/shipping.effect';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    ShippingRoutingModule,
    StoreModule.forFeature("myShippings",shippingReducer),
    StoreModule.forFeature('senderInvitaionerId',invitainSenderIdReducer),
    EffectsModule.forFeature(ShippingEffects)
  ]
})
export class ShippingModule { }
