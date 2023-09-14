import { Action, createReducer, on } from '@ngrx/store';
import * as shippingActions from '../actions/shipping.action';
import { Shipping } from 'src/app/Models/shipping';

export const initialShippingList: any = [];
export const applicationSenderId: string = '';

export const shippingReducer = createReducer(
  initialShippingList,
  on(shippingActions.getShippingSuccess, (state, { data }) => {
    console.log(data);
    return [...data];
  }),
  on(shippingActions.addShipping, (state, { shipping }) => {
    let getState = [...state];
    getState.unshift(shipping);
    return getState;
  }),
  on(shippingActions.updateShipping, (state, { shipping }) => {
    let newBookState = [...state];
    return newBookState.map((datas) =>
      datas.shippingId == shipping.shippingId ? shipping : datas
    );
  }),
  on(shippingActions.deleteShipping, (state, { id }) => {
    let newBookState = [...state];
    let data = newBookState.filter((data) => {
      if (data.shippingId != id) return data;
    });
    console.log(data);
    return data;
  }),
  on(shippingActions.normalShipping, (state, { newState }) => {
    let getState = [...state];
    return (getState = newState);
  })
);

export const invitainSenderIdReducer = createReducer(
  applicationSenderId,
  on(shippingActions.sendSenderId, (state, { id }) => {
    return id;
  })
);
