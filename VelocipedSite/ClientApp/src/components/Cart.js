import {useEffect, useState} from "react";
import {NavMenu} from "./NavMenu";

export default function Cart() {
    let cart = JSON.parse(localStorage.getItem("cart"));
    const shopId = cart.length > 0 ? cart[0].shopId : "";
    
    const [shopImgUrl, setShopImgUrl] = useState("");
    const [reload, setReload] = useState(false);

    useEffect(() => {
        getShop();
        //getGoods();
        // eslint-disable-next-line
    }, []);
    
    async function getShop() {
        let response = await fetch("/api/v1/Shops/GetShopById?id=" + shopId);
        let data = await response.json();
        setShopImgUrl(data.shop.pathToImg);
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
    
    return (
        <div>
            <div style={{justifyContent: "center"}} className={"row"}>
                <img className={"shopBanner col-md-4 d-flex align-items-center"} src={"images/" + shopImgUrl} alt={""}/><br/>
            </div>
            <br/>
            {cart.length > 0 ?
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
                                <td><p>{x.price}</p></td>
                                <td>
                                    <p className={"justify-center"}>
                                        <button onClick={() => addQuantity(x)} className={"btn btn-outline-info"}>+</button>
                                        {x.quantity}
                                        <button onClick={() => subQuantity(x)} className={"btn btn-outline-info"}>-</button>
                                    </p>
                                </td>
                                <td>
                                    <p>{(x.price * x.quantity).toFixed(2)}</p>
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
                </table>
                : <p>Корзина пуста</p>
            }
        </div>
    );
}