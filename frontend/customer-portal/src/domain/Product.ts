
// one shared definition of a product, used by every layer.

export interface Product {
    id:number;
    name:string;
    description:string;
    price:number;
    category:string;
    imageUrl:string;
    rating:number;
    stock:number
}