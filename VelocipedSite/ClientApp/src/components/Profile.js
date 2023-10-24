import {useNavigate} from "react-router-dom";
import {useEffect} from "react";

export default function Profile() {
    const token = localStorage.getItem("auth_token");
    const loggedIn = token != null && checkToken(token);
    
    let nav = useNavigate();
    
    useEffect(() => {
        if (!loggedIn)
            nav("/login");

        // eslint-disable-next-line
    }, []);
    
    function checkToken(token) {
        let isValid = false;
        fetch("/api/v1/Profile/CheckToken", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                token: token
            })
        })
            .then(response => response.json())
            .then(json => isValid = json.isValid);
        
        return isValid;
    }
    
    return (
        <div>
            <p>Имя: <input type={"text"}/></p>
            <p>Фамилия: <input type={"text"}/></p>
        </div>
    )
}