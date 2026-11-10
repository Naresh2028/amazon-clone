import { type ReactNode, useState } from "react";
import type { CartItem } from "../domain/CartItem";
import type { Product } from "../domain/Product";
import { CartContext, type CartContextType } from "./CartContextType";

interface CartProviderProps {
  children: ReactNode;
}

function CartProvider({ children }: CartProviderProps) {
  const [items, setItems] = useState<CartItem[]>([]);

  function addToCart(product: Product) {
    const updatedCart: CartItem[] = [];
    let alreadyInCart = false;

    for (let i = 0; i < items.length; i++) {
      const item = items[i];

      if (item.product.id === product.id) {
        alreadyInCart = true;

        if (item.quantity < product.stock) {
          updatedCart.push({
            product: item.product,
            quantity: item.quantity + 1,
          });
        } else {
          updatedCart.push(item);
        }
      } else {
        updatedCart.push(item);
      }
    }

    if (!alreadyInCart) {
      updatedCart.push({ product: product, quantity: 1 });
    }

    setItems(updatedCart);
  }

  function increaseQuantity(productId: number) {
    const updatedItems: CartItem[] = [];

    for (let i = 0; i < items.length; i++) {
      const item = items[i];

      if (item.product.id == productId && item.quantity < item.product.stock) {
        updatedItems.push({
          product: item.product,
          quantity: item.quantity + 1,
        });
      } else {
        updatedItems.push(item);
      }
    }

    setItems(updatedItems);
  }

  function decreaseQuantity(productId: number) {
    const updatedItems: CartItem[] = [];

    for (let i = 0; i < items.length; i++) {
      const item = items[i];

      if (item.product.id === productId && item.quantity > 1) {
        updatedItems.push({
          product: item.product,
          quantity: item.quantity - 1,
        });
      } else {
        updatedItems.push(item);
      }
    }

    setItems(updatedItems);
  }

  function removeFromCart(productId: number) {
    const updatedItems: CartItem[] = [];

    for (let i = 0; i < items.length; i++) {
      if (items[i].product.id === productId) {
        updatedItems.push(items[i]);
      }
    }

    setItems(items);
  }

  function clearCart() {
    setItems([]);
  }

  let totalItems = 0;
  let totalAmount = 0;

  for (let i = 0; i < items.length; i++) {
    totalItems = totalItems + items[i].quantity;
    totalAmount = totalAmount + items[i].product.price * items[i].quantity;
  }

  const value: CartContextType = {
    items: items,
    totalItems: totalItems,
    totalAmount: totalAmount,
    addToCart: addToCart,
    increaseQuantity: increaseQuantity,
    decreaseQuantity: decreaseQuantity,
    removeFromCart: removeFromCart,
    clearCart: clearCart,
  };

  return <CartContext.Provider value={value}>{children}</CartContext.Provider>;
}

export default CartProvider;
