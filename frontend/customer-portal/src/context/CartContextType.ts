import type { CartItem } from "../domain/CartItem";
import type { Product } from "../domain/Product";
import {createContext} from 'react';


export interface CartContextType {
    items:CartItem[];
    totalItems:number;
    totalAmount:number;
    addToCart: (product:Product) => void;
    increaseQuantity: (productId:number) => void;
    decreaseQuantity: (productId:number) => void;
    removeFromCart: (productId:number) => void;
    clearCart:() => void; 
}

export const CartContext = createContext<CartContextType | null>(null);