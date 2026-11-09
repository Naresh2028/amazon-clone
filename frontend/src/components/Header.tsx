import { Link } from "react-router-dom";


// Link changes the page without a full reload.

export function Header() {
  return (
    <>
      <header>
        <Link to="/" className="logo">
          Amazon Clone
        </Link>
        <span className="cart-placeholder">Cart (0)</span>
      </header>
    </>
  );
}
