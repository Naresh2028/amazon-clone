
import { Link } from "react-router-dom";
import type { Product } from "../domain/Product";

interface ProductCardProps {
  product: Product;
}

// Why a separate component: the card is used by the Home page now and will be reused in search results and the cart later. 
// It receives data through props and has no state of its own.

function ProductCard({ product }: ProductCardProps) {
  return (
    <>
      <Link to={`/products/${product.id}`} className="card">
        <img src={product.imageUrl} alt={product.name} />
        <h3>{product.name}</h3>
        <p className="price">{product.price}</p>
        <p className="rating">{product.rating}</p>
      </Link>
    </>
  );
}

export default ProductCard;
