import type { CartItem } from "./CartItem";


export type PaymentMethod = 'COD' | 'CARD';

export interface ShippingDetails {
    fullName:string;
    phone:string;
    address:string;
    city: string;
    pincode:string
}

export interface OrderRequest {
    items:CartItem[];
    paymentMethod:PaymentMethod;
    shippingDetails:ShippingDetails;
    totaAmount:number;
}

export interface OrderResult {
    orderId: string
    totalAmount:number;
}