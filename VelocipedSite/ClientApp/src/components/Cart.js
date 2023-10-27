import {useEffect, useState} from "react";
import {NavMenu} from "./NavMenu";
import {useNavigate} from "react-router-dom";

export default function Cart() {    
    const [shop, setShop] = useState({});
    const [loading, setLoading] = useState(true);
    const [cart, setCart] = useState(JSON.parse(localStorage.getItem("cart")));
    const [reload, setReload] = useState(false);

    const shopId = cart.length > 0 ? cart[0].shopId : "";
    document.title = "Корзина";

    useEffect(() => {
        getShop();
        getGoods()
            .then(() => setReload(!reload));
        // eslint-disable-next-line
    }, []);
    
    let nav = useNavigate();
    
    async function getShop() {
        if (shopId === "")
            return;
        
        let response = await fetch("/api/v1/Shops/GetShopById", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                id: shopId
            })
        });
        let data = await response.json();
        setShop(data.shop);
        document.title = "Корзина - " + data.shop.name;
    }
    
    async function getGoods() {
        setCart(await Promise.all(cart.map(async x => {
            const response = await fetch ("/api/v1/Products/GetProductById", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    productId: x.id,
                    categoryId: x.categoryId,
                    shopId: x.shopId
                })
            });
            
            const data = (await response.json()).product;
            data.quantity = x.quantity;
            return data;
        })));
        setLoading(false);
    }
    
    function addQuantity(product) {
        let index = cart.indexOf(product);
        if (cart[index].quantity < 20)
            cart[index].quantity++;
        
        localStorage.setItem("cart", JSON.stringify(cart));
        setReload(!reload);
    }
    
    function subQuantity(product) {
        let index = cart.indexOf(product);
        if (cart[index].quantity > 1)
            cart[index].quantity--;
        
        localStorage.setItem("cart", JSON.stringify(cart));
        setReload(!reload);
    }
    
    function removeFromCart(product) {
        let index = cart.indexOf(product);
        cart.splice(index, 1);
        localStorage.setItem("cart", JSON.stringify(cart));
        setReload(!reload);
        NavMenu.rerender();
    }
    
    const sum = cart.reduce((partialSum, a) => partialSum + ((a.isOnSale ? a.salePrice : a.price) * a.quantity), 0) + shop.deliveryPrice;
    console.log(shop);
    
    return (
        <div>
            <div style={{justifyContent: "center"}} className={"row"}>
                <img className={"shopBanner col-md-4 d-flex align-items-center"} src={"images/" + shop.pathToImg} alt={""}/><br/>
            </div>
            <br/>
            {cart.length > 0 ?
                loading ? <em>Loading...</em> :
                <table align={"center"} width={"90%"}>
                    <thead>
                        <tr>
                            <td>Фото</td>
                            <td>Наименование</td>
                            <td>Цена</td>
                            <td>Кол-во</td>
                            <td>Сумма</td>
                            <td></td>
                        </tr>
                    </thead>
                    <tbody>
                        {cart.map(x => 
                            <tr>
                                <td className={"categoryBanner"}>
                                    <img src={"images/products/" + shopId + "/" + x.pathToImg} alt={""}/>
                                </td>
                                <td><p>{x.name}</p></td>
                                {
                                    x.isOnSale
                                        ? <td><p style={{color: "red", fontWeight: "bold"}}>{x.salePrice}</p></td>
                                        : <td><p>{x.price}</p></td>
                                }
                                <td>
                                    <p className={"justify-center"}>
                                        <button onClick={() => addQuantity(x)} className={"btn btn-outline-info"}>+</button>
                                        {x.quantity}
                                        <button onClick={() => subQuantity(x)} className={"btn btn-outline-info"}>-</button>
                                    </p>
                                </td>
                                <td>
                                    <p>{((x.isOnSale ? x.salePrice : x.price) * x.quantity).toFixed(2)}</p>
                                </td>
                                <td>
                                    <button onClick={() => removeFromCart(x)} className={"btn btn-danger"}>
                                        <span className="material-symbols-outlined">
                                            delete
                                        </span>
                                    </button>
                                </td>
                            </tr>
                        )}
                    </tbody>
                    <tfoot>
                        <tr>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td>Доставка:</td>
                            <td>{shop.deliveryPrice} руб.</td>
                        </tr>
                        <tr>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td><b>Итого:</b></td>
                            <td>{sum.toFixed(2)} руб.</td>
                        </tr>
                        <tr>
                            <td colSpan={5}>
                                {
                                    shop.minPrice > (sum - shop.deliveryPrice) 
                                        ? <b>Минимальная сумма заказа без учета стоимости доставки из данного магазина: {shop.minPrice} руб.</b>
                                        : ""
                                }
                            </td>
                            <td>
                                {
                                    shop.minPrice > (sum - shop.deliveryPrice) 
                                        ? <button className={"btn btn-danger"} disabled={true}>Оформить заказ</button>
                                        : <button className={"btn btn-success"} onClick={() => nav("/checkout")}>Оформить заказ</button>
                                }
                            </td>
                        </tr>
                    </tfoot>
                </table>
                : <p>Корзина пуста</p>
            }
        </div>
    );
}