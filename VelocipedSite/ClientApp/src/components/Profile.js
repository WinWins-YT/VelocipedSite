import {useNavigate} from "react-router-dom";
import {useEffect, useState} from "react";
import {Form} from "react-bootstrap";
import {Card, Container} from "reactstrap";

export default function Profile() {
    const token = localStorage.getItem("auth_token");
    
    const [loggedIn, setLoggedIn] = useState(false);
    const [user, setUser] = useState({});
    
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
                setLoggedIn(json.isValid);
                if (!json.isValid)
                    nav("/login");
                setUser(json.user);
            });
    }
    
    function set(key, value) {
        switch (key) {
            case "firstName":
                user.firstName = value;
                setUser(user);
                break;
                
            case "lastName":
                user.lastName = value;
                setUser(user);
                break;
                
            case "email":
                user.email = value;
                setUser(user);
                break;
                
            case "address":
                user.address = value;
                setUser(user);
                break;
                
            case "phone":
                user.phone = value;
                setUser(user);
                break;
        }
    }
    
    return (
        <Container
            className="d-flex justify-content-center align-items-center">
            <Form>
                <Form.Group className="mb-3" controlId="userForm.firstNameControl">
                    <Form.Label>Имя</Form.Label>
                    <Form.Control value={user.firstName}
                                  onChange={e => set("firstName", e.target.value)} />
                </Form.Group>
                <Form.Group className="mb-3" controlId="userForm.lastNameControl">
                    <Form.Label>Фамилия</Form.Label>
                    <Form.Control value={user.lastName}
                                  onChange={e => set("lastName", e.target.value)} />
                </Form.Group>
                <Form.Group className="mb-3" controlId="userForm.emailControl">
                    <Form.Label>E-Mail</Form.Label>
                    <Form.Control value={user.email}
                                  type={"email"}
                                  onChange={e => set("email", e.target.value)} />
                </Form.Group>
                <Form.Group className="mb-3" controlId="userForm.addressControl">
                    <Form.Label>Адрес</Form.Label>
                    <Form.Control value={user.address}
                                  onChange={e => set("address", e.target.value)} />
                </Form.Group>
                <Form.Group className="mb-3" controlId="userForm.phoneControl">
                    <Form.Label>Номер телефона</Form.Label>
                    <Form.Control value={user.phone}
                                  onChange={e => set("phone", e.target.value)} />
                </Form.Group>
            </Form>
        </Container>
    )
}