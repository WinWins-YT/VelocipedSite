import React, {useEffect, useState} from "react";
import {Link} from "react-router-dom";
import {NavMenu} from "./NavMenu";

export default function Category() 
{
    const shopId = new URL(window.location.href).searchParams.get("shop");
    const id = new URL(window.location.href).searchParams.get("id");
    let cart = JSON.parse(localStorage.getItem("cart"));

    const [shopImgUrl, setShopImgUrl] = useState("");
    const [catalogName, setCatalogName] = useState("");
    const [products, setProducts] = useState([]);
    const [loading, setLoading] = useState(true);
    const [reload, setReload] = useState(false);

    useEffect(() => {
        async function load() {
            await getShopAndCatalog(shopId, id);
            await getProducts();
        }
        load();

        // eslint-disable-next-line
    }, []);


    async function getShopAndCatalog(shop, categoryId) {
        let response = await fetch("/api/v1/Shops/GetShopById?id=" + shop);
        let data = await response.json();
        setShopImgUrl(data.shop.pathToImg);
        let shopName = data.shop.name;

        response = await fetch("/api/v1/Catalog/GetCatalogCategoryById?catalogId=" + categoryId);
        data = await response.json();
        setCatalogName(data.catalogCategory.name);

        document.title = `${data.catalogCategory.name} - ${shopName}`;
    }
    
    async function getProducts() {
        const response = await fetch(`/api/v1/Products/GetProductsInCategory?catalogId=${id}&shopId=${shopId}`);
        const data = await response.json();
        setProducts(data.products);
        setLoading(false);
    }
    
    function addToCart(productId) {
        cart = JSON.parse(localStorage.getItem("cart"));
        cart.push({
            id: productId,
            name: "Niggers"
        });
        localStorage.setItem("cart", JSON.stringify(cart));
        setReload(!reload);
        NavMenu.rerender();
    }
    
    function removeFromCart(productId) {
        cart = JSON.parse(localStorage.getItem("cart"));
        cart = cart.filter(x => x.id !== productId);
        localStorage.setItem("cart", JSON.stringify(cart));
        setReload(!reload);
        NavMenu.rerender();
    }
    
    function renderCatalog() {
        return (
            <div className={"row align-items-center"}>
                <p style={{fontSize: 24, fontWeight: "bold"}}>{catalogName}</p>
                {
                    products.map(x =>
                        <div className={"col-md-4 justify-center align-items-center categoryBanner"}>
                            <Link to={"/product?shop=" + shopId + "&id=" + x.id}>
                                <img src={"images/products/" + shopId + "/" + x.pathToImg} alt={""}/>
                            </Link>
                            <p>{x.name}</p>
                            <p>{x.price} руб.</p>
                            {cart.some(e => e.id === x.id)
                                ? <button onClick={() => removeFromCart(x.id)} className="btn btn-outline-danger align-self-center">Удалить из корзины</button>
                                : <button onClick={() => addToCart(x.id)} className="btn btn-primary align-self-center">Добавить в корзину</button>}
                        </div>
                    )
                }
            </div>
        );
    }
    
    return (
        <div>
            <div style={{justifyContent: "center"}} className={"row"}>
                <img className={"shopBanner col-md-4 d-flex align-items-center"} src={"images/" + shopImgUrl} alt={""}/><br/>
            </div>
            {
                loading ? <p><em>Loading...</em></p>
                    : renderCatalog()
            }
        </div>
    )
}