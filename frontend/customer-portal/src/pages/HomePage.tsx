import { useState, useEffect } from "react";
import ProductCard from "../components/ProductCard";
import { getProducts } from "../data/productService";
import type { Product } from "../domain/Product";


// Why three states: 
// 1. products holds the data
// 2. isLoading shows the loading message
// 3. searchText holds the search box value. 
// Why useEffect with []: load once when the page opens.

function HomePage() {
  const [searchText, setSearchText] = useState("");
  const [isLoading, setLoading] = useState(true);
  const [products, setProducts] = useState<Product[]>([]);
  const visibleProducts: Product[] = [];

  useEffect(() => {
    let isActive = true;

    getProducts().then((data) => {
      if (isActive) {
        setProducts(data);
        setLoading(false);
      }
    });

    return () => {
      isActive = false;
    };
  }, []);

  for (let i = 0; i < products.length; i++) {
    const nameMatches = products[i].name
      .toLowerCase()
      .includes(searchText.toLowerCase());

    if (nameMatches) {
      visibleProducts.push(products[i]);
    }
  }

  if (isLoading) {
    return <p className="message">Loading Products</p>;
  }

  return (
    <div className="page">
      <input
        className="search"
        type="text"
        placeholder="Search products..."
        value={searchText}
        onChange={(e) => setSearchText(e.target.value)}
      />

      {visibleProducts.length === 0 ? (
        <p className="message">No Products found.</p>
      ) : (
        <div className="grid">
          {visibleProducts.map((p) => (
            <ProductCard key={p.id} product={p} />
          ))}
        </div>
      )}
    </div>
  );
}

export default HomePage;
