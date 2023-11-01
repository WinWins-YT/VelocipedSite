import React, {useEffect, useState} from "react";
import {Container, Form} from "react-bootstrap";
import InputMask from "react-input-mask";

export default function Checkout() {
    const token = localStorage.getItem("auth_token");
    const [cart, setCart] = useState(JSON.parse(localStorage.getItem("cart")));
    const shopId = cart.length > 0 ? cart[0].shopId : "";
    document.title = "Оформление заказа";

    const [shop, setShop] = useState({});
    const [isAuth, setIsAuth] = useState(false);
    const [reload, setReload] = useState(false);

    const [address, setAddress] = useState("");
    const [phone, setPhone] = useState("");

    useEffect(() => {
        getUser(token);
        getShop();
        getProducts()
            .then(() => setReload(!reload));

        // eslint-disable-next-line
    }, []);

    function getUser(token) {
        fetch("/api/v1/Profile/GetUserByToken", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                token: token
            })
        })
            .then(response => response.json())
            .then(json => {
                setIsAuth(json.isValid);
                
                if (json.isValid) {
                    setAddress(json.user.address);
                    setPhone(json.user.phone);
                }
            });
    }

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
    
    async function getProducts() {
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
    }
    
    async function pay()  {
        
    }

    const sum = cart.reduce((partialSum, a) => partialSum + ((a.isOnSale ? a.salePrice : a.price) * a.quantity), 0) + shop.deliveryPrice;
    
    return (
        <Container>
            <div style={{justifyContent: "center"}} className={"row"}>
                <img className={"shopBanner col-md-4 d-flex align-items-center"} src={"images/" + shop.pathToImg} alt={""}/><br/>
            </div>
            {!isAuth ? 
                <p style={{color: "red", fontWeight: "bold"}}>Чтобы продолжить оформление, войдите в аккаунт или зарегистрируйтесь</p> :

                <>
                    <Form>
                        <Form.Group className="mb-3" controlId="userForm.addressControl">
                            <Form.Label>Адрес</Form.Label>
                            <Form.Control value={address}
                                          onChange={e => setAddress(e.target.value)} />
                        </Form.Group>
                        <Form.Group className="mb-3" controlId="userForm.phoneControl">
                            <Form.Label>Номер телефона</Form.Label>
                            <InputMask
                                className={"form-control mt-3"}
                                mask={"+7 (999) 999 99 99"}
                                value={phone}
                                onChange={e => setPhone(e.target.value)} />
                        </Form.Group>
                    </Form>
                    <h3>Итого к оплате: {sum} руб.</h3>
                    <button onClick={pay} className="btn btn-success">Оплатить</button>
                </>
            }
        </Container>
    )
}