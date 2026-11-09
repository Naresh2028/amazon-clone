import { Link, useParams } from "react-router-dom";
import { useState, useEffect } from "react";
import type { Product } from "../domain/Product";
import { getByProductId } from "../data/productService";

function ProductDetailsPage() {
  const { id } = useParams();
  const [product, setProduct] = useState<Product | null>(null);
  const [loading, SetIsLoading] = useState(true);

  useEffect(() => {
    let isActive = true;

    getByProductId(Number(id)).then((data) => {
      if (isActive) {
        setProduct(data);
        SetIsLoading(false);
      }
    });

    return () => {
      isActive = false;
    };
  }, [id]);

  if (loading) {
    return <p className="message">Loading Porducts..</p>;
  }

  if (!product || product === null) {
    return (
      <div className="message">
        <p>Product Not Found</p>
        <Link to="/">Back to products</Link>
      </div>
    );
  }

  return (
    <div className="page">
      <Link to="/"> Back to Products</Link>
      <div className="detail">
        <img src={product.imageUrl} alt={product.name} />

        <div className="detail-info">
          <h2>{product.name}</h2>
          <p className="rating">{product.rating}</p>
          <p className="price">{product.price}</p>
          <p>{product.description}</p>

          {product.stock > 0 ? (
            <p className="in-stock">In stock ({product.stock} Left)</p>
          ) : (
            <p className="out-of-stock">Out of Stock</p>
          )}

          <button disabled={product.stock === 0}>Add to cart</button>
        </div>
      </div>
    </div>
  );
}

export default ProductDetailsPage;
