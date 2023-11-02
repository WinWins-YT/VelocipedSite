import {Container} from "react-bootstrap";


export default function Help() {
    return (
        <Container className="helpContainer">
            <h3>Как пользоваться сайтом</h3>
            <p>В начале выберите искомый магазин</p>
            <div className="warning">Заказать можно только из одного магазина за раз</div>
            <p>Выберите товары в корзину, их количество можно поменять в корзине</p>
            <div className="warning">У каждого магазина своя цена доставки и своя минимальная цена заказа. С этим можно ознакомиться в корзине при оформлении заказа</div>
            <p>При оформлении заказа можно указать другой адрес и телефон</p>
        </Container>
    )
}