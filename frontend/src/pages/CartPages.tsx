import { useCart } from "../hooks/useCart";
import { Link } from "react-router-dom";

function CartPages() {
  const {
    items,
    totalItems,
    totalAmount,
    increaseQuantity,
    decreaseQuantity,
    removeFromCart,
    clearCart,
  } = useCart();

  if (items.length === 0) {
    return (
      <div className="message">
        <h2>Your cart is empty</h2>
        <Link to="/">Continue shoping</Link>
      </div>
    );
  }
  return (
    <div className="page">
      <h2>Shopping Cart</h2>
      <div className="cart-layout">
        <div className="cart-items">
          {items.map((item) => (
            <div key={item.product.id}>
              <img src={item.product.imageUrl} alt={item.product.name} />
              <div className="cart-item-info">
                <Link to={`/products/${item.product.id}`}>
                  <h3>{item.product.name}</h3>
                </Link>

                <p className="price">{item.product.price}</p>

                <div className="quantity-box">
                  <button
                    onClick={() => {
                      decreaseQuantity(item.product.id);
                    }}
                    disabled={item.quantity === 1}
                  >
                    -
                  </button>
                  <span>{item.quantity}</span>
                  <button
                    onClick={() => {
                      increaseQuantity(item.product.id);
                    }}
                    disabled={item.quantity >= item.product.stock}
                  >
                    +
                  </button>
                </div>

                <p>Item total: ${item.product.price * item.quantity}</p>

                <button
                  className="link-button"
                  onClick={() => removeFromCart(item.product.id)}
                >
                  Remove
                </button>
              </div>
            </div>
          ))}
        </div>

        <div className="summary">
          <h3>Order Summary</h3>
          <p>Items: {totalItems}</p>
          <p className="price">Total: ₹{totalAmount}</p>
          <Link to="/checkout" className="checkout-link">
            Proceed to checkout
          </Link>
          <button className="link-button" onClick={clearCart}>
            Clear cart
          </button>
        </div>
      </div>
    </div>
  );
}

export default CartPages;
