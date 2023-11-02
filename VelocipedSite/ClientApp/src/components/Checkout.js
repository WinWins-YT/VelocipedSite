import React, {useEffect, useRef, useState} from "react";
import {Container, Form} from "react-bootstrap";
import InputMask from "react-input-mask";

export default function Checkout() {
    const token = localStorage.getItem("auth_token");
    const [cart, setCart] = useState(JSON.parse(localStorage.getItem("cart")));
    const shopId = cart.length > 0 ? cart[0].shopId : "";
    document.title = "Оформление заказа";

    const [shop, setShop] = useState({});
    const [isAuth, setIsAuth] = useState(false);

    const [address, setAddress] = useState("");
    const [phone, setPhone] = useState("");
    const [promo, setPromo] = useState("");
    const [orderId, setOrderId] = useState(-1);
    const [formSubmit, setFormSubmit] = useState(false);
    const [isError, setIsError] = useState(false);
    const [promoError, setPromoError] = useState(false);
    const [promoErrorMessage, setPromoErrorMessage] = useState("");
    const [promoSuccess, setPromoSuccess] = useState(false);
    const [promoValue, setPromoValue] = useState(0);
    
    const sendForm = useRef(null);
    const payBtn = useRef(null);
    const promoBtn = useRef(null);

    useEffect(() => {
        getShop()
            .then(() => {
                getUser(token);
                getProducts();
            });
        
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
        console.log(data.shop);
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

    function sleep(ms) {
        return new Promise(resolve => setTimeout(resolve, ms));
    }
    
    async function pay()  {
        payBtn.current.disabled = true;
        setIsError(false);
        
        const response = await fetch("/api/v1/Orders/CreateOrder", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                token: token,
                address: address,
                phone: phone,
                products: cart,
                totalPrice: sum,
                saleValue: promoValue
            })
        });
        const data = await response.json();
        
        if (data.orderId === -1) {
            payBtn.current.disabled = false;
            setIsError(true);
            return;
        }
        
        localStorage.setItem("cart", "[]");
        
        setOrderId(data.orderId);
        await sleep(500);
        setFormSubmit(true);
    }
    
    async function applyPromo() {
        promoBtn.current.disabled = true;
        const response = await fetch("/api/v1/Promocode/GetPromocode", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                token: token,
                promocode: promo,
                sum: sum
            })
        });
        const data = await response.json();
        
        if (!data.isValid) {
            promoBtn.current.disabled = false;
            setPromoError(true);
            setPromoErrorMessage(data.errorMessage);
            return;
        }
        
        promoBtn.current.disabled = false;
        setPromoError(false);
        setPromoSuccess(true);
        setPromoValue(data.saleValue);
    }
    
    function form() {
        console.log(orderId);
        sendForm.current.elements["label"].value = orderId;
        sendForm.current.submit();
    }
    
    
    let sum = parseFloat(cart.reduce((partialSum, a) => partialSum + ((a.isOnSale ? a.salePrice : a.price) * a.quantity), 0) + shop.deliveryPrice - promoValue);
    
    return (
        <Container>
            <div style={{justifyContent: "center"}} className={"row"}>
                <img className={"shopBanner col-md-4 d-flex align-items-center"} src={"images/" + shop.pathToImg} alt={""}/><br/>
            </div>
            {!isAuth ? 
                <p style={{color: "red", fontWeight: "bold"}}>Чтобы продолжить оформление, войдите в аккаунт или зарегистрируйтесь</p> :

                <>
                    <Form ref={sendForm} method="POST" action="https://yoomoney.ru/quickpay/confirm">
                        <input type="hidden" name="receiver" value="4100118425939443"/>
                        <input type="hidden" name="label" value={orderId}/>
                        <input type="hidden" name="quickpay-form" value="button"/>
                        <input type="hidden" name="sum" value={sum.toFixed(2)} datatype="number"/>
                        <input type="hidden" name="paymentType" value="AC"/>
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
                        <Form.Group className="mb-3" controlId="userForm.phoneControl">
                            <Form.Control
                                style={{width: "90%", display: "inline"}}
                                placeholder="Промокод"
                                value={promo}
                                onChange={e => setPromo(e.target.value)} />
                            <button
                                ref={promoBtn}
                                style={{display: "inline", width: "auto"}}
                                className="btn btn-primary"
                                onClick={applyPromo}
                                type="button">Применить</button>
                            <p style={{color: "green"}}>
                                {promoSuccess ? `Промокод успешно применен: -${promoValue} руб.` : ""}
                            </p>
                            <p style={{color: "red"}}>
                                {promoError ? promoErrorMessage : ""}
                            </p>
                        </Form.Group>
                        <h3>Итого к оплате: {sum.toFixed(2)} руб.</h3>
                    </Form>
                    <button ref={payBtn} onClick={pay} className="btn btn-success">Оплатить</button>
                    <p style={{color: "red"}}>
                        {isError ? "Что-то пошло не так. Пожалуйста, попробуйте позднее" : ""}
                    </p>
                    {formSubmit && form()}
                </>
            }
        </Container>
    )
}