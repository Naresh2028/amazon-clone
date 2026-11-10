import { Routes, Route } from "react-router-dom";
import HomePage from "../pages/HomePage";
import ProductDetailsPage from "../pages/ProductDetails";
import CartPages from "../pages/CartPages";
import CheckOutPage from "../pages/CheckoutPages";

function AppRoutes() {
  return (
    <Routes>
      <Route path="/" element={<HomePage />} />
      <Route path="/products/:id" element={<ProductDetailsPage />} />
      <Route path="cart" element={<CartPages/>}/>
      <Route path="/checkout" element={<CheckOutPage/>} />
      <Route path="*" element={<p className="message">Page Not found</p>} />
    </Routes>
  );
}

export default AppRoutes;
