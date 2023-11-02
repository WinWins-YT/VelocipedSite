import {useNavigate} from "react-router-dom";
import React, {useEffect, useRef, useState} from "react";
import {Form} from "react-bootstrap";
import {Button, Container} from "react-bootstrap";
import InputMask from "react-input-mask";

export default function Profile() {
    const token = localStorage.getItem("auth_token");
    
    //const [loggedIn, setLoggedIn] = useState(false);
    //const [user, setUser] = useState({});
    
    const [firstName, setFirstName] = useState("");
    const [lastName, setLastName] = useState("");
    const [email, setEmail] = useState("");
    const [address, setAddress] = useState("");
    const [phone, setPhone] = useState("");
    
    const saveBtn = useRef(null);
    const [isFetchError, setIsFetchError] = useState(false);
    const [isFailed, setIsFailed] = useState(false);
    const [errorMessage, setErrorMessage] = useState("");
    const [changePass, setChangePass] = useState(false);
    
    let nav = useNavigate();
    
    useEffect(() => {
        getUser(token);

        // eslint-disable-next-line
    }, []);
    
    useEffect(() => {
        saveBtn.current.disabled =
            !/^[А-яA-z]+$/g.test(firstName) ||
            !/^[А-яA-z]+$/g.test(lastName) ||
            !/^[A-zА-я-.,/\\0-9 ]+$/g.test(address) ||
            !/^\+7 \(\d{3}\) \d{3} \d{2} \d{2}/g.test(phone);
    });
    
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
                //setLoggedIn(json.isValid);
                if (!json.isValid) {
                    nav("/login");
                    return;
                }
                //setUser(json.user);
                
                setFirstName(json.user.firstName);
                setLastName(json.user.lastName);
                setEmail(json.user.email);
                setAddress(json.user.address);
                setPhone(json.user.phone);
            });
    }
    
    async function logout() {
        localStorage.removeItem("auth_token");
        await fetch("/api/v1/Profile/InvalidateToken", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                token: token
            })
        });
        
        nav('/login');
    }
    
    async function saveChanges() {
        saveBtn.current.disabled = true;
        setIsFailed(false);
        setIsFetchError(false);
        
        try {
            const response = await fetch("/api/v1/Profile/UpdateUser", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    token: token,
                    firstName: firstName,
                    lastName: lastName,
                    address: address,
                    phone: phone
                })
            });
            const data = await response.json();
            
            if (!data.isSuccess) {
                saveBtn.current.disabled = false;
                setIsFailed(true);
                setErrorMessage(data.errorMessage);
            }
            else {
                nav('/');
            }
        }
        catch {
            saveBtn.current.disabled = false;
            setIsFetchError(true);
        }
    }
    
    return (
        <Container
            className="d-flex justify-content-center align-items-center">
            <Form style={{width: "100%"}}>
                <Form.Group className="mb-3" controlId="userForm.firstNameControl">
                    <Form.Label>Имя</Form.Label>
                    <Form.Control value={firstName}
                                  onChange={e => setFirstName(e.target.value)} />
                </Form.Group>
                <Form.Group className="mb-3" controlId="userForm.lastNameControl">
                    <Form.Label>Фамилия</Form.Label>
                    <Form.Control value={lastName}
                                  onChange={e => setLastName(e.target.value)} />
                </Form.Group>
                <Form.Group className="mb-3" controlId="userForm.emailControl">
                    <Form.Label>E-Mail</Form.Label>
                    <Form.Control value={email}
                                  disabled={true}
                                  type={"email"}
                                  onChange={e => setEmail(e.target.value)} />
                </Form.Group>
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
                <Form.Group style={{marginBottom: "16px"}}>
                    <Button ref={saveBtn} variant={"success"} onClick={saveChanges}>Сохранить изменения</Button>
                    <p className={"d-flex justify-content-center align-self-center"} style={{color: "red", textAlign: "center"}}>
                        {isFetchError ? "Произошла техническая ошибка. Пожалуйста попробуйте позднее" : ""}
                        {isFailed ? errorMessage : ""}
                    </p>
                </Form.Group>
                <Form.Group className="mb-3">
                    <Button style={{marginRight: "8px"}} onClick={() => setChangePass(true)} variant={"outline-primary"}>Сменить пароль</Button>
                    <Button style={{marginRight: "8px"}} onClick={() => nav('/order-history')} variant={"outline-primary"}>История заказов</Button>
                    <Button style={{marginRight: "8px"}} onClick={logout} variant={"outline-danger"}>Выйти из аккаунта</Button>
                </Form.Group>
            </Form>
            {changePass && <ChangePasswordDialog token={token} onClose={() => setChangePass(false)} />}
        </Container>
    )
}

function ChangePasswordDialog(props){
    const [oldPass, setOldPass] = useState("");
    const [newPass1, setNewPass1] = useState("");
    const [newPass2, setNewPass2] = useState("");
    const [isFailed, setIsFailed] = useState(false);
    const [isFetchError, setIsFetchError] = useState(false);
    const [errorMessage, setErrorMessage] = useState("");

    const okBtn = useRef(null);
    
    useEffect(() => {
        okBtn.current.disabled = 
            (newPass1 !== newPass2 && newPass1 !== "" && newPass2 !== "")
            || newPass1 === ""
            || newPass2 === ""
            || oldPass === "";
    })
    
    async function changePass() {
        okBtn.current.disabled = true;
        setIsFailed(false);
        setIsFetchError(false);
        
        try {
            const response = await fetch("/api/v1/Profile/ChangeUserPassword", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    token: props.token,
                    oldPassword: oldPass,
                    newPassword: newPass1
                })
            });
            const data = await response.json();
            
            if (!data.isSuccess) {
                okBtn.current.disabled = false;
                setIsFailed(true);
                setErrorMessage(data.errorMessage);
            }
            else {
                props.onClose();
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
                <Form className={"d-flex flex-column"}>
                    <Form.Control
                        className="mt-3"
                        placeholder="Старый пароль"
                        value={oldPass}
                        type={"password"}
                        onChange={e => setOldPass(e.target.value)} />
                    <Form.Control
                        className="mt-3"
                        placeholder="Новый пароль"
                        value={newPass1}
                        type={"password"}
                        onChange={e => setNewPass1(e.target.value)} />
                    <Form.Control
                        className="mt-3"
                        placeholder="Повторите новый пароль"
                        value={newPass2}
                        type={"password"}
                        onChange={e => setNewPass2(e.target.value)} />
                    <p className={"d-flex justify-content-center align-self-center"} style={{color: "red", textAlign: "center"}}>
                        {(newPass1 !== newPass2 && newPass1 !== "" && newPass2 !== "") ? "Пароли не совпадают" : ""}
                    </p>
                </Form>
                <button onClick={props.onClose} className={"btn btn-danger"}>Отмена</button>
                <button style={{marginLeft: "16px"}} ref={okBtn} onClick={changePass} className={"btn btn-success"}>Далее</button>
                <p className={"d-flex justify-content-center align-self-center"} style={{color: "red", textAlign: "center"}}>
                    {isFetchError ? "Произошла техническая ошибка. Пожалуйста попробуйте позднее" : ""}
                    {isFailed ? errorMessage : ""}
                </p>
            </div>
        </div>
    )
}