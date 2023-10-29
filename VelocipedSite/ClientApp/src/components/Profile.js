import {useNavigate} from "react-router-dom";
import {useEffect, useState} from "react";
import {Form} from "react-bootstrap";
import {Button, Container} from "react-bootstrap";

export default function Profile() {
    const token = localStorage.getItem("auth_token");
    
    //const [loggedIn, setLoggedIn] = useState(false);
    //const [user, setUser] = useState({});
    
    const [firstName, setFirstName] = useState("");
    const [lastName, setLastName] = useState("");
    const [email, setEmail] = useState("");
    const [address, setAddress] = useState("");
    const [phone, setPhone] = useState("");
    
    let nav = useNavigate();
    
    useEffect(() => {
        getUser(token);

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
                    <Form.Control value={phone}
                                  onChange={e => setPhone(e.target.value)} />
                </Form.Group>
                <Form.Group style={{marginBottom: "16px"}}>
                    <Button variant={"success"} onClick={saveChanges}>Сохранить изменения</Button>
                </Form.Group>
                <Form.Group className="mb-3">
                    <Button style={{marginRight: "8px"}} variant={"outline-primary"}>Сменить пароль</Button>
                    <Button style={{marginRight: "8px"}} onClick={logout} variant={"outline-danger"}>Выйти из аккаунта</Button>
                </Form.Group>
            </Form>
        </Container>
    )
}