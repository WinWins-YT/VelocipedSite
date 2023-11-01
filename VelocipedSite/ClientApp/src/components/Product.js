import React, {useEffect, useState} from "react";
import {Container, Row} from "react-bootstrap";
import {NavMenu} from "./NavMenu";

export default function Product() {
    const url = new URL(window.location.href);
    const shopId = url.searchParams.get("shop");
    const catalogId = url.searchParams.get("catalog");
    const productId = url.searchParams.get("id");

    let cart = JSON.parse(localStorage.getItem("cart"));
    
    const [product, setProduct] = useState({});
    const [reload, setReload] = useState(false);
    const [showDialog, setShowDialog] = useState(false);

    useEffect(() => {
        getProduct();

        // eslint-disable-next-line
    }, []);
    
    async function getProduct() {
        const response = await fetch("/api/v1/Products/GetProductById", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                shopId: shopId,
                categoryId: catalogId,
                productId: productId
            })
        });
        const data = await response.json();
        document.title = data.product.name;
        
        setProduct(data.product);
    }

    function addToCart(product) {
        cart = JSON.parse(localStorage.getItem("cart"));
        if (cart.some(x => x.shopId !== shopId)) {
            setShowDialog(true);
            return;
        }
        cart.push({
            id: product.id,
            categoryId: product.categoryId,
            shopId: product.shopId,
            quantity: 1
        });
        localStorage.setItem("cart", JSON.stringify(cart));
        setReload(!reload);
        NavMenu.rerender();
    }

    function removeFromCart(product) {
        cart = JSON.parse(localStorage.getItem("cart"));
        cart = cart.filter(x => x.id !== product.id);
        localStorage.setItem("cart", JSON.stringify(cart));
        setReload(!reload);
        NavMenu.rerender();
    }

    function clearCart() {
        localStorage.setItem("cart", "[]");
        setReload(!reload);
        NavMenu.rerender();
        setShowDialog(false);
    }
    
    return (
        <Container>
            <Row>
                <div className="col-lg-auto">
                    <img className="productDetails" src={`images/products/${shopId}/${product.pathToImg}`} alt="" />
                </div>
                <div className="col-lg-6">
                    <dl className="row">
                        <dt className="col-sm-4">Название</dt>
                        <dd className="col-sm-10">{product.name}</dd>
                        <dt className="col-sm-4">Описание</dt>
                        <dd className="col-sm-10">{product.description}</dd>
                        <dt className="col-sm-4">Цена</dt>
                        {product.isOnSale ?
                            <dd className="col-sm-10">
                                <p style={{textDecoration: "line-through", marginBottom: "0px"}}>{product.price} руб.</p>
                                <p className="col-sm-10" style={{color: "red", fontWeight: "bold"}}>{product.salePrice} руб.</p>
                            </dd> :
                            <dd className="col-sm-10">{product.price} руб.</dd>
                        }
                        {cart.some(e => e.id === product.id)
                            ? <button onClick={() => removeFromCart(product)} className="btn btn-outline-danger align-self-center">Удалить из корзины</button>
                            : <button onClick={() => addToCart(product)} className="btn btn-primary align-self-center">Добавить в корзину</button>}
                    </dl>
                </div>
            </Row>
            {
                showDialog && <DialogBox onNo={setShowDialog} onYes={clearCart} />
            }
        </Container>
    )
}

function DialogBox(props) {
    return (
        <div className={"fullItem"}>
            <div>
                <p>У вас в корзине есть товары из другого магазина. Чтобы заказать из разных магазинов, сделайте раздельные заказы. Хотите очистить корзину?</p>
                <button onClick={props.onYes} className={"btn btn-success"}>Да</button>
                <button onClick={() => props.onNo(false)} className={"btn btn-danger"} style={{marginLeft: "8px"}}>Нет</button>
            </div>
        </div>
    )
}