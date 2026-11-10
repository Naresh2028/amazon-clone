import { CartContext, type CartContextType } from "../context/CartContextType";
import { useContext } from "react";

export function useCart(): CartContextType {
  const context = useContext(CartContext);

  if (context == null) {
    throw new Error("useCart must be used inside CartProvider");
  }

  return context;
}
