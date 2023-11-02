import {Container} from "react-bootstrap";
import {useEffect, useState} from "react";

export default function OrderHistory() {
    const token = localStorage.getItem("auth_token");
    const [orders, setOrders] = useState([]);

    useEffect(() => {
        getOrders();

        // eslint-disable-next-line
    }, []);
    
    async function getOrders() {
        const response = await fetch("/api/v1/Orders/GetOrdersForUser", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                token: token
            })
        });
        const data = await response.json();
        
        setOrders(data.orders);
    }
    
    function getStatus(status) {
        switch (status) {
            case 0:
                return "Создан, ожидает оплаты";
                
            case 1:
                return "Оплачен, в обработке";
                
            case 2:
                return "Доставляется";
                
            case 3:
                return "Завершен";
                
            case 4:
                return "Отменен";
                
            default:
                return "Неизвестен";
        }
    }
    
    async function cancelOrder(id) {
        const response = await fetch("/api/v1/Orders/CancelOrder", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                token: token,
                orderId: id
            })
        });
        window.location.reload();
    }
    
    return (
        <Container>
            {orders.length > 0 ? orders.map((x, index) =>
                <div style={{background: index % 2 === 0 ? "white" : "lightgray"}}>
                    <h3>Заказ №{x.id} от {new Date(x.date).toLocaleString()}</h3>
                    <p>Статус: {getStatus(x.status)}</p>
                    <p>На сумму: {x.totalPrice}</p>
                    {x.saleValue > 0 ? <p>Скидка на сумму: -{x.saleValue}</p> : ""}
                    {x.status !== 3 && x.status !== 4 ?
                        <button className="btn btn-danger" onClick={() => cancelOrder(x.id)}>Отменить</button> : 
                        "" }
                </div>
            ) : 
            <p>Заказы отсутствуют</p>}
        </Container>
    )
}