import type { Product } from "../domain/Product";
import { mockProducts } from "./mockProducts";

export function getProducts(): Promise<Product[]> {
  return new Promise((resolve) => {
    setTimeout(() => resolve(mockProducts), 500);
  });
}

export function getByProductId(id: number): Promise<Product> {
  return new Promise((resolve) => {

    setTimeout(() => {
        
      let found: Product | null = null;

      for (let i = 0; i < mockProducts.length; i++) {
        if (mockProducts[i].id === id) {
          found = mockProducts[i];
          break;
        }
      }
      if(found){
        resolve(found);
      }
    }, 700);
  });
}
