import {Button, Card, Container, Form, Row} from "react-bootstrap";
import {Link, useNavigate} from "react-router-dom";
import React, {useEffect, useRef, useState} from "react";
import InputMask from 'react-input-mask';

export default function Register() {
    const [email, setEmail] = useState("");
    const [password1, setPassword1] = useState("");
    const [password2, setPassword2] = useState("");
    const [firstName, setFirstName] = useState("");
    const [lastName, setLastName] = useState("");
    const [address, setAddress] = useState("");
    const [phone, setPhone] = useState("");
    
    const [isFetchError, setIsFetchError] = useState(false);
    const [isFailed, setIsFailed] = useState(false);
    const [errorMessage, setErrorMessage] = useState("");
    const [showDialog, setShowDialog] = useState(false);
    
    const regBtn = useRef(null);
    
    const nav = useNavigate();
    
    document.title = "Регистрация";

    useEffect(() => {
        regBtn.current.disabled =
            !/^[\w-.]+@([\w-]+\.)+[\w-]{2,4}$/gi.test(email) ||
            (password1 !== password2 && password1 !== "" && password2 !== "") ||
            !/^[А-яA-z]+$/g.test(firstName) ||
            !/^[А-яA-z]+$/g.test(lastName) ||
            !/^[A-zА-я-.,/\\ ]+$/g.test(address) ||
            !/^\+7 \(\d{3}\) \d{3} \d{2} \d{2}/g.test(phone);
    });
    
    async function register() {
        regBtn.current.disabled = true;
        setIsFailed(false);
        setIsFetchError(false);
        
        try {
            const response = await fetch("/api/v1/Profile/Register", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    email: email,
                    password: password1,
                    firstName: firstName,
                    lastName: lastName,
                    address: address,
                    phone: phone
                })
            });
            
            const data = await response.json();
            
            if (!data.isSuccess)
            {
                regBtn.current.disabled = false;
                setIsFailed(true);
                setErrorMessage(data.errorMessage);
            }
            else {
                setShowDialog(true);
            }
        }
        catch {
            regBtn.current.disabled = false;
            setIsFetchError(true);
        }
    }
    
    function regOK() {
        setShowDialog(false);
        nav("/");
    }
    
    return (
        <Container
            className="d-flex justify-content-center align-items-center">
            <Card style={{width: 600}} className="p-5">
                <h2 className="m-auto">Регистрация</h2>
                <Form className="d-flex flex-column">
                    <Form.Control
                        className="mt-3"
                        placeholder="Email"
                        value={email}
                        onChange={e => {
                            setEmail(e.target.value);
                        }}
                        type={"email"}
                    />
                    <p className={"d-flex justify-content-center align-self-center"} style={{color: "red", textAlign: "center"}}>
                        {email !== "" && !/^[\w-.]+@([\w-]+\.)+[\w-]{2,4}$/gi.test(email) ? "Формат почты неверный" : ""}
                    </p>
                    <Form.Control
                        className="mt-3"
                        placeholder="Пароль"
                        value={password1}
                        onChange={e => setPassword1(e.target.value)}
                        type={"password"}
                    />
                    <Form.Control
                        className="mt-3"
                        placeholder="Повторите пароль"
                        value={password2}
                        onChange={e => setPassword2(e.target.value)}
                        type={"password"}
                    />
                    <p className={"d-flex justify-content-center align-self-center"} style={{color: "red", textAlign: "center"}}>
                        {(password1 !== password2 && password1 !== "" && password2 !== "") ? "Пароли не совпадают" : ""}
                    </p>
                    <Form.Control
                        className="mt-3"
                        placeholder="Имя"
                        value={firstName}
                        onChange={e => setFirstName(e.target.value)}
                        type={"text"}
                    />
                    <Form.Control
                        className="mt-3"
                        placeholder="Фамилия"
                        value={lastName}
                        onChange={e => setLastName(e.target.value)}
                        type={"text"}
                    />
                    <Form.Control
                        className="mt-3"
                        placeholder="Адрес для доставки"
                        value={address}
                        onChange={e => setAddress(e.target.value)}
                        type={"text"}
                    />
                    <InputMask 
                        className={"form-control mt-3"}
                        placeholder="Телефон"
                        mask={"+7 (999) 999 99 99"}
                        value={phone}
                        onChange={e => setPhone(e.target.value)} />
                    <Row className="d-flex justify-content-between mt-3 pl-3 pr-3">
                        <Button ref={regBtn} variant={"success"} onClick={register}>
                            Зарегистрироваться
                        </Button>
                        <Link className={"d-flex justify-content-center align-self-center"} to={"/login"}>Войти в уже существующий аккаунт</Link>
                        <p className={"d-flex justify-content-center align-self-center"} style={{color: "red", textAlign: "center"}}>
                            {isFetchError ? "Произошла техническая ошибка. Пожалуйста попробуйте позднее" : ""}
                            {isFailed ? errorMessage : ""}
                        </p>
                    </Row>
                </Form>
            </Card>
            {showDialog && <DialogBox onOK={regOK} /> }
        </Container>
    );
}

function DialogBox(props) {
    return (
        <div className={"fullItem"}>
            <div>
                <p>На почту, указанную при регистрации, было отправлено письмо со ссылкой на активацию. Перейдите по ней в течение 7 дней для активации</p>
                <button onClick={props.onOK} className={"btn btn-success"}>ОК</button>
            </div>
        </div>
    )
}