import type { OrderRequest, OrderResult } from "../domain/Order";

function placeOrder(orderRequest:OrderRequest):Promise<OrderResult>{
    return new Promise((resolve) => {
        setTimeout(() => {
            const orderId = 'ORD-' + Date.now();

            resolve({orderId:orderId,totalAmount:orderRequest.totaAmount})
        }, 800)
    });
}

export default placeOrder