import { createFeatureSelector,createSelector } from "@ngrx/store";

export const selectShipping = createFeatureSelector<any>('myShippings');
export const selectShippingById = (id: number) =>
  createSelector(selectShipping, (shipping: any) => {
    return shipping.filter((data:any)=>(data.shippingId || data.ShippingId) == id); 
  });
  export const senderId = createFeatureSelector<any>('senderInvitaionerId');