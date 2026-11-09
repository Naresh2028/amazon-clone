import type { Product } from "../domain/Product";


// one place for mock data: it gets deleted later without touching any page.

export const mockProducts: Product[] = [
  { id: 1, name: 'Wireless Headphones', description: 'Over-ear Bluetooth headphones with 30-hour battery life.', price: 2499, category: 'Electronics', imageUrl: 'https://picsum.photos/seed/p1/400/300', rating: 4.5, stock: 12 },
  { id: 2, name: 'Running Shoes', description: 'Lightweight shoes with breathable mesh and soft cushioning.', price: 3199, category: 'Fashion', imageUrl: 'https://picsum.photos/seed/p2/400/300', rating: 4.2, stock: 0 },
  { id: 3, name: 'Stainless Steel Bottle', description: 'Keeps drinks cold for 24 hours and hot for 12 hours.', price: 799, category: 'Home', imageUrl: 'https://picsum.photos/seed/p3/400/300', rating: 4.7, stock: 40 },
  { id: 4, name: 'Mechanical Keyboard', description: 'Compact keyboard with tactile switches and backlight.', price: 4599, category: 'Electronics', imageUrl: 'https://picsum.photos/seed/p4/400/300', rating: 4.6, stock: 7 },
  { id: 5, name: 'Backpack 30L', description: 'Water-resistant laptop backpack with multiple compartments.', price: 1599, category: 'Fashion', imageUrl: 'https://picsum.photos/seed/p5/400/300', rating: 4.3, stock: 25 },
  { id: 6, name: 'Desk Lamp', description: 'LED lamp with three brightness levels and USB charging port.', price: 1199, category: 'Home', imageUrl: 'https://picsum.photos/seed/p6/400/300', rating: 4.4, stock: 18 },
];