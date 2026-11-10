import { Link } from "react-router-dom";
import { useCart } from "../hooks/useCart";


// Link changes the page without a full reload.

export function Header() {
  const { totalItems } = useCart();
  return (
    <>
      <header>
        <Link to="/" className="logo">
          Amazon Clone
        </Link>
        <Link to="/cart" className="cart-link">
          Cart ({totalItems})
        </Link>
        <span className="cart-placeholder">Cart (0)</span>
      </header>
    </>
  );
}
