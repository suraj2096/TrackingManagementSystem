import { createAction,props } from "@ngrx/store";
import { Shipping } from "src/app/Models/shipping";
export enum ShippingStatus{
   GET_ALL_SHIPPING = 'shipping list invoke',
   GET_SHIPPING_SUCCESS= 'shipping list get',
   ADD_SHIPPING_INITIAL='add shipping initial',
    ADD_SHIPPING="add shipping",
    UPDATE_SHIPPING_INITIAL='update shipping initial',
    UPDATE_SHIPPING = "update shipping",
    DELETE_SHIPPING_INITIAL = "Delete shipping initial",
    DELETE_SHIPPING = "Delete shipping",
    NORMAL_SHIPPING="initial shipping ",
    INVITATIONER_SHIPPING="invitaioner shipping",
    INVITATIONER_SENDER_ID = "invitation sender Id"
};

export const getShipping = createAction(ShippingStatus.GET_ALL_SHIPPING);
export const getShippingSuccess = createAction(ShippingStatus.GET_SHIPPING_SUCCESS,props<{data:[]}>());
export const addShippingInitial = createAction(ShippingStatus.ADD_SHIPPING_INITIAL,props<{shipping:any}>());
export const addShipping = createAction(ShippingStatus.ADD_SHIPPING,props<{shipping:any}>());
export const updateShippingInitial = createAction(ShippingStatus.UPDATE_SHIPPING_INITIAL,props<{shipping:any}>());
export const updateShipping = createAction(ShippingStatus.UPDATE_SHIPPING,props<{shipping:any}>());
export const deleteShippingInitial = createAction(ShippingStatus.DELETE_SHIPPING_INITIAL,props<{id:any}>());
export const deleteShipping= createAction(ShippingStatus.DELETE_SHIPPING,props<{id:any}>());
export const normalShipping= createAction(ShippingStatus.NORMAL_SHIPPING,props<{newState:[]}>());
export const getInvitationerShipping = createAction(ShippingStatus.INVITATIONER_SHIPPING,props<{invitationerShippingId:any}>());
export const sendSenderId= createAction(ShippingStatus.INVITATIONER_SENDER_ID,props<{id:string}>());
