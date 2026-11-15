import { Link, useParams } from "react-router-dom";
import { useState, useEffect } from "react";
import type { Product } from "../domain/Product";
import { getByProductId } from "../data/productService";
import { useCart } from '../hooks/useCart';

function ProductDetailsPage() {
  const { id } = useParams();
  const [product, setProduct] = useState<Product | null>(null);
  const [loading, SetIsLoading] = useState(true);
  const [justAdded, setJustAdded] = useState(false);

  const  {addToCart}  = useCart();

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

  function handleAddToCart(selectedProduct:Product){
    addToCart(selectedProduct);
    setJustAdded(true);
  }

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


          <button 
            disabled={product.stock === 0}
            onClick={() => handleAddToCart(product)}
              >Add to cart
          </button>

          {justAdded && (
            <p className="in-stock">
              Added to cart. <Link to="/cart">Go to cart</Link>
            </p>
          )}

          {
            justAdded && (
              <p className="in-stock">
                Added to cart. <Link to="/cart">Go to cart</Link>
              </p>
            )
          }
        </div>
      </div>
    </div>
  );
}

export default ProductDetailsPage;


