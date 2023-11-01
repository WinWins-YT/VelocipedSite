import {Button, Card, Container, Form, Row} from "react-bootstrap";
import React, {useEffect, useRef, useState} from "react";
import {Link, useNavigate} from "react-router-dom";

export default function Login() {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [isFetchError, setIsFetchError] = useState(false);
    const [isFailed, setIsFailed] = useState(false);
    const [errorMessage, setErrorMessage] = useState("");
    const [forgotPassword, setForgotPassword] = useState(false);
    const loginBtn = useRef(null);
    
    document.title = "Вход в аккаунт";
    
    useEffect(() => {
        loginBtn.current.disabled = email === "" || password === "";
    });
    
    async function checkLogin() {
        loginBtn.current.disabled = true;
        
        try {
            const response = await fetch("/api/v1/Profile/Authenticate", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    email: email,
                    password: password
                })
            });
            
            const data = await response.json();
            
            if (!data.isSuccess) {
                loginBtn.current.disabled = false;
                setIsFailed(true);
                setErrorMessage(data.errorMessage);
            }
            else {
                localStorage.setItem("auth_token", data.token);
                window.history.back();
            }
        }
        catch {
            loginBtn.current.disabled = false;
            setIsFetchError(true);
        }
    }
    
    return (
        <Container
            className="d-flex justify-content-center align-items-center">
            <Card style={{width: 600}} className="p-5">
                <h2 className="m-auto">Авторизация</h2>
                <Form className="d-flex flex-column">
                    <Form.Control
                        className="mt-3"
                        placeholder="Email"
                        value={email}
                        onChange={e => setEmail(e.target.value)}
                    />
                    <Form.Control
                        className="mt-3"
                        placeholder="Пароль"
                        value={password}
                        onChange={e => setPassword(e.target.value)}
                        type="password"
                    />
                    <Row className="d-flex justify-content-between mt-3 pl-3 pr-3">
                        <Button ref={loginBtn} variant={"success"} onClick={checkLogin}>
                            Войти
                        </Button>
                        <Link className={"d-flex justify-content-center align-self-center"} to={"#"} onClick={() => setForgotPassword(true)}>Забыли пароль?</Link>
                        <Link className={"d-flex justify-content-center align-self-center"} to={"/register"}>Регистрация</Link>
                        <p className={"d-flex justify-content-center align-self-center"} style={{color: "red", textAlign: "center"}}>
                            {isFetchError ? "Произошла техническая ошибка. Пожалуйста попробуйте позднее" : ""}
                            {isFailed ? errorMessage : ""}
                        </p>
                    </Row>
                </Form>
            </Card>
            {forgotPassword && <DialogBox onClose={() => setForgotPassword(false)} />}
        </Container>
    );
}

function DialogBox(props) {
    const [email, setEmail] = useState("");
    const [isFailed, setIsFailed] = useState(false);
    const [isFetchError, setIsFetchError] = useState(false);
    const [errorMessage, setErrorMessage] = useState("");
    
    const okBtn = useRef(null);
    const nav = useNavigate();
    
    async function changePass() {
        okBtn.current.disabled = true;
        setIsFailed(false);
        setIsFetchError(false);
        
        try {
            const response = await fetch("api/v1/Profile/ForgotPassword", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    email: email
                })
            });
            const data = await response.json();
            
            if (!data.isSuccess) {
                okBtn.current.disabled = false;
                setIsFailed(true);
                setErrorMessage(data.errorMessage);
            }
            else {
                nav('/');
            }
        }
        catch {
            okBtn.current.disabled = false;
            setIsFetchError(true);
        }
    }
    
    return (
        <div className={"fullItem"}>
            <div>
                <p>Введите электронную почту, которую указывали при регистрации. На нее придет новый пароль</p>
                <Form className={"d-flex flex-column"}>
                    <Form.Control
                        className="mt-3"
                        placeholder="Email"
                        value={email}
                        onChange={e => setEmail(e.target.value)} />
                </Form>
                <button onClick={props.onClose} className={"btn btn-danger"}>Отмена</button>
                <button ref={okBtn} onClick={changePass} className={"btn btn-success"}>Далее</button>
                <p className={"d-flex justify-content-center align-self-center"} style={{color: "red", textAlign: "center"}}>
                    {isFetchError ? "Произошла техническая ошибка. Пожалуйста попробуйте позднее" : ""}
                    {isFailed ? errorMessage : ""}
                </p>
            </div>
        </div>
    )
}