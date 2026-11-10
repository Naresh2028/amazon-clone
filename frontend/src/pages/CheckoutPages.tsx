import React, { useState, type ChangeEvent } from "react";
import type { PaymentMethod, ShippingDetails } from "../domain/Order";
import { useCart } from "../hooks/useCart";
import { Link } from "react-router-dom";
import placeOrder from "../data/orderService";

interface FormErrors {
  fullName?: string;
  phone?: string;
  address?: string;
  city?: string;
  pincode?: string;
}

function CheckOutPage() {
  const {items, totalItems, totalAmount, clearCart} = useCart();

  const [formData, setFormData] = useState<ShippingDetails>({
    fullName: "",
    phone: "",
    address: "",
    city: "",
    pincode: "",
  });
  const [errors, setErrors] = useState<FormErrors>({});
  const [paymentMethod, setPaymentMethod] = useState<PaymentMethod>('COD');
  const [submitError, setSubmitError] = useState("");
  const [isPlacing, setIsPlacing] = useState(false);
  const [placeOderId, setPlaceOrderId] = useState<string | null>(null);

  function handleFormChange(e: ChangeEvent<HTMLInputElement>) {
    const { name, value } = e.target;

    setFormData({ ...formData, [name]: value });
  }

  function validate(): FormErrors {
    const result: FormErrors = {};

    if (formData.fullName.trim() === "") {
      result.fullName = "Full name is required.";
    }
    if (!/^\d{10}$/.test(formData.phone)) {
      result.phone = "Enter a 10-digit phone number.";
    }
    if (formData.address.trim().length < 10) {
      result.address = "Address must be at least 10 characters.";
    }
    if (formData.city.trim() === "") {
      result.city = "City is required.";
    }
    if (!/^\d{6}$/.test(formData.pincode)) {
      result.pincode = "Enter a 6-digit pincode.";
    }

    return result;
  }

  async function handleSubmit(e: React.SubmitEvent<HTMLFormElement>) {
    e.preventDefault();

    const validationErrors = validate();

    setErrors(validationErrors);

    if(Object.keys(validationErrors).length > 0){
        return;
    }

    setIsPlacing(true);
    setSubmitError('');

    try{
        const result = await placeOrder({
            items:items,
            shippingDetails:formData,
            totaAmount: totalAmount,
            paymentMethod : paymentMethod
        });

        setPlaceOrderId(result.orderId);
        clearCart();
    }
    catch{
        setSubmitError('Could not place the order, please try again');
    }
    finally{
        setIsPlacing(false);
    }
  }



  if (placeOderId !== null) {
    return (
      <div className="message">
        <h3>Order Placed Successfully</h3>
        <p>
          Your order number is <strong>{placeOderId} </strong>.
        </p>
        <Link to="/">Continue Shopping</Link>
      </div>
    );
  }

  if (items.length === 0) {
    return (
      <div className="message">
        <h2>Your cart is empty</h2>
        <Link to="/">Continue Shopping</Link>
      </div>
    );
  }

  return (
    <div className="Page">
      <h2>Checkout</h2>
      <div className="cart-Layout">
        <form className="checkout-form" onSubmit={handleSubmit} noValidate>
          <h3>Shipping Details</h3>

          <label>Full Name</label>
          <input
            type="text"
            name="fullName"
            value={formData.fullName}
            onChange={handleFormChange}
          />
          {errors.fullName && <p className="error">{errors.fullName}</p>}

          <label>Phone</label>
          <input
            type="text"
            name="phone"
            value={formData.phone}
            onChange={handleFormChange}
          />
          {errors.phone && <p className="error">{errors.phone}</p>}

          <label>Address</label>
          <input
            name="address"
            type="text"
            value={formData.address}
            onChange={handleFormChange}
          />
          {errors.address && <p className="error">{errors.address}</p>}

          <label>City</label>
          <input
            type="text"
            name="city"
            value={formData.city}
            onChange={handleFormChange}
          />
          {errors.city && <p className="error">{errors.city}</p>}

          <label>Pincode</label>
          <input
            type="text"
            name="pincode"
            value={formData.pincode}
            onChange={handleFormChange}
          />
          {errors.pincode && <p className="error">{errors.pincode}</p>}

          <h3>Payment Method</h3>
          <label className="radio-row">
            <input
              type="radio"
              checked={paymentMethod === "COD"}
              onChange={() => {
                setPaymentMethod("COD");
              }}
            />
            Cash on Delivery
          </label>

          <label className="card-row">
            <input
              type="radio"
              checked={paymentMethod === "CARD"}
              onChange={() => {
                setPaymentMethod("CARD");
              }}
            />
            Card (demo)
          </label>

          {submitError !== "" && <p className="error">{submitError}</p>}

          <button type="submit" disabled={isPlacing}>
            {isPlacing ? "Placing order..." : "Place order"}
          </button>

          <div className="summary">
            <h3>Order Summary</h3>
            <p>Items: {totalItems}</p>
            <p className="price">Total : Price {totalAmount}</p>
          </div>
        </form>
      </div>
    </div>
  );
}

export default CheckOutPage;
